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

    void Start() {
        if(myItem != null) {
            SetItem(myItem);
        }
        else {
            itemImage.gameObject.SetActive(false);
        }
    }

    public virtual void RemoveItem() {
        myItem = null;
        itemImage.gameObject.SetActive(false);
    }

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
        //posible animation
    }

    public void ChangeItemAmount(int amount) {
        myItem.amount = amount;
        amountText.text = amount.ToString();
    }

    public virtual void StopItemDrag() {
        if(myItem != null && Inventory.Instance.mouseOver != null) {
            if(Inventory.Instance.mouseOver.myItem != null) {
                if(Inventory.Instance.mouseOver.GetType() != typeof(CharacterSlot) && GetType() != typeof(CharacterSlot) || Inventory.Instance.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.Instance.mouseOver.myItem.itemType == myItem.itemType) {
                    Item otherItem = Instantiate(Inventory.Instance.mouseOver.myItem);

                    if(otherItem.itemName == myItem.name) { //bugggg
                        int leftOver = otherItem.amount + myItem.amount - otherItem.amountCap;
                        if(leftOver > 0) {
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
                    itemImage.gameObject.SetActive(true);
                }
            }
            else if(Inventory.Instance.mouseOver.GetType() != typeof(CharacterSlot) || Inventory.Instance.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.Instance.mouseOver.myType == myItem.itemType) {
                    Inventory.Instance.mouseOver.SetItem(Instantiate(myItem));
                    RemoveItem();
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

    public virtual void StartItemDrag() {
        if(myItem != null) {
            itemImage.gameObject.SetActive(false);
            Inventory.Instance.StartDrag(this);
        }
    }

    public virtual void MouseEnter() {
        Inventory.Instance.mouseOver = this;
    }

    public virtual void MouseExit() {
        if(Inventory.Instance.mouseOver == this) {
            Inventory.Instance.mouseOver = null;
        }
    }
}
