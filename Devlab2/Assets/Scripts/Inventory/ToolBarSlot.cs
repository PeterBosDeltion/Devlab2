using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolBarSlot : InventorySlot {
    public Image ToolbarImage;
    private Player player;
    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
                    if(Inventory.mouseOver.GetType() != typeof(CharacterSlot) && GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.mouseOver.myItem.itemType == myItem.itemType || Inventory.mouseOver.GetType()== typeof(StorageSlot)&& myItem.itemName == Interactor.instance.myStorage.storageItem.itemName) {
                        Item otherItem = Instantiate(Inventory.mouseOver.myItem);

                        Inventory.mouseOver.SetItem(myItem);
                        SetItem(otherItem);
                    }
                    else {
                        itemImage.gameObject.SetActive(true);
                        ToolbarImage.gameObject.SetActive(true);
                    }
                }
                else if(Inventory.mouseOver.GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot) && Inventory.mouseOver.myType == myItem.itemType || Inventory.mouseOver.GetType()== typeof(StorageSlot)&& myItem.itemName == Interactor.instance.myStorage.storageItem.itemName) {
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
        player.ChangeEquippedItem(Inventory.Instance.SelectedToolbarSlot);
    }

    //Changes Item And Item Components
    public override void SetItem(Item newItem) {
        Inventory.Instance.ChangeToolBarSelected();

        myItem = Instantiate(newItem);

        itemImage.sprite = myItem.item2D;
        ToolbarImage.sprite = myItem.item2D;

        itemImage.gameObject.SetActive(true);
        ToolbarImage.gameObject.SetActive(true);
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
