using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi.Data
{
    public class GridPlotAttribute : PropertyAttribute
    {
        
        public float Height = 250;
        public bool KeepAspectRatio = false;

        public Color MinColor = Color.red.xA(0.5f);
        public Color MaxColor = Color.green.xA(0.5f);

        public bool Trace = true;
        /*
        public Vector2 Grid = new Vector2(100, 10);
        

        public Color DataColour = new Color(1f, 1f, 1f);
        public Color MedianColour = new Color(1f, 1f, 0f);
        public Color BackgroundColor = new Color(0.254902f, 0.254902f, 0.254902f);
        public Color GridColor = new Color(1, 1, 1);

        public string LabelX = null;
        public string LabelY = null;
        */
    }
}