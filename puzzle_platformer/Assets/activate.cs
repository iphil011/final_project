using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activate : MonoBehaviour {
    public GameObject trigger;
    public GameObject door;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == trigger) {
            door.GetComponent<Collider2D>().enabled = false;
            door.GetComponent<Renderer>().enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == trigger)
        {
            door.GetComponent<Collider2D>().enabled = true;
            door.GetComponent<Renderer>().enabled = true;
        }
    }
}
