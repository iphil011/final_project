using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {
    public float radius;
    public float strength;
    public Vector2 target;
    public Vector3 vel;
    public float dist;
	// Use this for initialization
	void Start () {
        radius = 1;
        strength = 7;
        vel = new Vector3(target.x, target.y, 0) -transform.position;
        vel = vel.normalized *0.2f;
        dist = Vector2.Distance(transform.position, target);
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(transform.position, target) > 0.2) {
            transform.position += vel;
        }

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
