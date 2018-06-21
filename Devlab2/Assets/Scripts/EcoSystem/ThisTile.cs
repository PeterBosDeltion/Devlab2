using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThisTile : MonoBehaviour {
    public NavMeshObstacle myObstavle;
    public List<EcoManager.State> myTimeLine = new List<EcoManager.State>();
    public EcoManager.GroundState currentState;
    public int gridPosX, gridPosY;
    public GameObject myTile;
    [SerializeField] Renderer myRenderer;
    public AddPolution myOccupant;

    public void SetThis(EcoManager.GroundState newState, int gridX, int gridY) {
        ChangeMaterial(newState, "How Did This Get Here.");
        gridPosX = gridX;
        gridPosY = gridY;
    }

    public void ChangeMaterial(EcoManager.GroundState newState, string newReason) {
        EcoManager.State s = new EcoManager.State();
        s.state = newState;
        s.reason = newReason;

        myTimeLine.Add(s);
        currentState = newState;
        if (myRenderer == null) {
            myRenderer = myTile.GetComponent<Renderer>();
        }
        myRenderer.material = EcoManager.groundTextures[(int)newState - 1];
    }

    private void Start() {
        if (gameObject.layer == LayerMask.NameToLayer("Water")) {
            myObstavle.enabled = true;
            foreach (Transform t in transform) {
                if (t.name == "WaterCollider") {
                    t.transform.gameObject.SetActive(true);
                }
            }
        } else {
            foreach (Transform t in transform) {
                if (t.name == "WaterCollider") {
                    t.transform.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnMouseDown() {
        if (Input.GetMouseButtonDown(0)&& (int)UIManager.instance.currentUI <= 0) {
            Inspect();
        }
    }

    void Inspect() {
        EcoInspector.instance.ChangeInspectorUI(EcoManager.instance.Grid[gridPosX].myArray[gridPosY]);
    }
}