using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBuild : MonoBehaviour {
    public BoxCollider myCollider;

    void OnEnable() {
        Builder.instance.displayCollider = myCollider;
    }
}
