using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPolution : MonoBehaviour {
    public int pollution;

    void Start() {
        EcoManager.instance.AddPollution(pollution);
    }

    void OnEnable() {
        if(EcoManager.instance != null) {
            EcoManager.instance.AddPollution(pollution);
        }
    }

    void OnDisable() {
        EcoManager.instance.AddPollution(-pollution);
    }


    //          *** add Polution
}
