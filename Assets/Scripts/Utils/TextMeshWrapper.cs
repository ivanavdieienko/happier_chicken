using System.Text;
using UnityEngine;

public class TextMeshWrapper
{
    public static void WrapText(TextMesh input, int maxLength = 20)
    {
        var words = input.text.Split(' ');
        var result = new StringBuilder();
        var line = "";

        foreach (string word in words)
        {
            if (line.Length + word.Length > maxLength)
            {
                result.AppendLine(line);
                line = "";
            }

            line += (line.Length > 0 ? " " : "") + word;
        }

        result.Append(line);
        input.text = result.ToString();
    }
}
