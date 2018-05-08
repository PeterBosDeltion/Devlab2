using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameManager instance;

    void Awake() {
        instance = this;
    }
}
