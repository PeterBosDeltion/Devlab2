using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : Slot {
    public Sprite baseImage;

    void Start() {
        itemImage.sprite = baseImage;
    }

    public override void RemoveItem() {
        myItem = null;
        itemImage.sprite = baseImage;
    }

    public override void SetItem(Item newItem) {
        myItem = Instantiate(newItem);
        itemImage.sprite = myItem.item2D;
    }

    public override void StartItemDrag() {
        if(myItem != null) {
            itemImage.sprite = baseImage;
            Inventory.Instance.StartDrag(this);
        }
    }
}
