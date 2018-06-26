using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    public Item myItem;
    public Image itemImage;
    public Item.TypeOffItem myType;

    public void InspectItem() {
        if (myItem != null) {
            Inventory.Instance.InspectItem(this);
        }
    }

    void Start() {
        if (myItem != null) {
            SetItem(myItem);
        } else {
            itemImage.gameObject.SetActive(false);
        }
    }

    //Removes Currently Held Item
    public virtual void RemoveItem() {
        myItem = null;
        itemImage.gameObject.SetActive(false);
    }

    //Changes Item And Item Components
    public virtual void SetItem(Item newItem) {
        if (newItem != null) {
            myItem = Instantiate(newItem);
            itemImage.sprite = myItem.item2D;
            itemImage.gameObject.SetActive(true);
        } else {
            RemoveItem();
        }
    }

    //Stops Item Drag
    public virtual void StopItemDrag() {
        if (myItem != null) {
            if (Inventory.mouseOver != null) {
                if (Inventory.mouseOver.GetType()== typeof(StorageSlot)&& myItem.itemName == Interactor.instance.myStorage.storageItem.itemName || Inventory.mouseOver.GetType()!= typeof(StorageSlot)) {

                    if (Inventory.mouseOver.myItem != null) {
                        if (Inventory.mouseOver.GetType()!= typeof(CharacterSlot)&& GetType()!= typeof(CharacterSlot)|| Inventory.mouseOver.GetType()== typeof(CharacterSlot)&& Inventory.mouseOver.myItem.itemType == myItem.itemType) {
                            Item otherItem = Instantiate(Inventory.mouseOver.myItem);

                            Inventory.mouseOver.SetItem(myItem);
                            SetItem(otherItem);
                        } else {
                            itemImage.gameObject.SetActive(true);
                        }
                    } else if (Inventory.mouseOver.GetType()!= typeof(CharacterSlot)|| Inventory.mouseOver.GetType()== typeof(CharacterSlot)&& Inventory.mouseOver.myType == myItem.itemType) {
                        Inventory.mouseOver.SetItem(Instantiate(myItem));
                        RemoveItem();
                    } else {
                        itemImage.gameObject.SetActive(true);
                    }
                    if (Inventory.mouseOver.GetType()== typeof(CraftingSlot)|| GetType()== typeof(CraftingSlot)) {
                        Inventory.Instance.CheckRecipe();
                    }
                } else {
                    itemImage.gameObject.SetActive(true);
                }
            } else {
                itemImage.gameObject.SetActive(true);
            }
            Inventory.Instance.StopDrag();
        }
    }

    //Starts Item Drag
    public virtual void StartItemDrag() {
        if (myItem != null) {
            itemImage.gameObject.SetActive(false);
            Inventory.Instance.StartDrag(this);
        }
    }

    public virtual void MouseEnter() {
        Inventory.mouseOver = this;
    }

    public virtual void MouseExit() {
        if (Inventory.mouseOver == this) {
            Inventory.mouseOver = null;
        }
    }
}