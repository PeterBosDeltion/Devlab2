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
            en.TakeDamage(equippable.damage);


            if(en.GetComponent<Animal>() != null)
            {
                if (en.GetComponent<Animal>().type == Animal.Types.Aggresive)
                {
                    en.GetComponent<Animal>().state = Animal.States.Attack;
                }
                else if (en.GetComponent<Animal>().type == Animal.Types.Passive)
                {
                    en.GetComponent<Animal>().state = Animal.States.Run;
                }
            }
        }
           

        if(other.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
