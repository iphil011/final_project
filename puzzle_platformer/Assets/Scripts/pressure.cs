using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pressure : MonoBehaviour
{
   
    public GameObject target;
    public int type;
    public GameObject dual;
    public bool on;
    Animator anim;
    int onHash = Animator.StringToHash("turnOn");
    int offHash = Animator.StringToHash("turnOff");
    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (type == 1)
            {
                target.GetComponent<Collider2D>().enabled = false;
                target.GetComponent<Renderer>().enabled = false;
                //anim.SetTrigger(onHash);
            }
            else if (target != null)
            {
                target.GetComponent<launch>().on = true;
                //anim.SetTrigger(onHash);
            }
        }
        //activates linked object while trigger active
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (type == 1)
            {
                target.GetComponent<Collider2D>().enabled = true;
                target.GetComponent<Renderer>().enabled = true;
                //anim.SetTrigger(offHash);
            }
            else
            {
                target.GetComponent<launch>().on = false;
                //anim.SetTrigger(offHash);
            }
        }
        //deactivates linked object when trigger is inactive
    }
}
