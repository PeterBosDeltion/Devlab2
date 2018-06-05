using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionManager : MonoBehaviour {
    public static InteractionManager instance;

    public Buildable currentInteracting;

    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void StartInteraction(Buildable toInteract) {
        currentInteracting = toInteract;
    }

    public void StopInteraction() {
        currentInteracting = null;
        UIManager.instance.SetCanvas(UIManager.UIState.BaseCanvas);
    }
    #region Interaction UI
    public Image machineImage;
    public TextMeshProUGUI machineName;

    void SetInteractionUI() {
        machineImage.sprite = currentInteracting.myItem.item2D;
        machineName.text = currentInteracting.myItem.itemName;


    }
    #endregion
}
