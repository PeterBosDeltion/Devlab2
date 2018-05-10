using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    List<InventorySlot> theInventory = new List<InventorySlot>();
    Item currentlyDragged;

    public InventorySlot mouseOver;
    public Image dragImage;

    [Header("Inventory Inspector")]
    public Text inspectorText;
    public Image inspectorImage;
    public List<InspectorButton> buttons = new List<InspectorButton>();

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
        //inspectorText.text = _HighlightItem.itemDiscription;
        inspectorImage.sprite = HighlightItem.item2D;
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
                return(true);
            }
        }
        return (false);
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

    [System.Serializable]
    public struct InspectorButton {
        public string myTag;
        public GameObject myButton;
    }
}
