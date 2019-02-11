using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed;
    public float maxSpeed;
    public GameObject pulse;
    public int limit;
    private int shots;
	// Use this for initialization
	void Start () {
        limit = 3;
        shots = 0;
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
            Instantiate(pulse, new Vector3(mousePos.x, mousePos.y, 0), transform.rotation);
            shots++;
        }
        if (Input.GetButtonDown("Fire2")) {
            shots = 0;
        }
    }
}
