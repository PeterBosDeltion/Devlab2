using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable {
    public Item myItem;

    public override void Interact() {
        if(Inventory.Instance.AddItem(myItem)) {
            ObjectPooler.instance.AddToPool(myItem.itemName,gameObject);
        }
    }
}
