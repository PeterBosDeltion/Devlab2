using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EcoManager))]
public class EcoManagerEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Map")) {
            EcoManager.instance.GenerateMap();
        }
    }
}
