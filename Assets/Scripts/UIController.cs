using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    
    private int time;
    private int nextUpdate;

    public bool IsPlaying { get; private set; }

    public void SetEggCount(int count) => eggCount.text = $"{count}";

    public void SetGoldCount(int count) => goldCount.text = $"{count}";

    public event Action OnEndGame;

    public void StartGame()
    {
        time = 60;
        IsPlaying = true;
    }

    private void EndGame()
    {
        IsPlaying = false;
        OnEndGame?.Invoke();
    }

    private void OnShopClick()
    {
        var shop = GameObject.Instantiate(settings.GetScreen("shop"));
        shop.transform.SetParent(transform);
        var rect = shop.GetComponent<RectTransform>();
        rect.sizeDelta = Vector2.zero;
    }

    private void UpdateTime()
    {
        totalCount.text = string.Format("00:{0:D2}", time);
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        shopButton.onClick.AddListener(OnShopClick);
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsPlaying && Time.time >= nextUpdate)
        {
    		nextUpdate = Mathf.FloorToInt(Time.time) + 1;

            UpdateTime();

            if (time == 0)
            {
                EndGame();
            }

            time--;
    	}
    }

    private void OnDisable()
    {
        shopButton.onClick.RemoveListener(OnShopClick);
    }
}
