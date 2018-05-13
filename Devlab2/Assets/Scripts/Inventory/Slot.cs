using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour {
    public Item myItem;
    public Image itemImage;
    public TextMeshProUGUI amountText;
    public Item.TypeOffItem myType;

    public void InspectItem() {
        if(myItem != null) {
            Inventory.Instance.InspectItem(this);
        }
    }

    void Start() {
        if(myItem != null) {
            SetItem(myItem);
        }
        else {
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
        myItem = Instantiate(newItem);
        itemImage.sprite = myItem.item2D;
        itemImage.gameObject.SetActive(true);

        if(myItem.amountCap > 0) {
            amountText.text = myItem.amount.ToString();
            amountText.enabled = true;
        }
        else {
            amountText.enabled = false;
        }
    }

    //Changes The Amount Of Current Item And Updates Text
    public void ChangeItemAmount(int amount) {
        myItem.amount = amount;
        amountText.text = amount.ToString();
    }

    //Stops Item Drag
    public virtual void StopItemDrag() {
        if(myItem != null && Inventory.mouseOver != null) {
            if(Inventory.mouseOver.myItem != null) {
                if(Inventory.mouseOver.GetType() != typeof(CharacterSlot) && GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.mouseOver.myItem.itemType == myItem.itemType) {
                    Item otherItem = Instantiate(Inventory.mouseOver.myItem);

                    if(otherItem.itemName == myItem.name) { //          ***bugggg Same names are not true
                        int leftOver = otherItem.amount + myItem.amount - otherItem.amountCap;
                        if(leftOver > 0) {
                            myItem.amount = leftOver;
                            Inventory.mouseOver.ChangeItemAmount(otherItem.amountCap);
                            itemImage.gameObject.SetActive(true);
                        }
                        else {
                            otherItem.amount += myItem.amount;
                            RemoveItem();
                        }
                    }
                    else {
                        Inventory.mouseOver.SetItem(myItem);
                        SetItem(otherItem);
                    }
                }
                else {
                    itemImage.gameObject.SetActive(true);
                }
            }
            else if(Inventory.mouseOver.GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.mouseOver.myType == myItem.itemType) {
                Inventory.mouseOver.SetItem(Instantiate(myItem));
                RemoveItem();
            }
            else {
                itemImage.gameObject.SetActive(true);
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

    //Starts Item Drag
    public virtual void StartItemDrag() {
        if(myItem != null) {
            itemImage.gameObject.SetActive(false);
            Inventory.Instance.StartDrag(this);
        }
    }

    public virtual void MouseEnter() {
        Inventory.mouseOver = this;
    }

    public virtual void MouseExit() {
        if(Inventory.mouseOver == this) {
            Inventory.mouseOver = null;
        }
    }
}
