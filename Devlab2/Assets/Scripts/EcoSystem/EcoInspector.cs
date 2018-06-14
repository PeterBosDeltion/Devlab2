using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EcoInspector : MonoBehaviour {
    public static EcoInspector instance;

    public EcoManager.Tile currentlyInspected;
    public Sprite x;
    public Image tileImage1, tileImage2, tileImage3;
    public Image mainTile;
    public TextMeshProUGUI description;
    public TextMeshProUGUI reason;
    public TextMeshProUGUI tileName;
    public
    int currentlySelected;

    private void Awake() {
        instance = this;
    }

    public void ChangeInspectorUI(EcoManager.Tile newTile) {
        currentlyInspected = newTile;
        UpdateUI();
    }

    public void ButtonRight() {
        if(currentlyInspected.myTimeLine.Count - 1 != currentlySelected) {
            currentlySelected++;
            UpdateUI();
        }
    }

    public void ButtonLeft() {
        if(currentlySelected != 0) {
            currentlySelected--;
            UpdateUI();
        }
    }

    public void Update(){
        if(Input.GetButtonDown("Tile Manager") && UIManager.instance.currentUI == UIManager.UIState.BaseCanvas){
            UIManager.instance.tileInspector.SetBool("Inspector", !UIManager.instance.tileInspector.GetBool("Inspector"));
            UIManager.instance.tileInspector.SetBool("Interactor", false);
        }
    }

    void UpdateUI() {
        int i = (int)currentlyInspected.myTimeLine[currentlySelected].state - 1;

        tileImage2.sprite = EcoManager.groundSprites[i];
        mainTile.sprite = EcoManager.groundSprites[(int)currentlyInspected.myTimeLine[0].state - 1];
        description.text = EcoManager.groundDescription[i];

        reason.text = currentlyInspected.myTimeLine[currentlySelected].reason;
        tileName.text = EcoManager.groundName[i];

        if(currentlySelected == 0) {
            tileImage1.sprite = x;
        }
        else {
            tileImage3.sprite = EcoManager.groundSprites[(int)currentlyInspected.myTimeLine[currentlySelected - 1].state];
        }

        if(currentlySelected == currentlyInspected.myTimeLine.Count - 1) {
            tileImage3.sprite = x;
        }
        else {
            tileImage3.sprite = EcoManager.groundSprites[(int)currentlyInspected.myTimeLine[currentlySelected + 1].state];
        }
    }
}
