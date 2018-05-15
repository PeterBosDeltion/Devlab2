using System.Collections;
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

    public void Harvest()
    {
        if(Inventory.itemInHand.myGathering == type)
        {
            //Inventory.Instance.AddItem(myResource); //Uncomment when there is inventory in scene pl0x

            if(toughness > 0)
            {
                toughness--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
