using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receptor : MonoBehaviour {
    public GameObject trigger;
    public GameObject target;
    public int type;
    public GameObject dual;
    public bool on;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (type == 1) {
            target.GetComponent<Collider2D>().enabled = false;
            target.GetComponent<Renderer>().enabled = false;
        }
        else if (target != null) {
            target.GetComponent<launch>().on = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (type == 1) {
            target.GetComponent<Collider2D>().enabled = true;
            target.GetComponent<Renderer>().enabled = true;
        }
        else {
            target.GetComponent<launch>().on = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
