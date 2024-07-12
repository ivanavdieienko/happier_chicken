using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    private int intentsCount = 1;

    [HideInInspector]
    public bool AdsInitialized = false;

    public event System.Action OnInitialized;

    public static AdsInitializer Instance;

    private void Awake()
    {
        Instance = this;
        InitializeAds();
    }

    public void InitializeAds()
    {
    #if UNITY_IOS
            _gameId = _iOSGameId;
    #elif UNITY_ANDROID
            _gameId = _androidGameId;
    #elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        AdsInitialized = true;

        OnInitialized?.Invoke();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");

        StartCoroutine(WaitAndReInit());
    }

    private IEnumerator WaitAndReInit()
    {
        yield return new WaitForSeconds(10 * intentsCount++);
        
        InitializeAds();
    }
}