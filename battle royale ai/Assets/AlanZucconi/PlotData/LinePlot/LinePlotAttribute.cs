using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi.Data
{
    public class LinePlotAttribute : PropertyAttribute
    {
        public float Height = 250;
        //public Vector2 Grid = new Vector2(100, 10);
        public float GridX = 10f;
        public float GridY = 10f;
        public bool KeepAspectRatio = false;

        public Color DataColour = new Color(1f, 1f, 1f);
        //public Color MedianColour = new Color(1f, 1f, 0f);
        public Color BackgroundColor = new Color(0.254902f, 0.254902f, 0.254902f);
        public Color GridColor = new Color(1, 1, 1);

        public string LabelX = null;
        public string LabelY = null;
    }
}