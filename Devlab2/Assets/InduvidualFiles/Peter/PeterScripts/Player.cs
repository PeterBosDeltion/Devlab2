using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public List<GameObject> items = new List<GameObject>();
    public Weapon currentWeapon;
    public static int arrows = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("1"))
        {
            ChangeEquippedItem(0);
        }
        else if (Input.GetKeyDown("2"))
        {
            ChangeEquippedItem(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            ChangeEquippedItem(2);
        }
    }

    public void ChangeEquippedItem(int i)
    {
        if (!currentWeapon.beingUsed)
        {
            foreach (GameObject g in items)
            {
                if (i == items.IndexOf(g))
                {
                    g.SetActive(true);
                    //Inventory.itemInHand = g.GetComponent<Equippable>();
                    currentWeapon = g.GetComponent<Weapon>();
                }
                else
                {
                    g.SetActive(false);
                }
            }
        }
       
    }
}
