using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {
    public string machineName;
    public Sprite machineImage;
    public bool isTurnedOn;

    public void StartInteraction(){
        Interactor.instance.ChangeInteractor(this);
    }
}
