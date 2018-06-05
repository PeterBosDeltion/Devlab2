using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Interact intr = other.GetComponent<Interact>();
            StartInteract(intr);
        }
    }

    public void StartInteract(Interact intr)
    {
        Interact.currentlyInteracting = this;
        intr.craftingStationCanvas.enabled = true;
        //xxxtra-logic.com right here

    }
}
