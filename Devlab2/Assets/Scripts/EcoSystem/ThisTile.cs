using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThisTile : MonoBehaviour {
    public GameObject waterObstacle;
    public GameObject grass;
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

        if (newState == EcoManager.GroundState.Water) {
            waterObstacle.SetActive(true);
        } else {
            if (newState == EcoManager.GroundState.burned) {
                burn();
            }

            if (currentState == EcoManager.GroundState.Grass || currentState == EcoManager.GroundState.fertile) {
                grass.SetActive(true);
            } else {
                grass.SetActive(false);
            }
            waterObstacle.SetActive(false);
        }
    }

    void Start() {
        if (gameObject.layer == LayerMask.NameToLayer("Water")) {
            waterObstacle.SetActive(true);
        } else {
            waterObstacle.SetActive(false);
        }
    }

    void OnMouseDown() {
        if (Input.GetMouseButtonDown(0)&& Input.GetKey(KeyCode.LeftShift)) {
            Inspect();
        }
    }

    void Inspect() {
        EcoInspector.instance.ChangeInspectorUI(EcoManager.instance.Grid[gridPosX].myArray[gridPosY]);
    }

    public void burn() {
        if (myOccupant != null && myOccupant.canBurn == true) {
            StartCoroutine(myOccupant.Burning());
        }
    }
}