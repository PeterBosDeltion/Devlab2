using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolBarSlot : InventorySlot {
    public Image ToolbarImage;
    public TextMeshProUGUI ToolbarAmountText;

    void Start() {
        if(myItem != null) {
            SetItem(myItem);
        }
        else {
            ToolbarImage.gameObject.SetActive(false);
            itemImage.gameObject.SetActive(false);
        }
    }

    public override void StopItemDrag() {
        if(myItem != null) {
            if(Inventory.mouseOver != null) {
                if(Inventory.mouseOver.myItem != null) {
                    if(Inventory.mouseOver.GetType() != typeof(CharacterSlot) && GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.mouseOver.myItem.itemType == myItem.itemType) {
                        Item otherItem = Instantiate(Inventory.mouseOver.myItem);

                        if(otherItem.itemName == myItem.name) { //          ***bugggg Same names are not true
                            int leftOver = otherItem.amount + myItem.amount - otherItem.amountCap;
                            if(leftOver > 0) {
                                myItem.amount = leftOver;
                                Inventory.mouseOver.ChangeItemAmount(otherItem.amountCap);
                                itemImage.gameObject.SetActive(true);
                                ToolbarImage.gameObject.SetActive(true);
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
                        ToolbarImage.gameObject.SetActive(true);
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
    }

    public override void RemoveItem() {
        myItem = null;
        ToolbarImage.gameObject.SetActive(false);
        itemImage.gameObject.SetActive(false);
    }

    //Changes Item And Item Components
    public override void SetItem(Item newItem) {
        Inventory.Instance.ChangeToolBarSelected();

        myItem = Instantiate(newItem);

        itemImage.sprite = myItem.item2D;
        ToolbarImage.sprite = myItem.item2D;

        itemImage.gameObject.SetActive(true);
        ToolbarImage.gameObject.SetActive(true);

        if(myItem.amountCap > 0) {
            amountText.text = myItem.amount.ToString();
            ToolbarAmountText.text = myItem.amount.ToString();

            amountText.enabled = true;
            ToolbarAmountText.enabled = true;
        }
        else {
            amountText.enabled = false;
            ToolbarAmountText.enabled = false;
        }
    }

    //Changes The Amount Of Current Item And Updates Text
    public override void ChangeItemAmount(int amount) {
        myItem.amount = amount;

        ToolbarAmountText.text = amount.ToString();
        amountText.text = amount.ToString();
    }

    //Starts Item Drag
    public override void StartItemDrag() {
        if(myItem != null) {
            itemImage.gameObject.SetActive(false);
            ToolbarImage.gameObject.SetActive(false);

            Inventory.Instance.StartDrag(this);
        }
    }
}
