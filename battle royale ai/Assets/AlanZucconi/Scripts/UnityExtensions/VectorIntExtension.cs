using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi
{
    public static class VectorIntExtension
    {
        public static Vector3Int V3I (this Vector2Int v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }
    }
}