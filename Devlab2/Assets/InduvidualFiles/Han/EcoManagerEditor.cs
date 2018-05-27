using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EcoManager))]
public class EcoManagerEditor : Editor {

    public EcoManager ecoMan;

    void OnEnable() {
        ecoMan = (EcoManager)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Map")) {
            ecoMan.GenerateMap();
        }

        if(GUILayout.Button("Destroy Map")) {
            ecoMan.DestroyMap();
        }
    }
}
