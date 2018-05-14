using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    enum UIState {
        BaseCanvas,
        Inventory,
        PauseMenu,
    }

    public Canvas pauseMenu, inventory,baseCanvas;

    UIState currentUI;

    void Awake() {
        instance = this;
    }

    void Start() {
        SetCanvas(UIState.BaseCanvas);
    }

    void Update() {
        if(Input.GetButtonDown("Cancel")) {
            SetCanvas(CancelCanvas());
        }
        if(Input.GetButtonDown("Inventory")) {
            if(currentUI == UIState.Inventory) {
                SetCanvas(CancelCanvas());
            }
            else {
                SetCanvas(UIState.Inventory);
            }
        }
    }

    //Steps Back A Canvas When Cancel Button Is Pressed
    UIState CancelCanvas() {
        switch(currentUI) {
            case UIState.BaseCanvas:
            return (UIState.PauseMenu);
            default:
            return (UIState.BaseCanvas);
        }
    }

    //Changes Current Canvas
    void SetCanvas(UIState _newState) {
        currentUI = _newState;
        switch(currentUI) {
            case UIState.BaseCanvas:
            pauseMenu.enabled = false;
            if(inventory.enabled == true) {
                Inventory.EnableInventory(false);
            }
            baseCanvas.enabled = true;
            //        ***changeToAnimation Later
            break;
            case UIState.Inventory:
            Inventory.EnableInventory(true);
            baseCanvas.enabled = false;
            //        ***changeToAnimation Later
            break;
            case UIState.PauseMenu:
            pauseMenu.enabled = true;
            baseCanvas.enabled = false;
            //        ***changeToAnimation Later
            break;
            default:
            break;
        }
    }
}
