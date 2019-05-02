using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace AlanZucconi.Data
{
    // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
    [CustomPropertyDrawer(typeof(GridPlotAttribute))]
    public class GridPlotDrawer : PropertyDrawer
    {
        //GridPlot scatterPlot = null;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
            EditorGUI.PropertyField(position, property);

            if (!property.isExpanded)
                return;

            position.y += PropertyHeight;
            position.height -= PropertyHeight * 1f;

            GridPlotAttribute plotAttribute = attribute as GridPlotAttribute;
            GridData data = fieldInfo.GetValue(property.serializedObject.targetObject) as GridData;
            data.CalculateStatistics();

            //if (scatterPlot == null)
            //    scatterPlot = new ScatterPlot(data, plotAttribute);


            //scatterPlot.OnGUI(position);
            //scatterPlot.OnInspectorGUI(position);

            //SerializedProperty data = property.FindPropertyRelative("Data");
            //SerializedProperty row = data.GetArrayElementAtIndex(0).F

            if (data.Data == null)
                return;

            

            int rs = data.Data.GetLength(0);
            int cs = data.Data.GetLength(1);

            Vector2 cellSize = new Vector2
            (
                position.width / cs,
                position.height / rs
            );

            const float border = 1f;

            // https://forum.unity.com/threads/fixed-editorguilayout-label-centered-how-to-do.377152/
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            //var labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter };

            var labelStyleUpper = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter };
            var labelStyleLower = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter };
            //EditorGUILayout.LabelField("Blabla", style, GUILayout.ExpandWidth(true));

            for (int r = 0; r < rs; r++)
            {
                for (int c = 0; c < cs; c++)
                {
                    // Skips trace
                    if (r == c && !plotAttribute.Trace)
                        continue;

                    Rect cellPosition = new Rect
                    (
                        position.x + (c / (cs-0f)) * position.width,
                        position.y + (r / (rs-0f)) * position.height,
                        
                        cellSize.x,
                        cellSize.y
                    );

                    // Rectangle
                    EditorGUI.DrawRect
                    (
                        new Rect(cellPosition).Extrude(-border),
                        ColorExtension.Lerp3
                        (
                            Color.red.xA(0.5f),
                            Color.yellow.xA(0.5f),
                            Color.green.xA(0.5f),
                            (data[r, c] - data.Min) / (data.Max - data.Min)
                        )

                        //ColorExtension.RYG((data[r, c] - data.Min) / (data.Max - data.Min))
                        //plotAttribute.MaxColor                        
                        /*
                        Color.Lerp
                        (
                            plotAttribute.MinColor,
                            plotAttribute.MaxColor,
                            // [m, M] -> [0, 1]
                            (data[r, c] - data.Min) / (data.Max - data.Min)
                        )
                        */
                    );
                    // Text
                    EditorGUI.LabelField(cellPosition, data[r,c].ToString(), style);


                    // Label
                    if (data.LabelsR != null && data.LabelsC != null)
                    {
                        EditorGUI.LabelField(cellPosition, data.LabelsR[r].ToString(), labelStyleUpper);
                        EditorGUI.LabelField(cellPosition, data.LabelsC[c].ToString(), labelStyleLower);
                    }
                    /*
                    // Label
                    if (data.Labels != null)
                    {
                        if (r == 0)
                            EditorGUI.LabelField(cellPosition, data.Labels[c].ToString(), labelStyle);
                        else
                        if (c == 0)
                            EditorGUI.LabelField(cellPosition, data.Labels[r].ToString(), labelStyle);
                    }
                    */

                }
            }
        }

        
        const float PropertyHeight = 16;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //ScatterPlotAttribute scatterPlot = attribute as ScatterPlotAttribute;
            //return property.isExpanded ? scatterPlot.Height : PropertyHeight;
            //return property.isExpanded ? 16 : PropertyHeight;
            if (!property.isExpanded)
                return PropertyHeight;

            GridData data = fieldInfo.GetValue(property.serializedObject.targetObject) as GridData;
            // Data is empty
            if (data == null || data.Data == null)
                return PropertyHeight;

            GridPlotAttribute plotAttribute = attribute as GridPlotAttribute;
            return Mathf.Max(PropertyHeight + plotAttribute.Height,
                PropertyHeight + PropertyHeight*3f*data.Data.GetLength(1))
                ;
            //return PropertyHeight + plotAttribute.Height;
        }
        
    }
}