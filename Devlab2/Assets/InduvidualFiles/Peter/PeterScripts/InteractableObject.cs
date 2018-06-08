using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            Interact intr = other.GetComponent<Interact>();
            StartInteract(intr);
        }
    }

    public void StartInteract(Interact intr)
    {
        Interact.currentlyInteracting = this;
    }
}
