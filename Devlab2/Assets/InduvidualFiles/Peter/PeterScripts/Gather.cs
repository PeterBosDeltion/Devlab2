using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gather : MonoBehaviour {

    public Equippable axe;

    public Animation anim;
	// Use this for initialization
	void Start () {
        Inventory.itemInHand = axe;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            anim.Play();
        }
	}

    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.name);

        if (anim.isPlaying)
        {
            if (col.transform.tag == "Resource")
            {
                col.transform.GetComponent<Resource>().Harvest();
            }

            Animal a = col.transform.GetComponent<Animal>();
            if (a != null)
            {
                Destroy(a.gameObject);
            }
        }

       

    }
}
