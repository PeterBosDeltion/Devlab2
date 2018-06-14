using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : Slot {
    public Animator slotAnimator;
    public bool productSlot;
    public bool isChecked;

    public override void MouseEnter() {
        slotAnimator.SetBool("Enter", true);
        base.MouseEnter();
    }

    public override void MouseExit() {
        slotAnimator.SetBool("Enter", false);
        base.MouseExit();
    }

    public override void StopItemDrag() {
        if(productSlot == false) {
            base.StopItemDrag();
            return;
        }

        if(myItem != null) {
            if(Inventory.mouseOver != null) {
                if(Inventory.mouseOver.myItem != null) {
                    itemImage.gameObject.SetActive(true);
                }
                else if(Inventory.mouseOver.GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.mouseOver.myType == myItem.itemType) {
                    if(Inventory.mouseOver.GetType() != typeof(CraftingSlot)) {
                        Item newitem = myItem;
                        if(productSlot == true){
                            Inventory.Instance.CraftProduct();
                        }
                        Inventory.mouseOver.SetItem(Instantiate(newitem));
                        RemoveItem();
                    }
                    else {
                        itemImage.gameObject.SetActive(true);
                    }
                }
                if(Inventory.mouseOver.GetType() == typeof(CraftingSlot) || GetType() == typeof(CraftingSlot)) {
                    Inventory.Instance.CheckRecipe();
                }
            }
            else {
                itemImage.gameObject.SetActive(true);
            }
            Inventory.Instance.StopDrag();
        }
    }
}
