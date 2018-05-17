using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public UIState currentUI;
    public enum UIState {
        BaseCanvas = 0,
        PauseMenu = 1,
        Inventory = 2,
        Builder = 3
    }

    public Canvas pauseMenu, inventory, baseCanvas, builderCanvas;

    [Header("Animation")]
    public Animator CanvasAnimator;

    void Awake() {
        instance = this;
    }

    void Start() {
        SetCanvas(UIState.BaseCanvas);
        SetCanvas(UIState.BaseCanvas);
        pauseMenu.enabled = false;
        inventory.enabled = false;
        builderCanvas.enabled = false;
        baseCanvas.enabled = true;
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
            case UIState.Builder:
            return (UIState.Inventory);
            default:
            return (UIState.BaseCanvas);
        }
    }

    //Changes Current Canvas
    public void SetCanvas(UIState newState) {
        switch(currentUI) {
            case UIState.BaseCanvas:
            break;
            case UIState.Inventory:
            break;
            case UIState.PauseMenu:
            break;
            case UIState.Builder:
            SetBuilderCanvas(newState);
            break;
            default:
            break;
        }

        currentUI = newState;
        CanvasAnimator.SetInteger("UIState", (int)currentUI);
    }

    void SetBaseCanvas() {

    }

    void SetBuilderCanvas(UIState State) {
        Builder.instance.StopBuild();
    }

    void SetInventoryCanvas() {

    }

    void SetpauseCanvas() {

    }
}
