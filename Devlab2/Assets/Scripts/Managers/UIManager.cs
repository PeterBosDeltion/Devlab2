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
    }

    public Canvas pauseMenu, inventory, baseCanvas;

    [Header("Animation")]
    public Animator CanvasAnimator;

    void Awake() {
        instance = this;
    }

    void Start() {
        pauseMenu.enabled = false;
        inventory.enabled = false;
        baseCanvas.enabled = true;
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
    public void SetCanvas(UIState newState) {
        switch(currentUI) {
            case UIState.BaseCanvas:
            break;
            case UIState.Inventory:
            break;
            case UIState.PauseMenu:
            break;
            default:
            break;
        }

        currentUI = newState;
        CanvasAnimator.SetInteger("UIState", (int)currentUI);
    }
}
