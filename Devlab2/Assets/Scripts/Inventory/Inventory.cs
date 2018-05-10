using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    public List<InventorySlot> theInventory = new List<InventorySlot>();

    Item currentlyDragged;

    public InventorySlot mouseOver;
    public Image dragImage;

    [Header("Inventory Inspector")]
    public Text inspectorItemText;
    public Image inspectorItemImage;
    public Text inspectorItemAmount;
    public List<InspectorButton> buttons = new List<InspectorButton>();

    [Header("Animations")]
    public Animator craftInspectAnimator;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if(currentlyDragged != null) {
            dragImage.transform.position = Input.mousePosition;
        }
    }

    public void StartDrag(Item draggedItem) {
        currentlyDragged = draggedItem;
        dragImage.sprite = draggedItem.item2D;
        dragImage.enabled = true;
    }

    public void StopDrag() {
        currentlyDragged = null;
        dragImage.enabled = false;
    }

    public void HighlightItem(Item HighlightItem) {
        /*inspectorText.text = _HighlightItem.itemDiscription;
        if(HighlightItem.amountCap > 0) {
            inspectorItemAmount.enabled = true;
            inspectorItemAmount.text = HighlightItem.amount.ToString();
        }
        else {
            inspectorItemAmount.enabled = false;
        }*/
        inspectorItemImage.sprite = HighlightItem.item2D;
        for(int i = 0; i < buttons.Count; i++) {
            buttons[i].myButton.SetActive(false);
        }

        for(int b = 0; b < HighlightItem.myButtons.Count; b++) {
            for(int bb = 0; bb < buttons.Count; bb++) {
                if(HighlightItem.myButtons[b] == buttons[bb].myTag) {
                    buttons[bb].myButton.SetActive(true);
                    break;
                }
            }
        }
    }

    public bool AddItem(Item newItem) {
        for(int i = 0; i < theInventory.Count; i++) {
            if(theInventory[i].myItem == null) {
                theInventory[i].SetItem(newItem);
                return (true);
            }
        }
        return (false);
    }

    public void InspectorCraftingSwitch() {
        craftInspectAnimator.SetBool("Switch",!craftInspectAnimator.GetBool("Switch"));
    }

    #region Inspector Buttons
    public void DropItem() {
        if(mouseOver != null && mouseOver.myItem != null) {

        }
    }

    public void EquipeItem() {
        if(mouseOver != null && mouseOver.myItem != null) {

        }
    }

    public void ConsumeItem() {
        if(mouseOver != null && mouseOver.myItem != null) {

        }
    }
    #endregion

    #region Crafting
    [Header("Crafting")]
    public List<InventorySlot> craftingSlots = new List<InventorySlot>();
    public List<Recipe> CraftingRecipes = new List<Recipe>();
    public InventorySlot productSlot;

    public void CheckRecipe() {
        if(productSlot.myItem != null) {
            return;
        }

        int fullSlots = 0;
        for(int r = 0; r < craftingSlots.Count; r++) {
            if(craftingSlots[r].myItem != null) {
                fullSlots++;
            }
        }

        Debug.Log("bugg");

        for(int rr = 0; rr < CraftingRecipes.Count; rr++) {
            if(CraftingRecipes[rr].ingredients.Count - 1 == fullSlots) {
                for(int ii = 0; ii < CraftingRecipes[rr].ingredients.Count; ii++) {
                    for(int i = 0; i < craftingSlots.Count; i++) {
                        if(craftingSlots[i].myItem != null) {
                            if(CraftingRecipes[rr].ingredients[ii] == craftingSlots[i].myItem.name && craftingSlots.Count - 1 != i) {
                                productSlot.SetItem(CraftingRecipes[rr].product);
                                for(int c = 0; c < craftingSlots.Count; c++) {
                                    if(craftingSlots[c].myItem.amount - 1 <= 0) {
                                        craftingSlots[c].myItem = null;
                                    }
                                    else {
                                        craftingSlots[c].myItem.amount -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    #endregion

    [System.Serializable]
    public struct InspectorButton {
        public string myTag;
        public GameObject myButton;
    }

    [System.Serializable]
    public class Recipe {
        public List<string> ingredients = new List<string>();
        public Item product;
    }
}
