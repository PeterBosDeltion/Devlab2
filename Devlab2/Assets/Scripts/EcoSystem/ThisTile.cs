using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThisTile : MonoBehaviour {
    public NavMeshObstacle myObstavle;

    void OnEnable() {
        if(gameObject.layer == LayerMask.NameToLayer("Water")) {
            myObstavle.enabled = true;
        }
    }

    void OnMouseDown(){
        if(Input.GetMouseButtonDown(1) && UIManager.instance.currentUI == UIManager.UIState.BaseCanvas) {
            Inspect();
        }
    }

    void Inspect() {
        UIManager.instance.SetCanvas(UIManager.UIState.TileInspector);
    }
}
