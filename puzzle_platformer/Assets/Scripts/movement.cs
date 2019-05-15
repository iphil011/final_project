using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed; //speed player moves
    public float maxSpeed; // max velocity of player
    public GameObject pulse; 
    public int limit; // limit of pulses player can spawn
    public float sSpeed;
    private int shots; // amount of shots out
    
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
        transform.Translate(x, 0, 0); // moves player horizontally through transform

        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed) {
            Vector2 vel = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
            GetComponent<Rigidbody2D>().velocity = vel; // limits player velocity
        }
        
    }
    // Update is called once per frame
    void Update () {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = new Vector3(mousePos.x,mousePos.y,0) - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir,dist,layerMask);
        //raycasts from the player to the mouse location ignoring every collision inside the layermask
        Debug.DrawRay(transform.position, new Vector2(0, -1)*1);
        RaycastHit2D grounded = Physics2D.Raycast(transform.position, new Vector2(0, -1),0.6f, layerMask);
        //ray cast from the palyer straight down to check if the palyer is on the ground
        
        if (Input.GetButtonDown("Fire1")&&shots<limit)
        {
            if (hit)
            {
                Instantiate(pulse, hit.point, transform.rotation);
                shots++;
                //if ray to mouse hits a collider spawns a pulse at the hit location
            }
            else
            {
                Instantiate(pulse, mousePos, transform.rotation);
                shots++;
                // if ray doesn't collide spawns a pulse at mouse location
            }
            
        }
        if (Input.GetButtonDown("Fire2")) {
            if (grounded)
            {
                shots = 0;
                // if player is on the ground refreshes shots
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "energyZone")
        {
            limit = 100;
            //sets limit of pulses to 100
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "energyZone")
        {
            limit = 3;
            //sets limit back to 3
        }
    }
}
