using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed;
    public float maxSpeed;
    public GameObject pulse;
    public int limit;
    public float sSpeed;
    private int shots;
	// Use this for initialization
	void Start () {
        limit = 3;
        shots = 0;
        sSpeed = 0.5f;
	}
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * speed;
        transform.Translate(x, 0, 0);

        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed) {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }
        
    }
    // Update is called once per frame
    void Update () {
        
        if (Input.GetButtonDown("Fire1")&&shots<limit)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(pulse, mousePos, transform.rotation);
            /*
            Instantiate(pulse, transform.position, transform.rotation);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pulse.GetComponent<explode>().target = new Vector2(mousePos.x, mousePos.y);
            shots++;
            */
        }
        if (Input.GetButtonDown("Fire2")) {
            shots = 0;
        }
    }
}
