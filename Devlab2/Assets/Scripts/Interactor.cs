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
}