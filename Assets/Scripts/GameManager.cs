using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // [SerializeField]
    private GameObject currentPlayer;

    [SerializeField]
    GameSettings settings;

    [SerializeField]
    UIController ui;

    private static readonly int CHICK_CLUCK = Animator.StringToHash("chicken-cluck");
    private static readonly int EGG_SHAKING = Animator.StringToHash("egg-shaking");
    private Sequence playerAnimation;
    private HashSet<Sequence> eggAnimations;
    private HashSet<Sequence> babiesAnimations;
    private int eggCount;

    private void StartGame()
    {
        eggCount = 0;
        ui.SetEggCount(eggCount);
        ui.StartGame();
    }

    private void OnEndGame()
    {
        settings.gold += eggCount * settings.GetCreatureReward();

        ui.SetGoldCount(settings.gold);
        currentPlayer.GetComponent<AudioSource>().PlayOneShot(settings.GetSound("coins"));

        settings.SaveSettings();
    }

    void Awake()
    {
        DOTween.Init();
        eggAnimations = new HashSet<Sequence>();
        babiesAnimations = new HashSet<Sequence>();

        currentPlayer = GameObject.Instantiate(settings.GetActiveCreaturePrefab(), transform.position, transform.rotation);

        ui.SetGoldCount(settings.gold);
        StartGame();
    }

    void OnEnable()
    {
        ui.OnEndGame += OnEndGame;
    }

    void OnDisable()
    {
        ui.OnEndGame -= OnEndGame;
    }

    // Update is called once per frame
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
        animation.AppendCallback(() => currentPlayer.GetComponent<AudioSource>().PlayOneShot(settings.GetSound("big")));

        playerAnimation = animation;
    }

    private void CreateEgg(Vector2 position)
    {
        var egg = Instantiate(settings.GetEggPrefab(), position, Quaternion.identity);

        Sequence animation = DOTween.Sequence();
        animation.AppendInterval(5f);
        animation.AppendCallback(() => egg.GetComponent<Animator>().Play(EGG_SHAKING));
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => {
            var sound = egg.GetComponent<AudioSource>();
            sound.PlayOneShot(sound.clip);
        });
        animation.AppendInterval(0.5f);
        animation.AppendCallback(() => GameObject.DestroyImmediate(egg));
        animation.AppendCallback(() => CreateBaby(position));

        eggAnimations.Add(animation);

        ui.SetEggCount(++eggCount);
    }

    private void CreateBaby(Vector2 position)
    {
        var baby = GameObject.Instantiate(settings.GetActiveCreaturePrefab(), position, Quaternion.identity);
        baby.transform.localScale = Vector2.one * 0.5f;
        Sequence animation = DOTween.Sequence();
        animation.AppendCallback(() => baby.GetComponent<Animator>().Play(CHICK_CLUCK));
        animation.AppendCallback(() => {
            var sound = baby.GetComponent<AudioSource>();
            sound.PlayOneShot(sound.clip);
        });
        animation.Append(baby.transform.DOMoveX(-5, 5f));
        animation.AppendCallback(() => GameObject.DestroyImmediate(baby));

        babiesAnimations.Add(animation);
    }
}
