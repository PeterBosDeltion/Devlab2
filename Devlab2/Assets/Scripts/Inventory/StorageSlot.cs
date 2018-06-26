using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageSlot : Slot {
    public Animator slotAnimator;
    public int mySlot;

    public override void MouseEnter() {
        slotAnimator.SetBool("Enter", true);
        base.MouseEnter();
    }

    public override void MouseExit() {
        slotAnimator.SetBool("Enter", false);
        base.MouseExit();
    }

    public override void StopItemDrag() {
        if (myItem != null) {
            if (Inventory.mouseOver != null) {
                if (Inventory.mouseOver.myItem != null) {
                    if (Inventory.mouseOver.GetType()!= typeof(CharacterSlot)&& GetType()!= typeof(CharacterSlot)|| Inventory.mouseOver.GetType()== typeof(CharacterSlot)&& Inventory.mouseOver.myItem.itemType == myItem.itemType || Inventory.mouseOver.GetType()== typeof(StorageSlot)&& myItem == Interactor.instance.myStorage.storageItem) {
                        Item otherItem = Instantiate(Inventory.mouseOver.myItem);

                        Inventory.mouseOver.SetItem(myItem);
                        SetItem(otherItem);
                        Interactor.instance.setStorageItem(mySlot);
                    } else {
                        itemImage.gameObject.SetActive(true);
                    }
                } else if (Inventory.mouseOver.GetType()!= typeof(CharacterSlot)|| Inventory.mouseOver.GetType()== typeof(CharacterSlot)&& Inventory.mouseOver.myType == myItem.itemType) {
                    Inventory.mouseOver.SetItem(Instantiate(myItem));
                    RemoveItem();
                    Interactor.instance.setStorageItem(mySlot);
                } else {
                    itemImage.gameObject.SetActive(true);
                }
                if (Inventory.mouseOver.GetType()== typeof(CraftingSlot)|| GetType()== typeof(CraftingSlot)) {
                    Inventory.Instance.CheckRecipe();
                }
            } else {
                itemImage.gameObject.SetActive(true);
            }
            Inventory.Instance.StopDrag();
        }
    }

    public override void SetItem(Item newItem) {
        if (newItem != null) {
            myItem = Instantiate(newItem);
            itemImage.sprite = myItem.item2D;
            itemImage.gameObject.SetActive(true);
            Interactor.instance.setStorageItem(mySlot);
        } else {
            RemoveItem();
        }
    }

    public override void RemoveItem() {
        base.RemoveItem();
        Interactor.instance.setStorageItem(mySlot);
    }
}