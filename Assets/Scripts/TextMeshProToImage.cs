#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using TMPro;
public class TextToSpriteEditor : MonoBehaviour
{
    [MenuItem("Tools/Convert Selected TextMeshPro to PNG")]
    public static void ConvertSelectedTextToPNG()
    {
        // Get the selected TextMeshPro object
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null || selectedObject.GetComponent<TextMeshProUGUI>() == null)
        {
            Debug.LogError("Please select a TextMeshPro object in the scene.");
            return;
        }

        TextMeshProUGUI textMeshPro = selectedObject.GetComponent<TextMeshProUGUI>();

        // Prompt the user for a save path
        string path = EditorUtility.SaveFilePanel("Save TextMeshPro as PNG", "Assets", "TextSprite", "png");
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Save path is invalid.");
            return;
        }

        // Create a new camera
        Camera renderCamera = new GameObject("RenderCamera").AddComponent<Camera>();

        // Create a new Canvas and set it to World Space
        Canvas canvas = new GameObject("Canvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(textMeshPro.rectTransform.rect.width, textMeshPro.rectTransform.rect.height);

        // Store the original parent and position of the TextMeshPro object
        Transform originalParent = textMeshPro.transform.parent;
        Vector3 originalPosition = textMeshPro.transform.localPosition;

        // Set the TextMeshPro object as a child of the Canvas
        textMeshPro.transform.SetParent(canvas.transform, false);

        // Center the TextMeshPro object within the Canvas
        textMeshPro.rectTransform.anchoredPosition = Vector2.zero;

        // Calculate the size of the RenderTexture based on the TextMeshPro object's size
        int width = Mathf.CeilToInt(textMeshPro.rectTransform.rect.width);
        int height = Mathf.CeilToInt(textMeshPro.rectTransform.rect.height);
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        renderCamera.targetTexture = renderTexture;

        // Position the camera to capture the TextMeshPro object
        renderCamera.transform.position = canvas.transform.position - new Vector3(0, 0, 10);
        renderCamera.orthographic = true;
        renderCamera.orthographicSize = canvasRect.rect.height / 2;
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = Color.clear;

        // Render the TextMeshPro object to the RenderTexture
        renderCamera.Render();

        // Convert the RenderTexture to a Texture2D
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Save the Texture2D as a PNG file
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);

        // Restore the original parent and position of the TextMeshPro object
        textMeshPro.transform.SetParent(originalParent, false);
        textMeshPro.transform.localPosition = originalPosition;

        // Clean up
        DestroyImmediate(renderCamera.gameObject);
        DestroyImmediate(canvas.gameObject);

        Debug.Log($"PNG created and saved as '{path}'");
    }
}
#endif