using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed;
    public GameObject pulse;
	// Use this for initialization
	void Start () {
		
	}
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * speed;
        //Vector2 xMove = new Vector2(x, 0);
        transform.Translate(x, 0, 0);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonDown("Fire1")){
            Instantiate(pulse, new Vector3(mousePos.x,mousePos.y, 0), transform.rotation);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
