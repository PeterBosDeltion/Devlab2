using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public enum UIState {
        BaseCanvas,
        Inventory,
        PauseMenu,
        Builder
    }

    public Canvas pauseMenu, inventory, baseCanvas, builderCanvas;

    public UIState currentUI;

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
    public void SetCanvas(UIState _newState) {
        currentUI = _newState;
        switch(currentUI) {
            case UIState.BaseCanvas:
            SetpauseCanvas(false);
            SetInventoryCanvas(false);
            SetBuilderCanvas(false);
            SetBaseCanvas(true);
            break;
            case UIState.Inventory:
            Inventory.EnableInventory(true);
            SetBaseCanvas(false);
            break;
            case UIState.PauseMenu:
            SetpauseCanvas(true);
            SetBaseCanvas(false);
            break;
            case UIState.Builder:
            SetInventoryCanvas(false);
            SetBuilderCanvas(true);
            break;
            default:
            break;
        }
    }

    void SetBaseCanvas(bool state) {
        baseCanvas.enabled = state;
    }

    void SetBuilderCanvas(bool state) {
        builderCanvas.enabled = state;
    }

    void SetInventoryCanvas(bool state) {
        if(inventory.enabled == !state) {
            Inventory.EnableInventory(state);
        }
    }

    void SetpauseCanvas(bool state) {
        //Time.timeScale = state == true ? 0 : 1;
        pauseMenu.enabled = state;
    }
}
