using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour {
    public static Interactor instance;

    public Machine myMachine;
    public CraftMachine myCraftingMachine;
    public Storage myStorage;
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

    [Header("Storage")]
    public List<StorageSlot> storage = new List<StorageSlot>();
    public GameObject storageUI;

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

        for (int i = 0; i < storage.Count; i++) {
            storage[i].mySlot = i;
        }
    }

    public void ChangeInteractor(Machine newMachine) {
        myMachine = newMachine;

        machineImage.sprite = myMachine.machineImage;
        machineName.text = myMachine.machineName;

        Basic.SetActive(false);
        crafter.SetActive(false);
        storageUI.SetActive(false);

        if (myMachine.GetType()== typeof(CraftMachine)) {
            myCraftingMachine = ((CraftMachine)newMachine);
            crafter.SetActive(true);
            crafterMachineUseImage.sprite = myMachine.machineImage;
            startText.text = myCraftingMachine.StartText;

            for (int i = 0; i < craftSlots.Count; i++) {
                if (myCraftingMachine.craftSlots[craftSlots[i].myIndex] != null) {
                    craftSlots[i].SetItem(myCraftingMachine.craftSlots[craftSlots[i].myIndex]);
                } else {
                    craftSlots[i].RemoveItem();
                }
            }

            for (int i = 0; i < productSlots.Count; i++) {

                if (myCraftingMachine.productSlots[productSlots[i].myIndex] != null) {
                    productSlots[i].SetItem(myCraftingMachine.productSlots[productSlots[i].myIndex]);
                    productSlots[i].fillImage.fillAmount = 1;
                } else {
                    productSlots[i].RemoveItem();
                    productSlots[i].fillImage.fillAmount = 0;
                }
            }

            if (myCraftingMachine.needsFuel == true) {
                for (int i = 0; i < craftFuelSlots.Count; i++) {
                    if (myCraftingMachine.craftFuelSlots[craftFuelSlots[i].myIndex] != null) {
                        craftFuelSlots[i].SetItem(myCraftingMachine.craftFuelSlots[craftFuelSlots[i].myIndex]);
                    } else {
                        craftFuelSlots[i].RemoveItem();
                    }
                }

                burnSlot.SetItem(myCraftingMachine.burnSlot);
                burnSlot.fillImage.fillAmount = 1;

                for (int i = 0; i < productFuelSlots.Count; i++) {
                    if (myCraftingMachine.productFuelSlots[productFuelSlots[i].myIndex] != null) {
                        productFuelSlots[i].SetItem(myCraftingMachine.productFuelSlots[productFuelSlots[i].myIndex]);
                        productFuelSlots[i].fillImage.fillAmount = 1;
                    } else {
                        productFuelSlots[i].RemoveItem();
                        productSlots[i].fillImage.fillAmount = 0;
                    }
                }
                fuelForCrafting.SetActive(true);
            } else {
                fuelForCrafting.SetActive(false);
            }
        } else if (myMachine.GetType()== typeof(Storage)) {
            myStorage = ((Storage)newMachine);
            UpdateStorage();
            storageUI.SetActive(true);
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

    public void UpdateStorage() {
        for (int i = 0; i < storage.Count; i++) {
            if (myStorage.Inventory[i] == null) {
                storage[i].RemoveItem();
            } else {
                storage[i].SetItem(myStorage.Inventory[i]);
            }
        }
    }

    public void setStorageItem(int newIndex) {
        if (myStorage != null) {
            myStorage.Inventory[newIndex] = storage[newIndex].myItem;
            if (storage[newIndex].myItem != null) {
                myStorage.itemGameObjects[newIndex].SetActive(true);
            } else {
                myStorage.itemGameObjects[newIndex].SetActive(false);
            }
        }
    }
}