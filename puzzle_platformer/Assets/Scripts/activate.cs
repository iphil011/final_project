using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activate : MonoBehaviour {
    public GameObject trigger;
    public GameObject target;
    public int type;
    public GameObject dual;
    public bool on;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject == trigger)
        {
            on = true;
            if (type == 0)
            {
                target.GetComponent<Collider2D>().enabled = false;
                target.GetComponent<Renderer>().enabled = false;
            }
            if (type == 1 && dual.GetComponent<activate>().on) {
                target.GetComponent<Collider2D>().enabled = false;
                target.GetComponent<Renderer>().enabled = false;
            }
            if (type == 2) {
                target.GetComponent<Collider2D>().enabled = true;
                target.GetComponent<Renderer>().enabled = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject == trigger)
        {
            on = false;
            /*
            if (type == 0)
            {
                target.GetComponent<Collider2D>().enabled = true;
                target.GetComponent<Renderer>().enabled = true;
            }
            */
        }
    }
}
