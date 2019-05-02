using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace AlanZucconi.Data
{
    [Serializable]
    public class PlotData
    {
        public List<Vector2> Data = new List<Vector2>();

        public void Add(Vector2 point)
        {
            Data.Add(point);
            Dirty = true;
        }


        public Vector2 this[int i]
        {
            get
            {
                return Data[i];
            }
        }



        // Statistics
        [HideInInspector] public bool Dirty = true; // Statistics needs to be recalculated
        [HideInInspector] public Vector2 Max;
        [HideInInspector] public Vector2 Quartile1; // 25%
        [HideInInspector] public Vector2 Quartile2; // Median
        [HideInInspector] public Vector2 Quartile3; // 75%

        // TODO: could be optimised by sorting it once
        public void CalculateStatistics()
        {
            if (!Dirty)
                return;
            if (Data.Count == 0)
                return;

            Max = new Vector2
            (
                Data.Max(point => point.x),
                Data.Max(point => point.y)
            );

            Quartile1 = new Vector2
            (
                Data.Percentile(point => point.x, 1f / 4f),
                Data.Percentile(point => point.y, 1f / 4f)
            );
            Quartile2 = new Vector2
            (
                Data.Percentile(point => point.x, 2f / 4f),
                Data.Percentile(point => point.y, 2f / 4f)
            );
            Quartile3 = new Vector2
            (
                Data.Percentile(point => point.x, 3f / 4f),
                Data.Percentile(point => point.y, 3f / 4f)
            );

            Dirty = false;
        }
    }
}