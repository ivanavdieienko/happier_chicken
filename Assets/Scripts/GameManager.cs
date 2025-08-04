using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameSettings settings;

    [SerializeField]
    UIController ui;

    [SerializeField]
    private int gameDuration = 60;

    private static GameManager instance;
    private Camera mainCamera;
    private static readonly int RUNS = Animator.StringToHash("runs");
    private static readonly int CLUCK = Animator.StringToHash("cluck");
    private static readonly int EGG_IDLE = Animator.StringToHash("egg-idle");
    private static readonly int EGG_SHAKING = Animator.StringToHash("egg-shaking");
    private Sequence playerAnimation;
    private readonly HashSet<Sequence> eggAnimations = new();
    private readonly HashSet<Sequence> babiesAnimations  = new();
    private GameObject currentPlayer;
    private int eggCount;
    private int time;
    private int nextUpdate;
    private ObjectPool<GameObject> eggsPool;
    private ObjectPool<GameObject> babyPool;

    // public SystemLanguage systemLanguage;

    public static GameManager Instance => instance;

    public void CollectReward(int multiplier = 1)
    {
        settings.Gold += eggCount * settings.GetCreatureReward() * multiplier;
        PlaySound("coins");
        eggCount = 0;
    }

    public void StartGame()
    {
        time = gameDuration;
        ui.SetEggCount(eggCount);
        ui.IsPlaying = true;
    }

    private void EndGame()
    {
        ui.IsPlaying = false;
        ui.UpdateGoldCount();
        ui.SetEggCount(eggCount);
        if (eggCount > settings.HighScore)
        {
            settings.UpdateHighscore(eggCount);
            ui.ShowLeaderboard(true);
        }
        else
        {
            ui.ShowResults(eggCount);
        }
    }

    public void PlaySound(string soundName)
    {
        currentPlayer.GetComponent<AudioSource>().PlayOneShot(settings.GetSound(soundName));
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

    #region Unity lifecycle

    void Awake()
    {
        instance = this;
        mainCamera = Camera.main;

        DOTween.Init();
        LoadSettings();

        eggsPool = new ObjectPool<GameObject>(()=> Instantiate(settings.GetEggPrefab(), transform));
        babyPool = new ObjectPool<GameObject>(()=> Instantiate(settings.GetActiveCreaturePrefab(), transform));
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
            for (var index = 0; index < Input.touchCount; index++)
            {
                var t = Input.GetTouch(index);
                if (t.phase == TouchPhase.Ended)
                {
                    HandleTouch(t.position);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
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

    #endregion

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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 position3D = mainCamera.ScreenToWorldPoint(position);
        MovePlayer(position3D);
        CreateEgg(position3D);
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

        settings.SetPurchasedCreatures(PlayerPrefs.GetInt("purchased"));
        settings.UpdateHighscore(PlayerPrefs.GetInt("highscore"));
        settings.SetRateUsShown(PlayerPrefs.GetInt("rate_us"));
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("gold", settings.Gold);
        PlayerPrefs.SetString("activeCreature", currentPlayer.name);
        PlayerPrefs.SetInt("purchased", settings.PurchasedCreatures);
        PlayerPrefs.SetInt("highscore", settings.HighScore);
    }

    #endregion

    #region Player

    private void CreatePlayer()
    {
        var prefab = settings.GetActiveCreaturePrefab();
        currentPlayer = Instantiate(prefab);
        currentPlayer.name = prefab.name;
        eggsPool = new ObjectPool<GameObject>(()=> Instantiate(settings.GetEggPrefab(), transform));
        babyPool = new ObjectPool<GameObject>(()=> Instantiate(settings.GetActiveCreaturePrefab(), transform));
    }

    private void MovePlayer(Vector3 position)
    {
        playerAnimation?.Kill();

        currentPlayer.transform.position = position;

        Sequence animation = DOTween.Sequence();
        animation.AppendCallback(() => PlaySound("down_egg"));
        animation.Append(currentPlayer.transform.DOMoveY(position.y + 1f,0.33f));
        animation.Append(currentPlayer.transform.DOMoveX(position.x + 1f,0.33f));
        animation.Append(currentPlayer.transform.DOMoveY(position.y,0.33f));
        animation.AppendCallback(() => currentPlayer.GetComponent<Animator>().Play(CLUCK));
        animation.AppendCallback(() => PlaySound(currentPlayer.name));

        playerAnimation = animation;
    }

    private void DestroyPlayer()
    {
        Destroy(currentPlayer);
        foreach (var animation in eggAnimations) animation.Kill();
        foreach (var animation in babiesAnimations) animation.Kill();
        foreach (Transform child in transform) Destroy(child.gameObject);
        eggsPool.Clear();
        babyPool.Clear();
    }

    #endregion

    private void CreateEgg(Vector3 position)
    {
        var egg = eggsPool.Get();
        egg.transform.position = position;
        egg.SetActive(true);
        var animator = egg.GetComponent<Animator>();
        animator.Play(EGG_IDLE);

        Sequence animation = DOTween.Sequence();
        animation.AppendInterval(5f);
        animation.AppendCallback(() => animator.Play(EGG_SHAKING));
        animation.AppendInterval(0.3f);
        animation.AppendCallback(() =>
        {
            var sound = egg.GetComponent<AudioSource>();
            sound.PlayOneShot(sound.clip);
        });
        animation.AppendInterval(0.7f);
        animation.AppendCallback(() =>
        {
            egg.SetActive(false);
            eggsPool.Release(egg);
            eggAnimations.Remove(animation);
            CreateBaby(position);
        });

        eggAnimations.Add(animation);

        ui.SetEggCount(++eggCount);
    }

    private void CreateBaby(Vector3 position)
    {
        var baby = babyPool.Get();
        baby.transform.position = position;
        baby.transform.localScale = Vector2.one * 0.5f;
        baby.transform.parent = transform;
        baby.SetActive(true);
        Sequence animation = DOTween.Sequence();
        animation.AppendCallback(() =>
        {
            baby.GetComponent<Animator>().Play(RUNS);
            var sound = baby.GetComponent<AudioSource>();
            sound.PlayOneShot(sound.clip);
        });
        animation.Append(baby.transform.DOMoveX(-3.5f, 5f));
        animation.AppendCallback(() =>
        {
            baby.transform.localScale = Vector2.one;
            baby.SetActive(false);
            babyPool.Release(baby);
            babiesAnimations.Remove(animation);
        });

        babiesAnimations.Add(animation);
    }
}
