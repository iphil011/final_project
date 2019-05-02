﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receptor : MonoBehaviour {
    public GameObject trigger;
    public GameObject target;
    public int type;
    public GameObject dual;
    public bool on;
    Animator anim;
    int onHash = Animator.StringToHash("turnOn");
    int offHash = Animator.StringToHash("turnOff");
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (type == 1) {
            target.GetComponent<Collider2D>().enabled = false;
            target.GetComponent<Renderer>().enabled = false;
            anim.SetTrigger(onHash);
        }
        else if (target != null) {
            target.GetComponent<launch>().on = true;
            anim.SetTrigger(onHash);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (type == 1) {
            target.GetComponent<Collider2D>().enabled = true;
            target.GetComponent<Renderer>().enabled = true;
            anim.SetTrigger(offHash);
        }
        else {
            target.GetComponent<launch>().on = false;
            anim.SetTrigger(offHash);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
