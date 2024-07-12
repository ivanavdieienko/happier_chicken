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

    private const string PLAYER = "Me ♥";

    private readonly string[] leaders = {"Hanna","William","Barbara","Joseph","Halyna","Iván","Lubov","Elena","Georgedan","Michael"};
    private readonly int[] scores = {3456,2345,2020,2007,1989,1988,1961,1959,1280,420};

    private int highscoreIndex = int.MaxValue;

    public override void Show()
    {
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
        string value = string.Empty;
        var namesBuilder = new StringBuilder();
        var valueBuilder = new StringBuilder();
        bool hasShifted = false;

        for (int i = 0; i < scores.Length; i++)
        {
            if (i == highscoreIndex)
            {
                name = PLAYER; value = settings.HighScore.ToString();
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
    }
}