using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {
    public GameObject arrowPrefab;
    public float shootforce;
    public float destroyTimer = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if(Player.arrows > 0)
            {
                Use();
            }
        }
    }

    public override void Use()
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.forward, transform.rotation);

        //Consume arrows, might need to move the arrow int somewhere else
        Player.arrows--;

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootforce * Time.deltaTime);

        Destroy(arrow, destroyTimer);

    }
}
