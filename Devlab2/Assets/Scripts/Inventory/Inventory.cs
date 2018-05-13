using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    public List<InventorySlot> theInventory = new List<InventorySlot>();
    public Image dragImage;
    Slot currentlyDragged;
    public static Slot mouseOver;

    [Header("Inventory Inspector")]
    public TextMeshProUGUI inspectorItemText;
    public Image inspectorItemImage;
    public TextMeshProUGUI inspectorItemAmount;
    public List<InspectorButton> buttons = new List<InspectorButton>();
    Slot currentInspected;

    [Header("Animations")]
    public Animator craftInspectAnimator;

    void Awake() {
        Instance = this;
    }

    void Start() {
        dragImage.enabled = false;
    }

    void Update() {
        if(currentlyDragged != null) {
            dragImage.transform.position = Input.mousePosition;
        }
    }

    //Enables Inventory Drag Elements
    public void StartDrag(Slot draggedItem) {
        currentlyDragged = draggedItem;
        dragImage.sprite = draggedItem.myItem.item2D;
        dragImage.enabled = true;
    }

    //Disables Inventory Drag Elements
    public void StopDrag() {
        currentlyDragged = null;
        dragImage.enabled = false;
    }

    //Adds Given Item And Returns True If Posible Otherwise Returns False
    public bool AddItem(Item newItem) {
        for(int i = 0; i < theInventory.Count; i++) {
            if(theInventory[i].myItem == null) {
                theInventory[i].SetItem(newItem);
                return (true);
            }
        }
        return (false);
    }

    //Removes Given Item
    void DropItem(Slot itemToDrop) {
        if(itemToDrop != null && itemToDrop.myItem != null) {
            ObjectPooler.instance.GetFromPool(itemToDrop.myItem.itemName, Vector3.zero, Quaternion.Euler(Vector3.zero)); //No Place Choosen Yet
            itemToDrop.RemoveItem();
            InspectorReset();
        }

    }

    //Drops Currently Dragged Item
    public void RemoveCurrentItem() {
        DropItem(currentlyDragged);
    }

    public void InspectorCraftingSwitch() {
        craftInspectAnimator.SetBool("Switch", !craftInspectAnimator.GetBool("Switch"));
    }

    #region Inspector

    #region Inspector Buttons
    //Drops Currently Inspected Item
    public void DropItem() {
        DropItem(currentInspected);
    }

    //Equipes Currently Inspected Item
    public void EquipeItem() {

    }

    //Consumes Currently Inspected Item
    public void ConsumeItem() {

    }
    #endregion

    //Inspects Given Item
    public void InspectItem(InventorySlot itemToInspect) {
        currentInspected = itemToInspect;

        inspectorItemText.text = itemToInspect.myItem.itemDiscription;

        if(itemToInspect.myItem.amountCap > 0) {
            inspectorItemAmount.enabled = true;
            inspectorItemAmount.text = itemToInspect.myItem.amount.ToString();
        }
        else {
            inspectorItemAmount.enabled = false;
        }

        inspectorItemImage.sprite = itemToInspect.myItem.item2D;
        inspectorItemImage.enabled = true;
        for(int i = 0; i < buttons.Count; i++) {
            buttons[i].myButton.SetActive(false);
        }

        for(int b = 0; b < itemToInspect.myItem.myButtons.Count; b++) {
            for(int bb = 0; bb < buttons.Count; bb++) {
                if(itemToInspect.myItem.myButtons[b] == buttons[bb].myTag) {
                    buttons[bb].myButton.SetActive(true);
                    break;
                }
            }
        }
    }

    //Reset The Inspector To Basic
    void InspectorReset() {
        for(int i = 0; i < buttons.Count; i++) {
            buttons[i].myButton.SetActive(false);
        }

        inspectorItemImage.enabled = false;
        inspectorItemAmount.enabled = false;
        inspectorItemText.text = "";
    }
    #endregion

    #region Crafting
    [Header("Crafting")]
    public List<CraftingSlot> craftingSlots = new List<CraftingSlot>();
    public List<Recipe> CraftingRecipes = new List<Recipe>();
    public CraftingSlot productSlot;

    //Checks If Recipe Is Correct        ***NON EFFICIENT
    public void CheckRecipe() {
        productSlot.RemoveItem();

        int fullSlots = 0;
        for(int r = 0; r < craftingSlots.Count; r++) {
            if(craftingSlots[r].myItem != null) {
                fullSlots++;
            }
        }

        for(int rr = 0; rr < CraftingRecipes.Count; rr++) {
            int k = 0;
            if(CraftingRecipes[rr].ingredients.Count == fullSlots) {
                for(int r = 0; r < CraftingRecipes[rr].ingredients.Count; r++) {
                    for(int ii = 0; ii < craftingSlots.Count; ii++) {
                        if(craftingSlots[ii].myItem != null && craftingSlots[ii].myItem.itemName == CraftingRecipes[rr].ingredients[r]) {
                            k++;
                            if(k == fullSlots) {
                                productSlot.SetItem(Instantiate(CraftingRecipes[rr].product));
                                return;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    //Crafts Product
    public void CraftProduct() {
        if(productSlot.myItem != null && mouseOver != null) {
            if(mouseOver.myItem == null || mouseOver.myItem != null && mouseOver.myItem.amountCap > 0 && mouseOver.myItem.amount + productSlot.myItem.amount <= mouseOver.myItem.amountCap) {
                for(int c = 0; c < craftingSlots.Count; c++) {
                    if(craftingSlots[c].myItem != null) {
                        if(craftingSlots[c].myItem.amount - 1 <= 0) {
                            craftingSlots[c].RemoveItem();
                        }
                        else {
                            craftingSlots[c].myItem.amount -= 1;
                            craftingSlots[c].amountText.text = craftingSlots[c].myItem.amount.ToString();
                        }
                    }
                }
            }
        }
        CheckRecipe();
    }
    #endregion

    //Button For Inspector
    [System.Serializable]
    public struct InspectorButton {
        public string myTag;
        public GameObject myButton;
    }

    //Recipe To Check
    [System.Serializable]
    public class Recipe {
        public List<string> ingredients = new List<string>();
        public Item product;
        public int amount;
    }
}
