using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour {
    public GameObject player;
    public bool on;
    public float strength;
	// Use this for initialization
	void Start () {
        
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (on && collision.gameObject == player) {
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * strength, ForceMode2D.Impulse );
            //Debug.Log("in");
        }
    }
    // Update is called once per frame
    void Update () {
        
	}
}
