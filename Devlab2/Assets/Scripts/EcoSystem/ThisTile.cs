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
        
    }
}
