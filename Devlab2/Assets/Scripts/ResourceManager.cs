using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public ResourceManager instance;

    void Awake() {
        instance = this;
    }
}
