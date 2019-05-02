using UnityEditor;
using UnityEngine;

// https://forum.unity.com/threads/free-hierarchy-window-icons-on-objects-via-interfaces.436548/
[InitializeOnLoad]
public class HierarchyIcons
{
    static HierarchyIcons() { EditorApplication.hierarchyWindowItemOnGUI += EvaluateIcons; }

    private static void EvaluateIcons(int instanceId, Rect selectionRect)
    {
        GameObject go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
        if (go == null) return;

        IHierarchyIcon slotCon = go.GetComponent<IHierarchyIcon>();
        if (slotCon != null) DrawIcon(slotCon.EditorIconPath, selectionRect);
    }
    /*
    // Normal
    private static void DrawIcon(string texName, Rect rect)
    {
        Rect r = new Rect(rect.x + rect.width - 16f, rect.y, 16f, 16f);
        GUI.DrawTexture(r, GetTex(texName));
    }
    */

    // Hihlighted
    private static void DrawIcon(string texName, Rect rect)
    {
        Rect r = new Rect(rect.x + rect.width - 16f, rect.y, 16f, 16f);
        GUI.DrawTexture(r, GetTex(texName));

        Texture2D t = new Texture2D(1, 1);
        Color c = new Color(100, 200, 100, 0.05f);
        t.SetPixel(1, 1, c);
        t.Apply();
        GUI.DrawTexture(rect, t, ScaleMode.StretchToFill);
    }

    private static Texture2D GetTex(string name)
    {
        return (Texture2D)Resources.Load("Icons/" + name);
    }
}