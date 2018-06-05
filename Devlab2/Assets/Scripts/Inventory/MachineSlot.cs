using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSlot : Slot {
    public Animator slotAnimator;
    public bool productSlot;
    public CraftMachine myCraft;
    public bool productDone;

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

        if(productDone != true){
            return;
        }

        if(myItem != null) {
            if(Inventory.mouseOver != null) {
                if(Inventory.mouseOver.myItem != null) {
                    itemImage.gameObject.SetActive(true);
                }
                else if(Inventory.mouseOver.GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.mouseOver.myType == myItem.itemType) {
                    if(Inventory.mouseOver.GetType() != typeof(CraftingSlot)) {
                        productDone = false;
                        Item newitem = myItem;
                        Inventory.mouseOver.SetItem(Instantiate(newitem));
                        RemoveItem();
                    }
                    else {
                        itemImage.gameObject.SetActive(true);
                    }
                }
                if(Inventory.mouseOver.GetType() == typeof(CraftingSlot) || GetType() == typeof(CraftingSlot)) {
                    myCraft.CheckResipe();
                }
            }
            else {
                itemImage.gameObject.SetActive(true);
            }
            Inventory.Instance.StopDrag();
        }
    }

    public IEnumerator Processing(){
        yield return new WaitForSeconds(myCraft.timeToConvert);
        productDone = true;
    }
}
