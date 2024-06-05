using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsWindow : Window
{
    [SerializeField]
    private TextMeshProUGUI results;

    public void SetText(string text)
    {
        results.text = text;
    }
}