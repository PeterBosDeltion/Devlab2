using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EcoManager))]
public class EcoManagerEditor : Editor {
#if UNITY_EDITOR
    public EcoManager ecoMan;

    void OnEnable() {
        ecoMan = (EcoManager)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Map")) {
            ecoMan.DestroyMap();
            ecoMan.GenerateMap();
            ecoMan.Serialize();
        }

        if (GUILayout.Button("Destroy Map")) {
            ecoMan.DestroyMap();
        }

        if (GUILayout.Button("Generate Island")) {
            ecoMan.GenorateIsland();
        }

    }
#endif
}