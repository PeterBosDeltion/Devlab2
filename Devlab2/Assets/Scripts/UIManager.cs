using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public UIManager instance;

    void Awake() {
        instance = this;
    }
}
