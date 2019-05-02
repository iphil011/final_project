using UnityEngine;
using System;
using System.Linq;

namespace AlanZucconi.Data
{
    [Serializable]
    public class GridData
    {
        public float[,] Data = null;

        public string[] LabelsR = null;
        public string[] LabelsC = null;
        

        public GridData (int r, int c)
        {
            Data = new float[r, c];
        }

        public float this[int r, int c]
        {
            get { return Data[r,c]; }
            set
            {
                Data[r,c] = value;
                Dirty = true;
            }
        }

        public int Rows
        {
            get
            {
                return Data.GetLength(0);
            }
        }
        public int Columns
        {
            get
            {
                return Data.GetLength(1);
            }
        }

        // Statistics
        [HideInInspector] public bool Dirty = true; // Statistics needs to be recalculated
        [HideInInspector] public float Min;
        [HideInInspector] public float Max;
        //[HideInInspector] public float Percentile;

        // TODO: could be optimised by sorting it once
        public void CalculateStatistics()
        {
            if (!Dirty)
                return;
            if (Data == null)
                return;
            if (Data.Length == 0)
                return;

            Max = Data.Cast<float>().Max();
            Min = Data.Cast<float>().Min();

            Dirty = false;
        }
    }
}