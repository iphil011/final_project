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
        //Debug.Log("in");
        if (target != null)
        {
            target.GetComponent<launch>().on = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target.GetComponent<launch>().on = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
