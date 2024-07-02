using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameSettings settings;

    [SerializeField]
    private Button shopButton;

    [SerializeField]
    private TextMeshProUGUI eggCount;

    [SerializeField]
    private TextMeshProUGUI totalCount;

    [SerializeField]
    private TextMeshProUGUI goldCount;

    [SerializeField]
    TextMeshProUGUI txtTapToStart;

    private bool isPlaying;

    public bool IsPlaying
    {
        get { return isPlaying; }
        set { if (Windows.Count == 0) isPlaying = value; }
    }

    public void SetEggCount(int count) => eggCount.text = $"{count}";

    public void UpdateGoldCount() => goldCount.text = $"{settings.Gold}";

    private readonly HashSet<Window> Windows = new();

    public void StartGame()
    {
        IsPlaying = true;
        txtTapToStart.gameObject.SetActive(false);
    }

    public void UpdateTime(int time)
    {
        totalCount.text = string.Format("00:{0:D2}", time);
    }

    public void EndGame()
    {
        IsPlaying = false;
    }

    public void ShowStart()
    {
        var startWindow = Instantiate(settings.GetScreen(nameof(StartWindow)), transform) as StartWindow;
        startWindow.OnClose += OnStartClose;

        Windows.Add(startWindow);

        txtTapToStart.gameObject.SetActive(true);
    }

    private void OnStartClose(Window startWindow)
    {
        startWindow.OnClose -= OnStartClose;

        if (Windows.Contains(startWindow))
        {
            Windows.Remove(startWindow);
        }
        
        IsPlaying = true;

        txtTapToStart.gameObject.SetActive(false);

        GameManager.Instance.StartGame();
    }

    public void ShowResults(int eggsCollected)
    {
        var resultsWindow = Instantiate(settings.GetScreen(nameof(ResultsWindow)), transform) as ResultsWindow;
        var text = string.Format(Localization.Get(Localization.Results), eggsCollected, eggsCollected * settings.GetCreatureReward());
        resultsWindow.SetText(text);
        resultsWindow.OnClose += OnResultsClose;

        Windows.Add(resultsWindow);
    }

    private void OnResultsClose(Window results)
    {
        results.OnClose -= OnResultsClose;

        if (Windows.Contains(results))
        {
            Windows.Remove(results);
        }
        GameManager.Instance.CollectReward();

        ShowStart();
    }

    private void UpdateBalance(int value)
    {
        goldCount.text = value.ToString();
    }

    private void OnShopClick()
    {
        IsPlaying = false;
 
        var shopWindow = Instantiate(settings.GetScreen(nameof(ShopWindow)), transform) as ShopWindow;
        shopWindow.OnClose += OnShopClose;
        var rect = shopWindow.GetComponent<RectTransform>();
        rect.sizeDelta = Vector2.zero;

        Windows.Add(shopWindow);
    }

    private void OnShopClose(Window shop)
    {
        shop.OnClose -= OnShopClose;

        if (Windows.Contains(shop))
        {
            Windows.Remove(shop);
        }

        IsPlaying = true;
    }

    private void Awake()
    {
        Localization.Initialize();
    }

    private void Start()
    {
        txtTapToStart.text = Localization.Get(Localization.Intro);
    }

    private void OnEnable()
    {
        shopButton.onClick.AddListener(OnShopClick);
        settings.OnGoldChanged += UpdateBalance;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.K))
        {
            settings.Gold += 1000;

            UpdateGoldCount();
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D))
        {
            settings.Gold += 10000;

            UpdateGoldCount();
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.T))
        {
            settings.Gold += 100000;

            UpdateGoldCount();
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.M))
        {
            settings.Gold += 100000;

            UpdateGoldCount();
        }
    }

    private void OnDisable()
    {
        shopButton.onClick.RemoveListener(OnShopClick);
        settings.OnGoldChanged -= UpdateBalance;
    }

    private void OnDestroy()
    {
        Windows.Clear();
    }
}
