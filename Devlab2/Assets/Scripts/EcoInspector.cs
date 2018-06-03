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
    public TextMeshProUGUI description;
    public TextMeshProUGUI reason;
    public TextMeshProUGUI tileName;
    int currentlySelected;

    private void Awake() {
        instance = this;
    }

    public void ChangeInspectorUI(EcoManager.Tile newTile) {
        currentlyInspected = newTile;
        UpdateUI();
    }

    public void ButtonRight() {
        if(currentlyInspected.myTimeLine.Count != currentlySelected) {
            currentlySelected++;
            UpdateUI();
        }
    }

    public void ButtonLeft() {
        if(currentlySelected != -1) {
            currentlySelected--;
            UpdateUI();
        }
    }

    void UpdateUI() {

        int i = (int)currentlyInspected.myTimeLine[currentlySelected].state - 1;
        Debug.Log(currentlyInspected.currentState);

        tileImage2.sprite = EcoManager.groundSprites[i];

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
