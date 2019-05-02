using UnityEngine;
using UnityEditor;
// https://gist.github.com/frarees/9791517
[CustomPropertyDrawer (typeof (MinMaxSliderAttribute))]
class MinMaxSliderDrawer : PropertyDrawer {

    /*
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

		if (property.propertyType == SerializedPropertyType.Vector2) {
			Vector2 range = property.vector2Value;
			float min = range.x;
			float max = range.y;
			MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;
			EditorGUI.BeginChangeCheck ();
			EditorGUI.MinMaxSlider (position, label, ref min, ref max, attr.min, attr.max);
			if (EditorGUI.EndChangeCheck ()) {
				range.x = min;
				range.y = max;
				property.vector2Value = range;
			}
		} else {
			EditorGUI.LabelField (position, label, "Use only with Vector2");
		}
	}*/

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //if (property.serializedObject.isEditingMultipleObjects) return 0f;
        return base.GetPropertyHeight(property, label) + 16f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            Vector2 range = property.vector2Value;

            float min = range.x;
            float max = range.y;
            position.height -= 16f;

            MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            //var min = minProperty;
            //var max = maxProperty;

            var left = new Rect(position.x, position.y, position.width / 2 - 11f, position.height);
            var right = new Rect(position.x + position.width - left.width, position.y, left.width, position.height);
            var mid = new Rect(left.xMax, position.y, 22, position.height);
            min = Mathf.Clamp(EditorGUI.FloatField(left, min), attr.min, max);
            EditorGUI.LabelField(mid, " to ");
            max = Mathf.Clamp(EditorGUI.FloatField(right, max), min, attr.max);

            position.y += 16f;
            EditorGUI.MinMaxSlider(position, GUIContent.none, ref min, ref max, attr.min, attr.max);

            //minProperty = min;
            //maxProperty = max;

            range.x = min;
            range.y = max;
            property.vector2Value = range;

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label, "Use only with Vector2");
        }
    }
}