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
    
    int layerMask = 1 << 8;

	// Use this for initialization
	void Start () {
        limit = 3;
        shots = 0;
        sSpeed = 0.5f;
        layerMask = ~layerMask;
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = new Vector3(mousePos.x,mousePos.y,0) - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir,dist,layerMask);
        Debug.DrawRay(transform.position, new Vector2(0, -1)*1);
        RaycastHit2D grounded = Physics2D.Raycast(transform.position, new Vector2(0, -1),0.6f, layerMask);
        
        if (Input.GetButtonDown("Fire1")&&shots<limit)
        {
            if (hit)
            {
                Instantiate(pulse, hit.point, transform.rotation);
                shots++;
            }
            else
            {
                Instantiate(pulse, mousePos, transform.rotation);
                shots++;
            }
            
        }
        if (Input.GetButtonDown("Fire2")) {
            if (grounded)
            {
                shots = 0;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "energyZone")
        {
            limit = 100;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "energyZone")
        {
            limit = 3;
        }
    }
}
