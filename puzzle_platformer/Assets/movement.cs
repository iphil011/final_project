using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float speed;
    public GameObject pulse;
    public int limit;
    private int shots;
	// Use this for initialization
	void Start () {
        limit = 3;
        shots = 0;
	}
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * speed;
        //Vector2 xMove = new Vector2(x, 0);
        transform.Translate(x, 0, 0);
        
        
    }
    // Update is called once per frame
    void Update () {
        
        if (Input.GetButtonDown("Fire1")&&shots<limit)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(pulse, new Vector3(mousePos.x, mousePos.y, 0), transform.rotation);
            shots++;
        }
        if (Input.GetButtonDown("Fire2")) {
            shots = 0;
        }
    }
}
