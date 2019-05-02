using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi
{
    public static class Vector3Extension
    {
        // Changes a single value
        public static Vector3 X(this Vector3 v, float x)
        {
            v.x = x;
            return v;
        }
        public static Vector3 Y(this Vector3 v, float y)
        {
            v.y = y;
            return v;
        }
        public static Vector3 Z (this Vector3 v, float z)
        {
            v.z = z;
            return v;
        }


        // Get coordinate from another vector
        public static Vector3 X(this Vector3 v1, Vector3 v2)
        {
            v1.x = v2.x;
            return v1;
        }
        public static Vector3 Y(this Vector3 v1, Vector3 v2)
        {
            v1.y = v2.y;
            return v1;
        }
        public static Vector3 Z(this Vector3 v1, Vector3 v2)
        {
            v1.z = v2.z;
            return v1;
        }


        // Get coordinate from another vector
        // Vector2 version
        public static Vector3 X(this Vector2 v1, Vector3 v2)
        {
            Vector3 v = v1;
            v.x = v2.x;
            return v;
        }
        public static Vector3 Y(this Vector2 v1, Vector3 v2)
        {
            Vector3 v = v1;
            v.y = v2.y;
            return v;
        }
        public static Vector3 Z(this Vector2 v1, Vector3 v2)
        {
            Vector3 v = v1;
            v.z = v2.z;
            return v;
        }


        // Replaces two values at once
        public static Vector3 XY(this Vector3 v1, Vector2 v2)
        {
            v1.x = v2.x;
            v1.y = v2.y;
            return v1;
        }
        public static Vector3 XY(this Vector3 v, float x, float y)
        {
            v.x = x;
            v.y = y;
            return v;
        }



        // Gets two values at once
        public static Vector3 XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
        public static Vector3 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
        public static Vector3 YZ(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }




        // Limits to Area
        public static Vector2 Clamp (this Vector2 v, Rect rect)
        {
            return new Vector2
                (
                    Mathf.Clamp(v.x, rect.xMin, rect.xMax),
                    Mathf.Clamp(v.y, rect.yMin, rect.yMax)
                );
        }
        public static Vector3 Clamp(this Vector3 v, Rect rect)
        {
            return new Vector3
                (
                    Mathf.Clamp(v.x, rect.xMin, rect.xMax),
                    Mathf.Clamp(v.y, rect.yMin, rect.yMax),
                    v.z
                );
        }





        // Element-wise
        public static Vector3 Div (this Vector3 v1, Vector3 v2)
        {
            return new Vector3
                (   v1.x / v2.x,
                    v1.y / v2.y,
                    v1.z / v2.z
                );
        }
        public static Vector3 Mul(this Vector3 v1, Vector3 v2)
        {
            return new Vector3
                (v1.x * v2.x,
                    v1.y * v2.y,
                    v1.z * v2.z
                );
        }





        // Rect
        public static Rect Translate(this Rect rect, Vector2 p)
        {
            rect.position += p;
            return rect;
        }
        public static Rect Extrude (this Rect rect, float margin)
        {
            //rect.width += margin;
            //rect.height += margin;
            rect.xMax += margin;
            rect.xMin -= margin;
            rect.yMax += margin;
            rect.yMin -= margin;
            return rect;
        }


        // Rescale a point in between [0,1]
        // based on the bounds
        public static Vector3 Rescale (this Bounds bounds, Vector3 point)
        {
            Vector3 min = bounds.min;
            Vector3 diff = bounds.max - bounds.min;

            return new Vector3
                (
                    diff.x == 0
                    ?   min.x
                    :   (point.x - min.x) / diff.x,

                    diff.y == 0
                    ? min.y
                    : (point.y - min.y) / diff.y,

                    diff.z == 0
                    ? min.z
                    : (point.z - min.z) / diff.z
                );
        }

        // Centre a point so that it is centret as (0,0)
        //public static Vector3 Centre (this Bounds bounds, Vector3 point)
        //{
        //    return point - bounds.center;
        //}





        // Vector stuff
        public static float SquaredDistance (Vector2 a, Vector2 b)
        {
            Vector2 d = a - b;
            return d.x * d.x + d.y * d.y;
        }

        public static Vector2 ClampMagnitude (this Vector2 v, float m)
        {
            if (v.sqrMagnitude < m * m)
                return v;

            return v.normalized * m;
        }






        // Arrays
        public static Vector2[] ToVector2(this Vector3[] v3)
        {
            return System.Array.ConvertAll<Vector3, Vector2>(v3, getV3fromV2);
        }

        private static Vector2 getV3fromV2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }
    }
}