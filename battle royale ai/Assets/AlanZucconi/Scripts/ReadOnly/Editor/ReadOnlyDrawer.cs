using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {

        //EditorGUI.BeginProperty(position, label, property);


        ReadOnlyAttribute readOnly = attribute as ReadOnlyAttribute;
        GUI.enabled = (!Application.isPlaying && readOnly.Editor);
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;


        //GUI.enabled = false;
        //EditorGUI.PropertyField(position, property, label, true);
        //GUI.enabled = true;

        //EditorGUI.EndProperty();
    }
}