using UnityEditor;
using UnityEngine;

public class HierarchyDividers : Editor
{
    [InitializeOnLoadMethod]
    static void OnProjectLoadedInEditor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj != null && obj.name.Contains("---"))
        {
            Color startColor = Color.gray;
            Color endColor = Color.gray;

            if (obj.name.Contains("---!?"))
            {
                startColor = new Color(0.6f, 0, 0.6f);
                endColor = Color.magenta;
            }
            else if (obj.name.Contains("---!"))
            {
                startColor = new Color(0.6f, 0, 0);
                endColor = Color.red;
            }
            else if (obj.name.Contains("---?"))
            {
                startColor = new Color(0, 0, 0.6f);
                endColor = Color.blue;
            }
            else if (obj.name.Contains("---/"))
            {
                startColor = new Color(0, 0.6f, 0);
                endColor = Color.green;
            }
            else if (obj.name.Contains("--->"))
            {
                startColor = new Color(212f / 255f, 175f / 255f, 55f / 255f);
                endColor = Color.yellow;
            }

            DrawRoundedGradientRect(selectionRect, startColor, endColor);

            string cleanedName = obj.name.Replace("---!?", "").Replace("---!", "").Replace("---?", "").Replace("--->", "").Replace("---/", "").Replace("---", "").Trim();

            GUIStyle style = new(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter,
                richText = true
            };
            style.normal.textColor = Color.white;
            style.clipping = TextClipping.Clip;

            Vector2 textSize = style.CalcSize(new GUIContent(cleanedName));
            Rect centeredRect = new(selectionRect.x + (selectionRect.width - textSize.x) / 2, selectionRect.y, textSize.x, selectionRect.height);

            DrawTextWithOutline(centeredRect, cleanedName, style, Color.black, Color.white);
        }
    }

    static void DrawTextWithOutline(Rect rect, string text, GUIStyle style, Color outlineColor, Color textColor)
    {
        Color originalColor = style.normal.textColor;

        style.normal.textColor = outlineColor;
        EditorGUI.LabelField(new Rect(rect.x - 1, rect.y, rect.width, rect.height), text, style);
        EditorGUI.LabelField(new Rect(rect.x + 1, rect.y, rect.width, rect.height), text, style);
        EditorGUI.LabelField(new Rect(rect.x, rect.y - 1, rect.width, rect.height), text, style);
        EditorGUI.LabelField(new Rect(rect.x, rect.y + 1, rect.width, rect.height), text, style);

        style.normal.textColor = textColor;
        EditorGUI.LabelField(rect, text, style);

        style.normal.textColor = originalColor;
    }

    static void DrawRoundedGradientRect(Rect rect, Color startColor, Color endColor)
    {
        Handles.BeginGUI();
        GUI.BeginGroup(rect);

        for (float i = 0; i < rect.height; i++)
        {
            float t = i / rect.height;
            Color color = Color.Lerp(startColor, endColor, t * 0.5f);
            EditorGUI.DrawRect(new Rect(0, i, rect.width, 1), color);
        }

        float radius = 10f;
        Handles.color = startColor;
        Handles.DrawSolidArc(new Vector3(rect.xMin, rect.yMin), Vector3.forward, Vector3.right, 90, radius);
        Handles.DrawSolidArc(new Vector3(rect.xMax, rect.yMin), Vector3.forward, Vector3.down, 90, radius);
        Handles.DrawSolidArc(new Vector3(rect.xMin, rect.yMax), Vector3.forward, Vector3.left, 90, radius);
        Handles.DrawSolidArc(new Vector3(rect.xMax, rect.yMax), Vector3.forward, Vector3.up, 90, radius);

        GUI.EndGroup();
        Handles.EndGUI();
    }
}
