using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace AlanZucconi.Data
{
    // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
    [CustomPropertyDrawer(typeof(ScatterPlotAttribute))]
    public class ScatterPlotDrawer : PropertyDrawer
    {
        ScatterPlot scatterPlot = null;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
            EditorGUI.PropertyField(position, property);

            if (!property.isExpanded)
                return;

            position.y += PropertyHeight;

            ScatterPlotAttribute plotAttribute = attribute as ScatterPlotAttribute;
            PlotData data = fieldInfo.GetValue(property.serializedObject.targetObject) as PlotData;

            if (scatterPlot == null)
                scatterPlot = new ScatterPlot(data, plotAttribute);

            
            scatterPlot.OnGUI(position);
            //scatterPlot.OnInspectorGUI(position);
        }

        
        const float PropertyHeight = 16;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //ScatterPlotAttribute scatterPlot = attribute as ScatterPlotAttribute;
            //return property.isExpanded ? scatterPlot.Height : PropertyHeight;
            //return property.isExpanded ? 16 : PropertyHeight;
            ScatterPlotAttribute plotAttribute = attribute as ScatterPlotAttribute;
            return PropertyHeight + plotAttribute.Height;
        }
        
    }
}