using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum GameState
{
    AwaitingStart,
    Playing,
    Paused,
    Ended,
    ShowingResults
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameSettings settings;

    [SerializeField]
    UIController ui;

    [SerializeField]
    private int gameDuration = 60;

    private static GameManager instance;
    private static readonly int CHICK_CLUCK = Animator.StringToHash("chicken-cluck");
    private static readonly int EGG_SHAKING = Animator.StringToHash("egg-shaking");
    private Sequence playerAnimation;
    private readonly HashSet<Sequence> eggAnimations = new();
    private readonly HashSet<Sequence> babiesAnimations  = new();
    private GameObject currentPlayer;
    private int eggCount;
    private int time;
    private int nextUpdate;

    private GameState State = GameState.AwaitingStart;

    public static GameManager Instance => instance;

    public void CollectReward()
    {
        settings.Gold += eggCount * settings.GetCreatureReward();
        currentPlayer.GetComponent<AudioSource>().PlayOneShot(settings.GetSound("coins"));
        eggCount = 0;
    }

    public void StartGame()
    {
        time = gameDuration;
        ui.SetEggCount(eggCount);
        ui.StartGame();
        State = GameState.Playing;
    }

    private void EndGame()
    {
        ui.IsPlaying = false;
        ui.UpdateGoldCount();
        ui.SetEggCount(eggCount);
        ui.ShowResults(eggCount);
    }

    private void OnGoldChanged(int value)
    {
        SaveSettings();
    }

    private void OnCreatureChanged(Creature creature)
    {
        DestroyPlayer();
        CreatePlayer();
        SaveSettings();
    }

    void Awake()
    {
        instance = this;

        DOTween.Init();
        LoadSettings();
    }

    void Start()
    {
        CreatePlayer();
        ui.ShowStart();
        ui.UpdateGoldCount();
    }

    void OnEnable()
    {
        settings.OnGoldChanged += OnGoldChanged;
        settings.OnActiveCreatureChanged += OnCreatureChanged;
    }

    void OnDisable()
    {
        settings.OnGoldChanged -= OnGoldChanged;
        settings.OnActiveCreatureChanged -= OnCreatureChanged;
    }

    void Update()
    {
        if (!ui.IsPlaying)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch t in Input.touches)
            {
                if (t.phase == TouchPhase.Ended)
                {
                    HandleTouch(t.position);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleTouch(Input.mousePosition);
        }

        if (Time.time >= nextUpdate)
        {
    		nextUpdate = Mathf.FloorToInt(Time.time) + 1;

            ui.UpdateTime(time);

            if (time == 0)
            {
                EndGame();
            }

            time--;
    	}
    }

    void OnDestroy()
    {
        playerAnimation?.Kill();

        DestroyAnimation(eggAnimations);
        DestroyAnimation(babiesAnimations);
    }

    private void DestroyAnimation(HashSet<Sequence> animations)
    {
        if (animations != null && animations.Count > 0)
        {
            foreach (var animation in animations)
            {
                animation.Kill();
            }
            animations.Clear();
        }
    }

    private void HandleTouch(Vector3 position)
    {
        Vector2 position2D = Camera.main.ScreenToWorldPoint(position);
        MovePlayer(position2D);
        CreateEgg(position2D);
    }

    #region Settings

    private void LoadSettings()
    {
        settings.Gold = PlayerPrefs.GetInt("gold");
        var name = PlayerPrefs.GetString("activeCreature");
        if (string.IsNullOrEmpty(name))
        {
            name = "chicken";
        }
        settings.SetActiveCreature(name);

        var purchasesData = PlayerPrefs.GetInt("purchased");
        if (purchasesData == 0) purchasesData = (int) CreatureType.CHICKEN;
        settings.SetPurchasedCreatures(purchasesData);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("gold", settings.Gold);
        PlayerPrefs.SetString("activeCreature", currentPlayer.name);
        PlayerPrefs.SetInt("purchased", settings.GetPurchasedCreatures);
    }

    #endregion

    #region Player

    private void CreatePlayer()
    {
        var prefab = settings.GetActiveCreaturePrefab();
        currentPlayer = Instantiate(prefab, transform.position, transform.rotation);
        currentPlayer.name = prefab.name;
    }

    private void MovePlayer(Vector3 position)
    {
        playerAnimation?.Kill();

        currentPlayer.transform.position = position;

        Sequence animation = DOTween.Sequence();
        animation.AppendCallback(() => currentPlayer.GetComponent<AudioSource>().PlayOneShot(settings.GetSound("down_egg")));
        animation.Append(currentPlayer.transform.DOMoveY(position.y + 1f,0.33f));
        animation.Append(currentPlayer.transform.DOMoveX(position.x + 1f,0.33f));
        animation.Append(currentPlayer.transform.DOMoveY(position.y,0.33f));
        animation.AppendCallback(() => currentPlayer.GetComponent<Animator>().Play(CHICK_CLUCK));
        animation.AppendCallback(() => currentPlayer.GetComponent<AudioSource>().PlayOneShot(settings.GetSound(currentPlayer.name)));

        playerAnimation = animation;
    }

    private void DestroyPlayer()
    {
        Destroy(currentPlayer);
        foreach (var animation in eggAnimations) animation.Kill();
        foreach (var animation in babiesAnimations) animation.Kill();
        foreach (Transform child in transform) Destroy(child.gameObject);
    }

    #endregion

    private void CreateEgg(Vector2 position)
    {
        var egg = Instantiate(settings.GetEggPrefab(), position, Quaternion.identity);
        egg.transform.parent = transform;

        Sequence animation = DOTween.Sequence();
        animation.AppendInterval(5f);
        animation.AppendCallback(() => egg.GetComponent<Animator>().Play(EGG_SHAKING));
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => {
            var sound = egg.GetComponent<AudioSource>();
            sound.PlayOneShot(sound.clip);
        });
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => Destroy(egg));
        animation.AppendCallback(() => eggAnimations.Remove(animation));
        animation.AppendCallback(() => CreateBaby(position));

        eggAnimations.Add(animation);

        ui.SetEggCount(eggCount++);
    }

    private void CreateBaby(Vector2 position)
    {
        var baby = Instantiate(settings.GetActiveCreaturePrefab(), position, Quaternion.identity);
        baby.transform.localScale = Vector2.one * 0.5f;
        baby.transform.parent = transform;
        Sequence animation = DOTween.Sequence();
        animation.AppendCallback(() => baby.GetComponent<Animator>().Play(CHICK_CLUCK));
        animation.AppendCallback(() => {
            var sound = baby.GetComponent<AudioSource>();
            sound.PlayOneShot(sound.clip);
        });
        animation.Append(baby.transform.DOMoveX(-5, 5f));
        animation.AppendCallback(() => Destroy(baby));
        animation.AppendCallback(() => babiesAnimations.Remove(animation));

        babiesAnimations.Add(animation);
    }
}
