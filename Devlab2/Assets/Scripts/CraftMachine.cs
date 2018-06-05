using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMachine : Machine {
    public List<MachineSlot> craftSlots = new List<MachineSlot>(); 
    public List<MachineSlot> productSlots = new List<MachineSlot>();
    public List<Item> validItems = new List<Item>();

    bool isTurnedOn;

    Item processing;

    public float timeToConvert;
    float currentConvertTime;

    public void CheckResipe(){
        if(isTurnedOn == false) {
            return;
        }

        for(int i = 0; i < craftSlots.Count; i++) {
            if(craftSlots[i].myItem != null) {
                for(int ii = 0; ii < validItems.Count; ii++) {
                    if(craftSlots[i].myItem == validItems[ii]){
                        for(int x = 0; x < productSlots.Count; x++) {
                            if(productSlots[x].myItem == null) {
                                productSlots[x].Processing();
                                productSlots[x].SetItem(craftSlots[i].myItem);
                                StartCoroutine(productSlots[x].Processing());
                                break;
                            }
                            else if(x == productSlots.Count - 1){
                                return;
                            }
                        }
                        processing = craftSlots[i].myItem;
                        craftSlots[i].RemoveItem();
                        return;
                    }
                }
            }
        }
    }

    public void TurnOn(){
        CheckResipe();
    }
}
