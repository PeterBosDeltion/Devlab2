using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThisTile : MonoBehaviour {
    public NavMeshObstacle myObstavle;
    public int x, y;

    private void Start() {
        if(gameObject.layer == LayerMask.NameToLayer("Water")) {
            myObstavle.enabled = true;
        }
    }

    void OnMouseDown(){
        if(Input.GetMouseButtonDown(0) && (int)UIManager.instance.currentUI <= 0) {
            Inspect();
        }
    }

    void Inspect() {
        EcoInspector.instance.ChangeInspectorUI(EcoManager.instance.Grid[x,y]);
    }
}
