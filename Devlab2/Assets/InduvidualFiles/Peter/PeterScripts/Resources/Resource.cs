﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public Item myResource;
    public Equippable.CanGather type;
    public int toughness = 1;
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Harvest(Gather g)
    {
        Debug.Log("Harvest");
        if(Inventory.itemInHand.myGathering == type)
        {
            Debug.Log("CorrectTool");

            if(toughness > 0)
            {
                toughness--;
            }
            else
            {
                g.anim.SetBool("using", false);
                g.beingUsed = false;
                g.waiting = false;


                int itemsInInv = 0;
                foreach (Slot s in Inventory.Instance.theInventory)
                {
                    if(s.myItem == null)
                    {
                        Inventory.Instance.AddItem(myResource); //Uncomment when there is inventory in scene pl0x
                        break;
                    }
                    else
                    {
                        itemsInInv++;
                    }
                }

                if(itemsInInv >= Inventory.Instance.theInventory.Count)
                {
                    Debug.Log("Inventory full");
                    //Drop resource on floor
                }
                Destroy(gameObject);
            }
        }
    }
}
