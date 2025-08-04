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
    private Button leaderboardButton;

    [SerializeField]
    private TextMeshProUGUI eggCount;

    [SerializeField]
    private TextMeshProUGUI timer;

    [SerializeField]
    private TextMeshProUGUI goldCount;

    #endregion

    #region Fields
    
    private readonly HashSet<Window> ActiveWindows = new();

    private bool isPlaying;
    
    #endregion

    public bool IsPlaying
    {
        get => isPlaying;
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

    public void SetEggCount(int count) => eggCount.text = $"{count.ToString()}";

    public void UpdateGoldCount() => goldCount.text = $"{settings.Gold.ToString()}";

    public void UpdateTime(int time) => timer.text  = "00:" + time.ToString("D2");

    private Window GetWindow(WindowType type)
    {
        for (var i = 0; i < windows.Length; i++)
        {
            if (windows[i].type == type)
            {
                return windows[i].window;
            }
        }
        return null;
    }

    private void UpdateBalance(int value) => goldCount.text = value.ToString();

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

    public void ShowLeaderboard(bool isNewHighscore)
    {
        var window = GetWindow(WindowType.LEADERBOARD) as LeaderboardWindow;
        window.SetNewHighScore(isNewHighscore);
        window.Show();
        ActiveWindows.Add(window);

        window.OnClose += OnLeaderboardClose;
    }

    private void OnLeaderboardClick()
    {
        Time.timeScale = 0;

        ShowLeaderboard(false);
    }

    private void OnLeaderboardClose(Window leaderboard)
    {
        leaderboard.OnClose -= OnShopClose;

        if (ActiveWindows.Contains(leaderboard))
        {
            ActiveWindows.Remove(leaderboard);
        }

        if (((LeaderboardWindow)leaderboard).IsNewHighscore)
            ShowResults(settings.HighScore);
        else
            Time.timeScale = 1;
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

    private void Start()
    {
        Localization.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            settings.Gold += 1000;

            UpdateGoldCount();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            settings.Gold += 10000;

            UpdateGoldCount();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            settings.Gold += 100000;

            UpdateGoldCount();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            settings.Gold += 1000000;

            UpdateGoldCount();
        }
    }

    private void OnEnable()
    {
        leaderboardButton.onClick.AddListener(OnLeaderboardClick);
        shopButton.onClick.AddListener(OnShopClick);
        settings.OnGoldChanged += UpdateBalance;
    }

    private void OnDisable()
    {
        leaderboardButton.onClick.RemoveListener(OnLeaderboardClick);
        shopButton.onClick.RemoveListener(OnShopClick);
        settings.OnGoldChanged -= UpdateBalance;
    }

    private void OnDestroy()
    {
        ActiveWindows.Clear();
    }

    #endregion
}
