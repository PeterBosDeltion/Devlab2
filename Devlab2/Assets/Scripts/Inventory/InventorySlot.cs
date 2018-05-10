using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public Item myItem;
    public Image itemImage;
    public Text amountText;
    public bool craftingSlot;

    void Start() {
        if(myItem != null) {
            SetItem(myItem);
        }
        else {
            itemImage.gameObject.SetActive(false);
        }
    }

    public void SetItem(Item newItem) {
        myItem = Instantiate(newItem);
        itemImage.sprite = myItem.item2D;
        itemImage.gameObject.SetActive(true);

        /*if(myItem.amountCap > 0) {
            amountText.text = myItem.amount.ToString();
            amountText.enabled = true;
        }
        else {
            amountText.enabled = false;
        }*/
        //posible animation
    }

    public void RemoveItem() {
        myItem = null;
        itemImage.gameObject.SetActive(false);
    }

    public void StartItemDrag() {
        if(myItem != null) {
            itemImage.gameObject.SetActive(false);
            Inventory.Instance.StartDrag(myItem);
        }
    }

    public void StopItemDrag() {
        if(myItem != null) {
            if(Inventory.Instance.mouseOver != null){
                if(Inventory.Instance.mouseOver.myItem != null) {

                    Item otherItem = Instantiate(Inventory.Instance.mouseOver.myItem);
                    if(otherItem.itemName == myItem.name && otherItem.amount < otherItem.amountCap) {
                        int leftOver = otherItem.amount + myItem.amount - otherItem.amountCap;
                        if(leftOver >= 0) {
                            myItem.amount = leftOver;
                            Inventory.Instance.mouseOver.ChangeItemAmount(otherItem.amountCap);
                            itemImage.gameObject.SetActive(true);
                        }
                        else {
                            otherItem.amount += myItem.amount;
                            RemoveItem();
                        }
                    }
                    else {
                        Inventory.Instance.mouseOver.SetItem(myItem);
                        SetItem(otherItem);
                    }
                }
                else {
                    Inventory.Instance.mouseOver.SetItem(Instantiate(myItem));
                    RemoveItem();
                }
                if(Inventory.Instance.mouseOver.craftingSlot == true || craftingSlot == true) {
                    Inventory.Instance.CheckRecipe();
                }
            }
            else {
                itemImage.gameObject.SetActive(true);
            }
            Inventory.Instance.StopDrag();
        }
    }

    public void HighlightItem() {
        if(myItem != null) {
            Inventory.Instance.HighlightItem(myItem);
        }
    }

    public void ChangeItemAmount(int amount) {
        myItem.amount = amount;
        //amountText.text = _amount.ToString();
    }

    public void MouseEnter() {
        Inventory.Instance.mouseOver = this;
    }

    public void MouseExit() {
        if(Inventory.Instance.mouseOver == this) {
            Inventory.Instance.mouseOver = null;
        }
    }
}
