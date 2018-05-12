using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : Slot {
    public Animator slotAnimator;
    public bool productSlot;

    public override void MouseEnter() {
        slotAnimator.SetBool("Enter", true);
        Inventory.Instance.mouseOver = this;
    }

    public override void MouseExit() {
        slotAnimator.SetBool("Enter", false);
        if(Inventory.Instance.mouseOver == this) {
            Inventory.Instance.mouseOver = null;
        }
    }

    public override void StopItemDrag() {
        if(productSlot == false) {
            base.StopItemDrag();
            return;
        }

        if(myItem != null && Inventory.Instance.mouseOver != null) {
            if(Inventory.Instance.mouseOver.myItem != null) {
                itemImage.gameObject.SetActive(true);
            }
            else if(Inventory.Instance.mouseOver.GetType() != typeof(CharacterSlot) || Inventory.Instance.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.Instance.mouseOver.myType == myItem.itemType) {
                if(Inventory.Instance.mouseOver.GetType() != typeof(CraftingSlot)) {
                    Item newitem = myItem;
                    Inventory.Instance.CraftProduct();
                    Inventory.Instance.mouseOver.SetItem(Instantiate(newitem));
                    RemoveItem();
                }
                else {
                    itemImage.gameObject.SetActive(true);
                }
            }
            if(Inventory.Instance.mouseOver.GetType() == typeof(CraftingSlot) || GetType() == typeof(CraftingSlot)) {
                Inventory.Instance.CheckRecipe();
            }
        }
        else {
            itemImage.gameObject.SetActive(true);
        }
        Inventory.Instance.StopDrag();
    }
}
