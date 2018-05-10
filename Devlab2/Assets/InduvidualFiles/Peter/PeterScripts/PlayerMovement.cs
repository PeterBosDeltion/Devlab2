using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    public float moveSpeed;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();
        //LookAtMouse();
	}

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.Translate(Vector3.left *- x * moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * y * moveSpeed * Time.deltaTime);
    }

    public void LookAtMouse()
    {

        //Can't remember how this fucking mouse bullshit works right now, need to fix it later

        //Vector3 pos = Input.mousePosition;
        //pos = Camera.main.ScreenToWorldPoint(pos);

        //Vector3 dir = transform.position - pos;

        //float f = Vector3.Angle(transform.position, dir);

        //transform.Rotate(Vector3.up, f);
    }

}
