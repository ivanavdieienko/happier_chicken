using TMPro;
using UnityEngine;

public class StartWindow : Window
{
    [SerializeField] TextMeshProUGUI tapToShop;
    [SerializeField] TextMeshProUGUI tapToStart;

    private void Start()
    {
        tapToShop.text = Localization.Get(Localization.Market);
        tapToStart.text = Localization.Get(Localization.Intro);
    }
}