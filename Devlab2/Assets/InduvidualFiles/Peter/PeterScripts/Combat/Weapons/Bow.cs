using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {
    public GameObject arrowPrefab;
    public float shootforce;
    public float destroyTimer = 5;
    private Item arrow;
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

        //Consume arrows, might need to move the arrow int somewhere else


        //Find slot with arrow
        int arrs = 0;

        foreach (Slot w in Inventory.Instance.toolBar)
        {
            if(w.myItem != null)
            {
                if(w.myItem.itemName == "Arrow")
                {
                    arrs++;
                }
            }
        }


        if(arrs > 0)
        {
            foreach (Slot q in Inventory.Instance.toolBar)
            {
                if (q.myItem != null)
                {
                    Debug.Log("Not null");
                    if (q.myItem.itemName == "Arrow")
                    {
                        q.RemoveItem();
                        GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.forward, transform.rotation);

                        Rigidbody rb = arrow.GetComponent<Rigidbody>();
                        rb.AddForce(transform.forward * shootforce * Time.deltaTime);

                        Destroy(arrow, destroyTimer);

                        break;
                    }
                }
                else
                {
                    Debug.Log("Null");

                }
            }

        }
        else
        {
            foreach (Slot s in Inventory.Instance.theInventory)
            {
                if (s.myItem != null)
                {
                    if (s.myItem.itemName == "Arrow")
                    {
                        s.RemoveItem();
                        GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.forward, transform.rotation);

                        Rigidbody rb = arrow.GetComponent<Rigidbody>();
                        rb.AddForce(transform.forward * shootforce * Time.deltaTime);

                        Destroy(arrow, destroyTimer);

                        break;
                    }
                }
                else
                {
                    Debug.Log("Null");

                }

            }
        }

    }
}
