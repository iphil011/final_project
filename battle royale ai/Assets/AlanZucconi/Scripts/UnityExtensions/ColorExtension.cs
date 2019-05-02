using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi
{
    public static class ColorExtension
    {
        public static Color xA(this Color color, float a)
        {
            color.a *= a;
            return color;
        }

        // p [0,1]
        public static Color Darker (this Color color, float p)
        {
            return new Color
            (
                color.r * p,
                color.g * p,
                color.b * p,
                color.a
            );
        }

        // https://stackoverflow.com/questions/6394304/algorithm-how-do-i-fade-from-red-to-green-via-yellow-using-rgb-values
        // 0   -> R
        // 0.5 -> Y
        // 1   -> G
        public static Color RYG (float t)
        {
            float x = 1 - t;
            return new Color(2.0f * x, 2.0f * (1 - x), 0);
        }

        // Lerp between three colours
        public static Color Lerp3 (Color c0, Color c1, Color c2, float t)
        {
            if (t <= 0.5f)
                // t: [0, 0.5]
                //    [0, 1  ]
                return Color.Lerp(c0, c1, t * 2);
            else
                // t: [0.5, 1]
                //    [0,   1]
                return Color.Lerp(c1, c2, (t - 0.5f) * 2f);

        }
    }
}