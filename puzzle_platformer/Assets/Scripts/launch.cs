using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour {
    public GameObject player;
    public bool on;
    public float strength;
    Animator anim;
    int onHash = Animator.StringToHash("turnOn");
    int offHash = Animator.StringToHash("turnOff");
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (on && collision.gameObject == player) {
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * strength, ForceMode2D.Impulse );
        }
        
        
    }
    // Update is called once per frame
    void Update () {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (on)
        {
            anim.SetTrigger(onHash);
        }
        else
        {
            anim.SetTrigger(offHash);
        }
    }
}
