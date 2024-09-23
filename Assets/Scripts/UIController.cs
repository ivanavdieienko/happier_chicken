using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;

public enum WindowType
{
    NONE,
    START,
    SHOP,
    RESULTS,
    SETTINGS,
    RATEUS,
    LEADERBOARD
}

[Serializable]
public struct WindowData
{
    public WindowType type;

    public Window window;
}

public class UIController : MonoBehaviour
{
    #region Serialized fields

    [SerializeField]
    private GameSettings settings;

    [SerializeField]
    private WindowData[] windows;

    [SerializeField]
    private Button shopButton;

    [SerializeField]
    private TextMeshProUGUI eggCount;

    [SerializeField]
    private TextMeshProUGUI totalCount;

    [SerializeField]
    private TextMeshProUGUI goldCount;

    #endregion

    private readonly HashSet<Window> ActiveWindows = new();

    private bool isPlaying;

    #region Fields

    public bool IsPlaying
    {
        get { return isPlaying; }
        set
        {
            if (ActiveWindows.Count == 0)
            {
                isPlaying = value;
                Application.targetFrameRate = 60;
            }
            else
            {
                Application.targetFrameRate = 30;
            }
        }
    }

    public void SetEggCount(int count) => eggCount.text = $"{count}";

    public void UpdateGoldCount() => goldCount.text = $"{settings.Gold}";

    public void UpdateTime(int time) => totalCount.text = string.Format("00:{0:D2}", time);

    private Window GetWindow(WindowType type) => windows.Where(data => data.type == type).First().window;

    private void UpdateBalance(int value) => goldCount.text = value.ToString();

    #endregion

    #region Start Window

    public void ShowStart()
    {
        var startWindow = GetWindow(WindowType.START) as StartWindow;
        startWindow.OnClose += OnStartClose;
        startWindow.Show();

        ActiveWindows.Add(startWindow);
    }

    private void OnStartClose(Window startWindow)
    {
        startWindow.OnClose -= OnStartClose;

        if (ActiveWindows.Contains(startWindow))
        {
            ActiveWindows.Remove(startWindow);
        }
        
        IsPlaying = true;

        GameManager.Instance.StartGame();
    }

    #endregion

    #region Results

    public void ShowResults(int eggsCollected)
    {
        var resultsWindow = GetWindow(WindowType.RESULTS) as ResultsWindow;
        resultsWindow.OnClose += OnResultsClose;
        resultsWindow.ShowResults(eggsCollected, settings.GetCreatureReward());

        ActiveWindows.Add(resultsWindow);
    }

    private void OnResultsClose(Window window)
    {
        window.OnClose -= OnResultsClose;

        if (ActiveWindows.Contains(window))
        {
            ActiveWindows.Remove(window);
        }

        var results = window as ResultsWindow;

        GameManager.Instance.CollectReward(results.Multiplier);

        ShowStart();
    }

    #endregion

    #region Leaderboard

    public void ShowLeaderboard()
    {
        var window = GetWindow(WindowType.LEADERBOARD) as LeaderboardWindow;
        window.OnClose += OnLeaderboardClose;
        window.Show();

        ActiveWindows.Add(window);
    }

    private void OnLeaderboardClose(Window leaderboard)
    {
        leaderboard.OnClose -= OnShopClose;

        if (ActiveWindows.Contains(leaderboard))
        {
            ActiveWindows.Remove(leaderboard);
        }

        ShowResults(settings.HighScore);
    }

    #endregion

    #region Shop

    private void OnShopClick()
    {
        IsPlaying = false;
 
        var shopWindow = GetWindow(WindowType.SHOP) as ShopWindow;
        shopWindow.OnClose += OnShopClose;
        shopWindow.Show();

        ActiveWindows.Add(shopWindow);
    }

    private void OnShopClose(Window shop)
    {
        shop.OnClose -= OnShopClose;

        if (ActiveWindows.Contains(shop))
        {
            ActiveWindows.Remove(shop);
        }

        IsPlaying = true;
    }

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        Localization.Initialize();
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
            settings.Gold += 1000000;

            UpdateGoldCount();
        }
    }

    private void OnEnable()
    {
        shopButton.onClick.AddListener(OnShopClick);
        settings.OnGoldChanged += UpdateBalance;
    }

    private void OnDisable()
    {
        shopButton.onClick.RemoveListener(OnShopClick);
        settings.OnGoldChanged -= UpdateBalance;
    }

    private void OnDestroy()
    {
        ActiveWindows.Clear();
    }

    #endregion
}
