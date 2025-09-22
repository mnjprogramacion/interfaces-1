using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectTransform))]
public class SnapAnchorsToGrid : Editor
{
    private const float gridSize = 0.1f; // Tamaño de la cuadrícula (10x10)

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Snap Anchors to Grid"))
        {
            SnapAnchors((RectTransform)target);
        }
    }

    private void SnapAnchors(RectTransform rectTransform)
    {
        Undo.RecordObject(rectTransform, "Snap Anchors to Grid");

        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;

        anchorMin.x = Mathf.Round(anchorMin.x / gridSize) * gridSize;
        anchorMin.y = Mathf.Round(anchorMin.y / gridSize) * gridSize;
        anchorMax.x = Mathf.Round(anchorMax.x / gridSize) * gridSize;
        anchorMax.y = Mathf.Round(anchorMax.y / gridSize) * gridSize;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        EditorUtility.SetDirty(rectTransform);
    }
}