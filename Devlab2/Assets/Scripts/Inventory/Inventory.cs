using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    public List<InventorySlot> theInventory = new List<InventorySlot>();
    public List<CharacterSlot> characterSlots = new List<CharacterSlot>();
    public static Equippable itemInHand;
    public static Slot mouseOver;
    public Image dragImage;
    Slot currentlyDragged;

    [Header("ToolBar")]
    public Image toolBarSelectedImage;
    public List<ToolBarSlot> toolBar = new List<ToolBarSlot>();
    public int SelectedToolbarSlot;

    [Header("Inventory Inspector")]
    public TextMeshProUGUI inspectorItemText;
    public Image inspectorItemImage;
    public List<InspectorButton> buttons = new List<InspectorButton>();
    public Slot currentInspected;

    [Header("Animations")]
    public Animator craftInspectAnimator;

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
        if(toolBar[SelectedToolbarSlot].myItem != null) {
            if(Input.GetButtonDown("Fire1")) {
                if(toolBar[SelectedToolbarSlot].myItem.GetType() == typeof(Consumable)) {
                    ConsumeItem();
                }
                else if(toolBar[SelectedToolbarSlot].myItem.GetType() == typeof(Wearable)) {
                    EquipeItem();
                }
            }

            if(Input.GetButtonDown("DropItem")) {
                if(toolBar[SelectedToolbarSlot].myItem.placaBle != true) {
                    DropItem();
                }
            }
        }
    }

    public void ChangeToolBarSelected() {
        currentInspected = toolBar[SelectedToolbarSlot];
        toolBarSelectedImage.rectTransform.position = toolBar[SelectedToolbarSlot].ToolbarImage.rectTransform.position;
        Builder.instance.StopBuild();
        if(toolBar[SelectedToolbarSlot].myItem != null && toolBar[SelectedToolbarSlot].myItem.placaBle == true) {
            Builder.instance.StartBuilder(toolBar[SelectedToolbarSlot].myItem);
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
    public void DropItem(Slot itemToDrop) {
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
        Consumable consumedItem = (Consumable)currentInspected.myItem;

        Player.instance.Eat(consumedItem.food);
        Player.instance.Drink(consumedItem.water);

        currentInspected.RemoveItem();
        InspectorReset();
    }
    #endregion

    //Inspects Given Item
    public void InspectItem(Slot itemToInspect) {
        currentInspected = itemToInspect;

        inspectorItemText.text = itemToInspect.myItem.itemDiscription;

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
            for(int r = 0; r < CraftingRecipesList[fullSlots][rr].ingredients.Count; r++) {
                for(int ii = 0; ii < craftingSlots.Count; ii++) {
                    if(craftingSlots[ii].myItem != null && craftingSlots[ii].myItem.itemName == CraftingRecipesList[fullSlots][rr].ingredients[r]) {
                        k++;
                        if(k == fullSlots) {
                            productSlot.SetItem(Instantiate(CraftingRecipesList[fullSlots][rr].product));
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
            if(mouseOver.myItem == null) {
                for(int c = 0; c < craftingSlots.Count; c++) {
                    if(craftingSlots[c].myItem != null) {
                        craftingSlots[c].RemoveItem();
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