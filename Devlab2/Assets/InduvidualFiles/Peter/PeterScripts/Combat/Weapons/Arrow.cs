using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Weapon {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Use()
    {
        ///////////////////////////////////////
    }

    public void OnTriggerEnter(Collider other)
    {
        Entity en = other.GetComponent<Entity>();
        if (en != null && en.transform.tag != "Player")
        {
            en.TakeDamage(damage);
        }

        if(other.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
