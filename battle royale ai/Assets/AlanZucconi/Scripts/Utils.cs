using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Utils
{
    public static float Sin (float x, float min, float max, float period)
    {
        return (max - min) / 2f * (1f + Mathf.Sin(x * Mathf.PI * 2 / period)) + min;
    }
    public static float ManhattanDistance (Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }



    // http://math.stackexchange.com/questions/76457/check-if-a-point-is-within-an-ellipse
    // centre = centre of the ellipse
    // radius = (x,y) = (semi major axis x, semi major axis y)
    public static bool PointInsideEllipse (Vector2 point, Vector2 centre, Vector2 radius)
    {
       return NormalisedDistanceFromEllipseCentre(point, centre, radius) <= 1f;
    }

    // 0 when point is at the centre of ellipse
    // 1 when point is on the margin of the ellipse
    // <= 1 = point inside the ellipse
    // >  1 = point outside the ellipse
    public static float NormalisedDistanceFromEllipseCentre(Vector2 point, Vector2 centre, Vector2 radius)
    {
        return
           Mathf.Pow((point.x - centre.x) / radius.x, 2f) +
           Mathf.Pow((point.y - centre.y) / radius.y, 2f) ;
    }


    // in radians
    public static float AngleBetweenPoints (Vector2 from, Vector2 to)
    {
        Vector2 diff = to - from;
        return Mathf.Atan2(diff.y, diff.x);
    }

    public static float Snap (float newMultiplier, float snapValue)
    {
        return ((int)(newMultiplier / snapValue)) * snapValue;
    }


    // https://forum.unity.com/threads/closest-point-on-a-line.121567/
    static public Vector3 ClosestPointToRay (Vector3 origin, Vector3 direction, Vector3 point)
    {
        Vector3 point2origin = origin - point;
        //Vector3 point2closestPointOnLine = point2origin - Vector3.Dot(point2origin, direction) * direction;
        Vector3 point2closestPointOnLine = origin - Vector3.Dot(point2origin, direction) * direction;
        return point2closestPointOnLine;
        //return point2closestPointOnLine.magnitude;
    }
    // Given a ray and a point,
    // it finds the closest point to the point that lies on tha ray.
    // It returns the distance from the ray origin to that point.
    // It can be negative, if the point is aligned opposite to the ray direction
    static public float ProjectClosestPointToRay (Vector3 origin, Vector3 direction, Vector3 point)
    {
        Vector3 point2origin = origin - point;
        return -Vector3.Dot(point2origin, direction);
    }
}
