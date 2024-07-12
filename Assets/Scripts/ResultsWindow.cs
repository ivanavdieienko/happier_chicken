using TMPro;
using UnityEngine;

public class ResultsWindow : Window
{
    [SerializeField]
    private TextMeshProUGUI results;

    [SerializeField]
    private RewardedAdsButton btnCollectX2;

    [SerializeField]
    private Animator animator;

    private int reward;
    private int multiplier;
    private int eggsCollected;

    public int Multiplier => multiplier;

    public void ShowResults(int eggsCollected, int reward)
    {
        this.reward = reward;
        this.eggsCollected = eggsCollected;
        this.multiplier = 1;
        btnCollectX2.Enable();

        UpdateRewardsText();
        Show();
        AppRating.Instance.RateAndReview();
    }

    private void UpdateRewardsText()
    {
        results.text = string.Format(Localization.Get(Localization.Results), eggsCollected, eggsCollected * reward * multiplier);
    }

    private void OnGrantReward()
    {
        btnCollectX2.OnGrantReward -= OnGrantReward;
        btnCollectX2.Disable();

        multiplier = 2;

        UpdateRewardsText();
    }

    private void OnAdsInitialized()
    {
        btnCollectX2.LoadAd();
    }

    private void Awake()
    {
        bool adsInitialized = AdsInitializer.Instance.AdsInitialized;
        if (!adsInitialized)
        {
            AdsInitializer.Instance.OnInitialized += OnAdsInitialized;
        }
        else
        {
            OnAdsInitialized();
        }
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        animator.Play("btn-appear");

        var adsInitialized = AdsInitializer.Instance.AdsInitialized;
        if (adsInitialized)
        {
            btnCollectX2.OnGrantReward += OnGrantReward;
        }
        else{

        }
        btnCollectX2.gameObject.SetActive(adsInitialized);
    }

    override protected void OnDisable()
    {
        base.OnDisable();

        btnCollectX2.OnGrantReward -= OnGrantReward;
    }
}