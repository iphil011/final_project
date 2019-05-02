using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using System;
using System.Linq;

namespace AlanZucconi.Data
{
    // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
    [CustomPropertyDrawer(typeof(DerivedType))]
    public class DerivedTypeDrawer : PropertyDrawer
    {
        private string[] TypeNames;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DerivedType derivedType = fieldInfo.GetValue(property.serializedObject.targetObject) as DerivedType;
            if (TypeNames == null)
                TypeNames = derivedType.Derived.Select(t => t.ToString()).ToArray();

            if (TypeNames.Length == 0)
            {
                EditorGUI.LabelField(position, label.text, "no derived types");
                return;
            }

            derivedType.Index = EditorGUI.Popup(position, label.text, derivedType.Index, TypeNames);
        }
    }
}