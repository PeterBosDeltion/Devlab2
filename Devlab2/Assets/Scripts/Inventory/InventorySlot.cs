using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public Item myItem;
    public Image itemImage;
    public Text amountText;

    void Start() {
        if(myItem != null) {
            SetItem(myItem);
        }
        else {
            itemImage.enabled = false;
        }
    }

    public void SetItem(Item newItem) {
        myItem = Instantiate(newItem);
        itemImage.sprite = myItem.item2D;
        itemImage.enabled = true;
        /*.text = myItem.amount.ToString();
        if(myItem.amountCap > 0) {
            amountText.enabled = true;
        }
        else {
            amountText.enabled = false;
        }*/
        //posible animation
    }

    void RemoveItem() {
        myItem = null;
        itemImage.enabled = false;
    }

    public void StartItemDrag() {
        if(myItem != null) {
            itemImage.enabled = false;
            Inventory.Instance.StartDrag(myItem);
        }
    }

    public void StopItemDrag() {
        Debug.Log(gameObject);
        if(Inventory.Instance.mouseOver != null) {
            Item otherItem = Instantiate(Inventory.Instance.mouseOver.myItem);
            if(otherItem != null) {
                if(otherItem.itemName == myItem.name && otherItem.amount < otherItem.amountCap) {
                    int leftOver = otherItem.amount + myItem.amount - otherItem.amountCap;
                    if(leftOver >= 0) {
                        myItem.amount = leftOver;
                        Inventory.Instance.mouseOver.ChangeItemAmount(otherItem.amountCap);
                        itemImage.enabled = true;
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
                Inventory.Instance.mouseOver.SetItem(myItem);
                myItem = null;
            }

        }
        else {
            itemImage.enabled = true;
        }
        Inventory.Instance.StopDrag();
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
