using UnityEngine;
using TMPro;
using System.Text;

public class LeaderboardWindow : Window
{
    [SerializeField]
    TextMeshProUGUI names;
    [SerializeField]
    TextMeshProUGUI values;
    [SerializeField]
    TextMeshProUGUI highscore;
    [SerializeField]
    TextMeshProUGUI upperText;
    [SerializeField]
    TextMeshProUGUI middleText;
    [SerializeField]
    TextMeshProUGUI bottomText;

    private string[] leaders;
    private readonly int[] scores = {3456,2345,2020,2007,1989,1988,1961,1959,1280,420};

    private int highscoreIndex = int.MaxValue;

    protected override float onCloseDelay => 2f;

    public override void Show()
    {
        leaders ??= Localization.GetLocalLeaders();

        UpdateHighscoreIndex();
        base.Show();
    }

    private void UpdateHighscoreIndex()
    {
        int index = -1;

        for (int i = scores.Length - 1; i >= 0; i--)
        {
            if (scores[i] < settings.HighScore)
            {
                index = i;
            }
        }

        highscoreIndex = index < highscoreIndex ? index : highscoreIndex;
        UpdateText();
    }

    private void UpdateText()
    {
        string name;
        string myName = Localization.Get(Localization.I);
        string value = string.Empty;
        var namesBuilder = new StringBuilder();
        var valueBuilder = new StringBuilder();
        bool hasShifted = false;

        for (int i = 0; i < scores.Length; i++)
        {
            if (i == highscoreIndex)
            {
                name = myName; value = settings.HighScore.ToString();
                hasShifted = true;
            }
            else
            {
                name = hasShifted ? leaders[i-1] : leaders[i];
                value = (hasShifted ? scores[i-1] : scores[i]).ToString();
            }

            switch (i)
            {
                case 0: //gold for 1st place
                    namesBuilder.AppendLine($"<color=#EA8010>{name}</color>");
                    valueBuilder.AppendLine($"<color=#EA8010>{value}</color>");
                    break;
                case 1: //silver for 2nd
                    namesBuilder.AppendLine($"<color=#777>{name}</color>");
                    valueBuilder.AppendLine($"<color=#777>{value}</color>");
                    break;
                case 2: //bronze for 3rd
                    namesBuilder.AppendLine($"<color=#EA3C3C>{name}</color>");
                    valueBuilder.AppendLine($"<color=#EA3C3C>{value}</color>");
                    break;
                default:
                    namesBuilder.AppendLine(name);
                    valueBuilder.AppendLine(value);
                    break;
            }
        }

        names.SetText(namesBuilder);
        values.SetText(valueBuilder);
        highscore.SetText(settings.HighScore.ToString());
        upperText.SetText(Localization.Get(Localization.Highscore));
        middleText.SetText(Localization.Get(Localization.Record));
        bottomText.SetText(Localization.Get(Localization.Continue));
    }
}