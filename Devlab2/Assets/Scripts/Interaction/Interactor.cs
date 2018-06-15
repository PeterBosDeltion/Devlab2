using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour {
    public static Interactor instance;

    public Machine myMachine;
    public CraftMachine myCraftingMachine;
    public TextMeshProUGUI machineName;
    public Image machineImage;

    [Header("Basic")]
    public Image basicMachineUseImage;
    public GameObject Basic;

    #region Crafter
    [Header("Crafter")]
    public List<MachineSlot> craftSlots = new List<MachineSlot>();
    public List<MachineSlot> productSlots = new List<MachineSlot>();
    public List<MachineSlot> craftFuelSlots = new List<MachineSlot>();
    public List<MachineSlot> productFuelSlots = new List<MachineSlot>();
    public MachineSlot burnSlot;
    public Image crafterMachineUseImage;
    public GameObject crafter;
    public TextMeshProUGUI startText;
    public GameObject fuelForCrafting;
    #endregion

    private void Awake() {
        instance = this;

        for (int i = 0; i < craftSlots.Count; i++) {
            craftSlots[i].myIndex = i;
        }
        for (int i = 0; i < craftFuelSlots.Count; i++) {
            craftFuelSlots[i].myIndex = i;
        }
        for (int i = 0; i < productSlots.Count; i++) {
            productSlots[i].myIndex = i;
        }
        for (int i = 0; i < productFuelSlots.Count; i++) {
            productFuelSlots[i].myIndex = i;
        }
    }

    public void ChangeInteractor(Machine newMachine) {
        myMachine = newMachine;

        machineImage.sprite = myMachine.machineImage;
        machineName.text = myMachine.machineName;

        Basic.SetActive(false);
        crafter.SetActive(false);

        if (myMachine.GetType()== typeof(CraftMachine)) {
            myCraftingMachine = ((CraftMachine)newMachine);
            crafter.SetActive(true);
            crafterMachineUseImage.sprite = myMachine.machineImage;
            startText.text = myCraftingMachine.StartText;

            for (int i = 0; i < craftSlots.Count; i++) {
                if (myCraftingMachine.craftSlots[craftSlots[i].myIndex] != null) {
                    craftSlots[i].SetItem(myCraftingMachine.craftSlots[craftSlots[i].myIndex]);
                }
            }

            for (int i = 0; i < productSlots.Count; i++) {
                if (myCraftingMachine.productSlots[productSlots[i].myIndex] != null) {
                    productSlots[i].SetItem(myCraftingMachine.productSlots[productSlots[i].myIndex]);
                }
            }

            if (myCraftingMachine.needsFuel == true) {
                for (int i = 0; i < craftFuelSlots.Count; i++) {
                    if (myCraftingMachine.craftFuelSlots[craftFuelSlots[i].myIndex] != null) {
                        craftFuelSlots[i].SetItem(myCraftingMachine.craftFuelSlots[craftFuelSlots[i].myIndex]);
                    }
                }

                burnSlot.SetItem(myCraftingMachine.burnSlot);
                burnSlot.fillImage.fillAmount = 1;

                for (int i = 0; i < productFuelSlots.Count; i++) {
                    if (myCraftingMachine.productFuelSlots[productFuelSlots[i].myIndex] != null) {
                        productFuelSlots[i].SetItem(myCraftingMachine.productFuelSlots[productFuelSlots[i].myIndex]);
                        productFuelSlots[i].fillImage.fillAmount = 1;
                    }
                }
                fuelForCrafting.SetActive(true);
            } else {
                fuelForCrafting.SetActive(false);
            }
        } else {
            Basic.SetActive(true);
            basicMachineUseImage.sprite = myMachine.machineImage;
        }
    }

    public MachineSlot GetMachineSlot(bool isFuel, int myIndex) {
        if (isFuel == true) {
            return productFuelSlots[myIndex];
        } else {
            return productSlots[myIndex];
        }
    }

    public void SwitchOnOff() {
        if (myCraftingMachine != null) {
            if (myCraftingMachine.isTurnedOn == true) {
                myCraftingMachine.TurnOff();
                startText.text = myCraftingMachine.StartText;
            } else {
                myCraftingMachine.TurnOn();
                startText.text = myCraftingMachine.stopText;
            }
        }
    }
}