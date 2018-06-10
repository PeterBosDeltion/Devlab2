using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMachine : Machine {
    public string StartText;
    public string stopText;

    public List<Resipe> validItems = new List<Resipe>();
    public List<Resipe> validFuelItems = new List<Resipe>();

    public List<Item> craftSlots = new List<Item>();
    public List<Item> productSlots = new List<Item>();
    public List<Item> craftFuelSlots = new List<Item>();
    public List<Item> productFuelSlots = new List<Item>();
    public Item burnSlot;
    public Resipe burnSlotResipe;
    public int coolDownTime;
    public bool needsFuel;

    public void TurnOn() {
        isTurnedOn = true;
        CheckResipe();
    }

    public void TurnOff() {
        isTurnedOn = false;
    }

    public void CheckResipe() {
        if (needsFuel == true && CheckFuelResipe()== false) {
            if (isTurnedOn == true) {
                TurnOff();
            }
            return;
        } else if (isTurnedOn == false) {
            return;
        }

        for (int i = 0; i < craftSlots.Count; i++) {
            for (int ii = 0; ii < validItems.Count; ii++) {
                if (craftSlots[i] != null && craftSlots[i].itemName == validItems[ii].ingredient.itemName) {
                    for (int x = 0; x < productSlots.Count; x++) {
                        if (productSlots[x] == null) {
                            productSlots[x] = Instantiate(validItems[ii].product);
                            MachineSlot r = Interactor.instance.GetMachineSlot(false, i);
                            r.RemoveItem();
                            craftSlots[i] = null;

                            MachineSlot m = Interactor.instance.GetMachineSlot(false, x);
                            StartCoroutine(Processing(validItems[ii], m));

                            if (Interactor.instance.myCraftingMachine == this) {
                                m.SetItem(Instantiate(validItems[ii].product));
                                Interactor.instance.craftSlots[i].RemoveItem();
                            }
                            break;
                        } else if (x == productSlots.Count - 1) {
                            return;
                        }
                    }
                }
            }
        }
    }

    bool CheckFuelResipe() {
        if (burnSlot != null) {
            return true;
        }
        for (int i = 0; i < craftFuelSlots.Count; i++) {
            for (int ii = 0; ii < validFuelItems.Count; ii++) {
                if (craftFuelSlots[i] != null && craftFuelSlots[i].itemName == validFuelItems[ii].ingredient.itemName) {
                    for (int z = 0; z < productFuelSlots.Count; z++) {
                        if (productFuelSlots[z] == null && burnSlot == null) {
                            if (burnSlot == null) {
                                burnSlot = Instantiate(craftFuelSlots[i]);
                                craftFuelSlots[i] = null;
                                burnSlotResipe = validFuelItems[ii];
                                MachineSlot m = Interactor.instance.burnSlot;
                                StartCoroutine(BurnProcessing(burnSlotResipe, m));
                                if (Interactor.instance.myCraftingMachine == this) {
                                    m.SetItem(Instantiate(validFuelItems[ii].ingredient));
                                    Interactor.instance.craftFuelSlots[i].RemoveItem();
                                }
                                return true;
                            }
                        }
                    }

                }
            }
        }
        return false;
    }

    [System.Serializable]
    public struct Resipe {
        public Item product;
        public Item ingredient;
        public int timeToConvert;
    }

    public IEnumerator Processing(Resipe myResipe, MachineSlot mySlot) {
        float ttimer = myResipe.timeToConvert;
        mySlot.fillImage.fillAmount = 1;

        while (mySlot.fillImage.fillAmount >= 0.01f) {
            if (isTurnedOn == true) {
                ttimer -= Time.deltaTime;
            } else {
                ttimer = myResipe.timeToConvert;
                mySlot.fillImage.fillAmount = 1;
            }
            if (Interactor.instance.myCraftingMachine == this) {
                if (ttimer <= 0) {
                    mySlot.fillImage.fillAmount = 0;
                } else {
                    mySlot.fillImage.fillAmount = ttimer / myResipe.timeToConvert;
                }

            }
            yield return null;
        }
    }

    public IEnumerator Processing(MachineSlot mySlot) {
        float ttimer = coolDownTime;
        mySlot.fillImage.fillAmount = 1;

        while (mySlot.fillImage.fillAmount >= 0.01f) {
            if (isTurnedOn == false) {
                ttimer -= Time.deltaTime;
            } else {
                ttimer = coolDownTime;
                mySlot.fillImage.fillAmount = 1;
            }
            if (Interactor.instance.myCraftingMachine == this) {
                if (ttimer <= 0) {
                    mySlot.fillImage.fillAmount = 0;
                } else {
                    mySlot.fillImage.fillAmount = ttimer / coolDownTime;
                }

            }
            yield return null;
        }
    }

    public IEnumerator BurnProcessing(Resipe myResipe, MachineSlot mySlot) {
        float ttimer = myResipe.timeToConvert;
        mySlot.fillImage.fillAmount = 1;

        while (mySlot.fillImage.fillAmount >= 0.01f) {
            if (isTurnedOn == true) {
                ttimer -= Time.deltaTime;
                if (Interactor.instance.myCraftingMachine == this) {
                    if (ttimer <= 0.01f) {
                        mySlot.fillImage.fillAmount = 0;
                    } else {
                        mySlot.fillImage.fillAmount = ttimer / myResipe.timeToConvert;
                    }
                }
            }
            yield return null;
        }

        for (int x = 0; x < productFuelSlots.Count; x++) {
            if (productFuelSlots[x] == null) {
                MachineSlot m = Interactor.instance.GetMachineSlot(true, x);
                productFuelSlots[x] = Instantiate(burnSlotResipe.product);
                m.SetItem(burnSlotResipe.product);
                StartCoroutine(Processing(m));
                Interactor.instance.burnSlot.RemoveItem();
                burnSlot = null;
                mySlot.fillImage.fillAmount = 1;
                break;
            } else if (x == productFuelSlots.Count) {
                TurnOff();
            }
        }
        CheckResipe();
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Player" && Input.GetButtonDown("Interact")) {
            Interactor.instance.ChangeInteractor(this);
        }
    }
}