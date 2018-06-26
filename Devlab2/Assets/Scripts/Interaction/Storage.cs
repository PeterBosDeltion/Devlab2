using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Machine {
    public Item storageItem;
    public List<Item> Inventory = new List<Item>();
    public List<GameObject> itemGameObjects = new List<GameObject>();

    void OnTriggerStay(Collider other) {
        if (other.tag == "Player" && Input.GetButtonDown("Interact")) {
            Interactor.instance.ChangeInteractor(this);
            UIManager.instance.SetCanvas(UIManager.UIState.Inventory);
            UIManager.instance.tileInspector.SetBool("Inspector", false);
            UIManager.instance.tileInspector.SetBool("Interactor", true);
        }
    }
}