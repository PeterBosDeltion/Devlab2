using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBuild : MonoBehaviour {
    public Material myMaterial;
    public BoxCollider myCollider;

    void OnEnable() {
        Builder.instance.displayCollider = myCollider;
        Builder.instance.displayMaterial = myMaterial;
    }

    void OnMouseDrag() {
        Builder.instance.CheckBuilderDisplay();
    }
}
