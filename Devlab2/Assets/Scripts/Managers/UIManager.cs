using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public UIState currentUI = UIState.BaseCanvas;
    public enum UIState {
        BaseCanvas = 0,
        PauseMenu = 1,
        Inventory = 2,
    }

    public Canvas pauseMenu, inventory, baseCanvas;

    [Header("Animation")]
    public Animator CanvasAnimator;
    public Animator tileInspector;

    void Awake() {
        instance = this;
    }

    void Start() {
        pauseMenu.enabled = false;
        inventory.enabled = false;
        baseCanvas.enabled = true;
        SetCanvas(UIState.BaseCanvas);
    }

    public Animator endGameAnimator;
    public TextMeshProUGUI myText;
    public void EndGame() {
        endGameAnimator.SetTrigger("EndGame");
        Time.timeScale = 0;
        myText.text =  "Days Survived:" + EcoManager.instance.daysSurvived.ToString();
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            SetCanvas(CancelCanvas());
        }
        if (Input.GetButtonDown("Inventory")) {
            if (currentUI == UIState.Inventory) {
                SetCanvas(CancelCanvas());
            } else {
                SetCanvas(UIState.Inventory);
            }
        }
    }

    //Steps Back A Canvas When Cancel Button Is Pressed
    UIState CancelCanvas() {
        switch (currentUI) {
            case UIState.BaseCanvas:
                return (UIState.PauseMenu);
            default:
                return (UIState.BaseCanvas);
        }
    }

    //Changes Current Canvas
    public void SetCanvas(UIState newState) {
        switch (newState) {
            case UIState.BaseCanvas:
                tileInspector.SetBool("Interactor", false);
                break;
            case UIState.Inventory:
                tileInspector.SetBool("Inspector", false);
                break;
            case UIState.PauseMenu:
                tileInspector.SetBool("Interactor", false);
                tileInspector.SetBool("Inspector", false);
                break;
            default:
                break;
        }

        currentUI = newState;
        CanvasAnimator.SetInteger("UIState", (int)currentUI < 0 ? 0 : (int)currentUI);
    }
}