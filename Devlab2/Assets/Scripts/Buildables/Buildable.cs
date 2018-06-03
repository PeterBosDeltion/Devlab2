using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour {
    public int myPollution;
    public Item myItem;

    void OnMouseDown() {
        if(Input.GetMouseButtonDown(2) && UIManager.instance.currentUI == UIManager.UIState.BaseCanvas) {
            Interact();
        }
    }

    public virtual void Interact() {
        if(InteractionManager.instance.currentInteracting == null) {
            InteractionManager.instance.StartInteraction(this);
        }
    }

    public virtual void StopInteraction() {

    }
}
