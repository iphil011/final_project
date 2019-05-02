using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace AlanZucconi.Data
{
    // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
    [CustomPropertyDrawer(typeof(LinePlotAttribute))]
    public class LinePlotDrawer : PropertyDrawer
    {
        LinePlot linePlot = null;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
            EditorGUI.PropertyField(position, property);

            if (!property.isExpanded)
                return;

            position.y += PropertyHeight;

            LinePlotAttribute plotAttribute = attribute as LinePlotAttribute;
            PlotData data = fieldInfo.GetValue(property.serializedObject.targetObject) as PlotData;

            if (linePlot == null)
                linePlot = new LinePlot(data, plotAttribute);

            
            linePlot.OnGUI(position);
            //scatterPlot.OnInspectorGUI(position);
        }

        
        const float PropertyHeight = 16;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //ScatterPlotAttribute scatterPlot = attribute as ScatterPlotAttribute;
            //return property.isExpanded ? scatterPlot.Height : PropertyHeight;
            //return property.isExpanded ? 16 : PropertyHeight;
            LinePlotAttribute plotAttribute = attribute as LinePlotAttribute;
            return PropertyHeight + plotAttribute.Height;
        }
        
    }
}