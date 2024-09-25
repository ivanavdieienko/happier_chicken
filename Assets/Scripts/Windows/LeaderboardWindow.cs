using UnityEngine;
using System.Text;

public class LeaderboardWindow : Window
{
    [SerializeField]
    TextMesh names;
    [SerializeField]
    TextMesh values;
    [SerializeField]
    TextMesh highscore;

    private string[] leaders;
    private readonly int[] scores = {4010, 1940, 897, 656, 453, 313, 137, 132, 114, 99 };

    private int highscoreIndex = int.MaxValue;

    protected override float onCloseDelay => 3f;

    public override void Show()
    {
        leaders ??= Localization.Leaders;

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
        string name = string.Empty;
        string value = string.Empty;
        var namesBuilder = new StringBuilder();
        var valueBuilder = new StringBuilder();
        bool hasShifted = false;

        for (int i = 0; i < scores.Length; i++)
        {
            if (i == highscoreIndex)
            {
                name = ArabicFixerTool.FixLine(Localization.I);
                value = ArabicFixerTool.FixLine(settings.HighScore.ToString());

                hasShifted = true;
            }
            else
            {
                name = ArabicFixerTool.FixLine(hasShifted ? leaders[i-1] : leaders[i]);
                value = ArabicFixerTool.FixLine((hasShifted ? scores[i-1] : scores[i]).ToString());
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

        names.text = namesBuilder.ToString();
        values.text = valueBuilder.ToString();
        highscore.text = ArabicFixerTool.FixLine(settings.HighScore.ToString());
    }
}