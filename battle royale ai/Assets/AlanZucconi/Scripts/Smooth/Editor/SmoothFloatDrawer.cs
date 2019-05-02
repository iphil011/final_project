using UnityEngine;
using UnityEditor;

/*
[CustomPropertyDrawer(typeof(SmoothFloat))]
class SmoothFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Exposes the slider for the smooth damp time
        EditorGUI.PropertyField
        (
            //new Rect(rect.x + x, rect.y, 50, EditorGUIUtility.singleLineHeight),
            position,
            property.FindPropertyRelative("smoothTime"),
            new GUIContent(property.displayName)
        );
    }
}
*/