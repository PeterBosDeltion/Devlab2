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


        foreach (Slot s in Inventory.Instance.theInventory)
        {
            if(s.myItem != null)
            {
                    Debug.Log("Not null");
                if (s.myItem.itemName == "Arrow")
                {
                    Debug.Log("Arrow found");
                    s.RemoveItem();
                    GameObject arrow = Instantiate(arrowPrefab, transform.position + transform.forward, transform.rotation);

                    Debug.Log("Arrow Removed");
                    Rigidbody rb = arrow.GetComponent<Rigidbody>();
                    Debug.Log("rb found");
                    rb.AddForce(transform.forward * shootforce * Time.deltaTime);
                    Debug.Log("rb force");

                    Destroy(arrow, destroyTimer);
                    Debug.Log("Destroyed");

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
