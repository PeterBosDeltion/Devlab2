using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBuild : MonoBehaviour {
    public Material myMaterial;
    public Mesh myMesh;

    private void OnEnable() {
        Builder.displayMesh = myMesh;
        Builder.displayMaterial = myMaterial;
    }

    private void OnMouseDrag() {
        Builder.CheckBuilderDisplay();
    }
}
