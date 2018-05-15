using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    public Animator anim;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            Use();
        }
    }

    public override void Use()
    {
        anim.SetBool("Using", true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (anim.GetBool("Using"))
        {
            Entity en = other.GetComponent<Entity>();
            if (en != null && en.transform.tag != "Player")
            {
                en.TakeDamage(damage);
            }
            anim.SetBool("Using", false);

        }
    }
}
