using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi.Bots
{
    public class CreateArena : MonoBehaviour
    {

        public GameObject WallPrefab;

        [Range(10,100)]
        public float Radius = 10;

        public float Segments = 10;

        [Button(Editor = true)]
        void Delete()
        {
            Collider2D[] walls = transform.GetComponentsInChildren<Collider2D>(true);
            foreach (Collider2D wall in walls)
                DestroyImmediate(wall.gameObject);
        }
        [Button(Editor=true)]
        void Create()
        {
            Delete();
            float circumference = 2 * Mathf.PI * Radius;

            for (int i = 0; i < Segments; i ++)
            {
                float angle = (2f * Mathf.PI) * (i / (float) Segments);
                Vector3 position = transform.position + Radius * new Vector3
                (
                    Mathf.Cos(angle),
                    Mathf.Sin(angle)
                );
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
                GameObject wall = Instantiate(WallPrefab, position, rotation, transform);
                wall.transform.localScale = new Vector3(1f, circumference / Segments, 1f);
            }
        }
    }
}