using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi
{
    public abstract class DebugDraw
    {
        // Draw an arrow
        // 2D only
        public static void Arrow (Vector2 a, Vector2 b, Color c, float t = 0f, float arrowHead = 0.25f)
        {
            Debug.DrawLine(a, b, c, t);

            Vector2 direction = (b - a).normalized;

            Debug.DrawLine(b, b + RotateUnitVector(direction, +(90 + 45)) * arrowHead, c, t);
            Debug.DrawLine(b, b + RotateUnitVector(direction, -(90 + 45)) * arrowHead, c, t);
        }

        public static Vector2 RotateUnitVector (Vector2 v, float degrees)
        {
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);

            return new Vector2
            (
                v.x * cos - v.y * sin,
                v.x * sin + v.y * cos
            );
        }

        // DashLength: ====----
        public static void DashedLine (Vector3 a, Vector3 b, Color c, float dashLength = 0.5f, float t = 0f)
        {
            float maxDistance = Vector3.Distance(a, b);

            float halfDashLength = dashLength / 2f;

            Vector3 direction = (b - a).normalized;
            Vector3 halfDash = direction * halfDashLength;

            Vector3 previousPoint = a;

            int dashes = Mathf.FloorToInt(maxDistance / dashLength);
            for (int i = 0; i < dashes; i ++)
            {
                // Dash
                Vector3 nextPoint = previousPoint + halfDash;
                Debug.DrawLine(previousPoint, nextPoint, c, t);

                // Empty space
                previousPoint = nextPoint + halfDash;
            }

            // Two cases now:
            /*  [1] B is i the empty zone
             *      (we draw a full dash)
             *  P   B
             *  ===---
             *  
             *  [2] B is i the draw zone
             *      (we draw a smaller line)
             *  PB
             *  ===---
             *  ==x
             */
            float currentDistance = Vector3.Distance(a, previousPoint);
            float distanceLeft = maxDistance - currentDistance;
            if (distanceLeft < halfDashLength)
                Debug.DrawLine(previousPoint, b, c, t); // Smaller dash
            else
                Debug.DrawLine(previousPoint, previousPoint + halfDash, c, t); // Full dash
        }




        //public static void Arrow(Vector2 a, Vector2 b, Color c, float t = 0f, float arrowHead = 0.25f)
        public static void FadeLine (Vector3 a, Vector3 b, Color c, float t = 1f)
        {
            float segment = 0.1f;

            float distance = Vector3.Distance(a, b);
            int segments = Mathf.FloorToInt(distance / segment);

            segment = distance / segments; // Updated

            Vector3 direction = (b - a).normalized;

            Vector3 previousPosition = a;

            for (int i = 0; i < segments; i ++)
            {
                // i:        [0, segments]
                // duration: [0, t]
                float duration = (i / (float)segments) * t;

                Vector3 nextPosition = previousPosition + direction * segment;
                Debug.DrawLine(previousPosition, nextPosition, c, duration);

                previousPosition = nextPosition;
            }
        }


        // Rectangle
        public static void Rectangle (Vector3 centre, float w, float h, Color c, float d = 0f)
        {
            Debug.DrawLine
            (
                centre + new Vector3(-w / 2f, +h / 2f),
                centre + new Vector3(+w / 2f, +h / 2f),
                c, d
            );
            Debug.DrawLine
            (
                centre + new Vector3(+w / 2f, +h / 2f),
                centre + new Vector3(+w / 2f, -h / 2f),
                c, d
            );
            Debug.DrawLine
            (
                centre + new Vector3(+w / 2f, -h / 2f),
                centre + new Vector3(-w / 2f, -h / 2f),
                c, d
            );
            Debug.DrawLine
            (
                centre + new Vector3(-w / 2f, -h / 2f),
                centre + new Vector3(-w / 2f, +h / 2f),
                c, d
            );
        }


        // Circle 2D
        // (but can use Z)
        public static void Circle(Vector3 centre, float r, Color c, float d = 0f)
        {
            // TODO: change to get constant angle
            //float segment = r < 2 ? 0.5f : 2f;

            float circumference = 2 * Mathf.PI * r;

            float segmentLength = circumference / 25f;

            int segments = Mathf.CeilToInt(circumference / segmentLength);

            Polygon(centre, segments, r, c, 0f, d);
        }
        // Polygon 2D
        // (but can use Z)
        public static void Polygon(Vector3 centre, int sides, float r, Color c, float initialAngle = 0f, float d = 0f)
        {
            Vector3 previousPoint = centre + new Vector3(r, 0);
            for (int i = 0; i <= sides; i ++)
            {
                float angle = initialAngle + (2 * Mathf.PI) * (i / (float) sides);
                Vector3 nextPoint = centre + new Vector3
                (
                    r * Mathf.Cos(angle),
                    r * Mathf.Sin(angle)
                );

                Debug.DrawLine(previousPoint, nextPoint, c, d);

                previousPoint = nextPoint;
            }
        }
    }
}