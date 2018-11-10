using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receptor : MonoBehaviour {
    public GameObject trigger;
    public GameObject door;
    public int type;
    public GameObject dual;
    public bool on;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == trigger)
        {
            on = true;
        }
    }
}
