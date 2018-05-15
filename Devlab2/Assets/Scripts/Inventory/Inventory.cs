using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    public List<InventorySlot> theInventory = new List<InventorySlot>();
    public List<CharacterSlot> characterSlots = new List<CharacterSlot>();
    public Image dragImage;
    Slot currentlyDragged;
    public static Slot mouseOver;
    public static Equippable itemInHand;

    [Header("Inventory Inspector")]
    public TextMeshProUGUI inspectorItemText;
    public Image inspectorItemImage;
    public TextMeshProUGUI inspectorItemAmount;
    public List<InspectorButton> buttons = new List<InspectorButton>();
    Slot currentInspected;

    [Header("Animations")]
    public Animator craftInspectAnimator;
    public Animator InventoryAnimator;

    void Awake() {
        Instance = this;
        SortRecipes();
    }

    void Start() {
        dragImage.enabled = false;
    }

    void Update() {
        if(currentlyDragged != null) {
            dragImage.transform.position = Input.mousePosition;
        }
    }

    public static void EnableInventory(bool state) {
        Instance.InventoryAnimator.SetBool("Enabled", state);
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
        if(newItem != null) {
            for(int i = 0; i < theInventory.Count; i++) {
                if(theInventory[i].myItem == null) {
                    theInventory[i].SetItem(newItem);
                    return (true);
                }
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

    //Starts The Builder
    public void PlaceItem(){
        Builder.instance.StartBuilder(currentInspected.myItem);
        InspectorReset();
        currentInspected.RemoveItem();
        UIManager.instance.SetCanvas(UIManager.UIState.Builder);
    }

    //Equipes Currently Inspected Item
    public void EquipeItem() {
        for(int i = 0; i < characterSlots.Count; i++) {
            if(characterSlots[i].myType == currentInspected.myItem.itemType) {
                if(characterSlots[i].myItem != null) {
                    Item localItem = characterSlots[i].myItem;
                    characterSlots[i].SetItem(currentInspected.myItem);
                    currentInspected.SetItem(localItem);
                    InspectorReset();
                }
                else {
                    characterSlots[i].SetItem(currentInspected.myItem);
                    currentInspected.RemoveItem();
                    InspectorReset();
                }
                break;
            }
        }
    }

    //Consumes Currently Inspected Item
    public void ConsumeItem() {
        //          ***add food and or water to stats
    }
    #endregion

    //Inspects Given Item
    public void InspectItem(Slot itemToInspect) {
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
        inspectorItemText.text = "Select item to inspect";
    }

    //Button For Inspector
    [System.Serializable]
    public struct InspectorButton {
        public string myTag;
        public GameObject myButton;
    }
    #endregion

    #region Crafting
    [Header("Crafting")]
    public List<CraftingSlot> craftingSlots = new List<CraftingSlot>();
    public List<Recipe> CraftingRecipes = new List<Recipe>();
    List<Recipe>[] CraftingRecipesList = new List<Recipe>[5];
    //List<List<Recipe>> CraftingRecipesList = new List<List<Recipe>>();
    public CraftingSlot productSlot;

    void SortRecipes() {
        for(int i = 0; i < CraftingRecipesList.Length; i++) {
            CraftingRecipesList[i] = new List<Recipe>();
        }

        foreach(Recipe aRecipe in CraftingRecipes) {
            CraftingRecipesList[aRecipe.ingredients.Count].Add(aRecipe);
        }
    }

    //Checks If Recipe Is Correct        ***NON EFFICIENT
    public void CheckRecipe() {
        productSlot.RemoveItem();

        int fullSlots = 0;
        for(int r = 0; r < craftingSlots.Count; r++) {
            if(craftingSlots[r].myItem != null) {
                fullSlots++;
            }
        }

        for(int rr = 0; rr < CraftingRecipesList[fullSlots].Count; rr++) {
            int k = 0;
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

    //Recipe To Check
    [System.Serializable]
    public class Recipe {
        public List<string> ingredients = new List<string>();
        public Item product;
        public int amount;
    }
    #endregion
}