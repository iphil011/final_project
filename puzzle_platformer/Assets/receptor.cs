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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == trigger) {
            target.GetComponent<activate>().on = true;
            Debug.Log("in");
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
