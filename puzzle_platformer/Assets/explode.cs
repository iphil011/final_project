using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {
    public float radius;
    public float strength;
    public Vector2 target;
    public float speed;
	// Use this for initialization
	void Start () {
        radius = 1;
        strength = 7;
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);

        if (Input.GetButtonDown("Fire2")) {
            Vector2 explosionPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos,radius);
            foreach (Collider2D hit in colliders) {
                Vector2 vel = hit.transform.position - transform.position;
                vel.Normalize();
                vel = vel * strength;
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null) {
                    rb.AddForce(vel, ForceMode2D.Impulse);
                }
            }
            Destroy(gameObject);
        }
	}
}
