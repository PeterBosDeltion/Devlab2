using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineSlot : Slot {
    public Animator slotAnimator;
    public bool productSlot;
    public bool fuelSlot;
    public Image fillImage;
    public int myIndex;

    public override void MouseEnter() {
        slotAnimator.SetBool("Enter", true);
        base.MouseEnter();
    }

    public override void MouseExit() {
        slotAnimator.SetBool("Enter", false);
        base.MouseExit();
    }

    public override void StartItemDrag() {
        if (productSlot == false || fillImage.fillAmount <= 0.01f) {
            base.StartItemDrag();
        }
    }

    public override void RemoveItem() {
        base.RemoveItem();

        if (productSlot == true && fuelSlot == true) {
            Interactor.instance.myCraftingMachine.productFuelSlots[myIndex] = null;
        }
    }

    public override void SetItem(Item newItem) {
        base.SetItem(newItem);
        if (newItem != null) {
            if (productSlot == true) {
                if (fuelSlot == true) {
                    Interactor.instance.myCraftingMachine.productFuelSlots[myIndex] = myItem;
                }
            } else {
                if (fuelSlot == true) {
                    Interactor.instance.myCraftingMachine.craftFuelSlots[myIndex] = myItem;
                } else {
                    Interactor.instance.myCraftingMachine.craftSlots[myIndex] = myItem;
                }
            }
            Interactor.instance.myCraftingMachine.CheckResipe();
        } else {
            RemoveItem();
        }

    }

    public override void StopItemDrag() {
        Interactor.instance.myCraftingMachine.CheckResipe();

        if (productSlot == false) {
            base.StopItemDrag();
            if (fuelSlot == true) {
                Interactor.instance.myCraftingMachine.craftFuelSlots[myIndex] = myItem;
            } else {
                Interactor.instance.myCraftingMachine.craftSlots[myIndex] = myItem;
            }
            return;
        }

        if (fillImage.fillAmount > 0.01f) {
            return;
        }
        if (myItem != null) {
            if (Inventory.mouseOver != null) {
                if (Inventory.mouseOver.myItem != null) {
                    itemImage.gameObject.SetActive(true);
                } else if (Inventory.mouseOver.GetType()!= typeof(CharacterSlot)|| Inventory.mouseOver.GetType()== typeof(CharacterSlot)|| Inventory.mouseOver.GetType()== typeof(StorageSlot)&& myItem.itemName == Interactor.instance.myStorage.storageItem.itemName) {
                    if (Inventory.mouseOver.GetType()!= typeof(CraftingSlot)) {
                        Inventory.mouseOver.SetItem(Instantiate(myItem));
                        RemoveItem();
                    } else {
                        itemImage.gameObject.SetActive(true);
                    }
                }
                if (Inventory.mouseOver.GetType()== typeof(CraftingSlot)|| GetType()== typeof(CraftingSlot) || Inventory.mouseOver.GetType()== typeof(StorageSlot)&& myItem.itemName == Interactor.instance.myStorage.storageItem.itemName) {}
            } else {
                itemImage.gameObject.SetActive(true);
            }
            Inventory.Instance.StopDrag();
        }
        Interactor.instance.myCraftingMachine.CheckResipe();

        if (productSlot == true) {
            if (fuelSlot == true) {
                Interactor.instance.myCraftingMachine.craftFuelSlots[myIndex] = myItem;
            } else {
                Interactor.instance.myCraftingMachine.craftSlots[myIndex] = myItem;
            }
        }
    }
}