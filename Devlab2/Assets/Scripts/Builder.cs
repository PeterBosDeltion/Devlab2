using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {
    public static Builder instance;

    public Item CurrentlyBuilding;
    public Camera mainCamera;
    public LayerMask buildRayMask;
    public LayerMask buildCollisionMask;
    Mesh buildMesh;
    GameObject CurrentBuild;
    int buildRotation;

    private void Awake() {
        instance = this;
    }


    void Update() {
        if(CurrentlyBuilding != null) {
            RaycastHit rayHit;
            if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out rayHit, 10000, buildRayMask)) {
                DisplayBuild(new Vector3(buildRotation, 0, 0), rayHit.point);
                /*if(Physics.CheckBox(buildMesh.bounds.center, buildMesh.bounds.size, Quaternion.Euler(Vector3.zero), buildCollisionMask)) {
                    Debug.Log("na");

                }
                else {
                    Debug.Log("Yeath");
                }*/
            }
        }
    }

    public void StartBuilder(Item build) {
        CurrentlyBuilding = build;
        CurrentBuild = ObjectPooler.instance.GetFromPool(CurrentlyBuilding.itemName);
        buildMesh = CurrentBuild.GetComponent<MeshFilter>().mesh;
    }

    void DisplayBuild(Vector3 rotation, Vector3 position) {
        CurrentBuild.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
    }

    void PlaceBuild() {
        if(Physics.CheckBox(buildMesh.bounds.center, buildMesh.bounds.size, Quaternion.Euler(Vector2.zero), buildCollisionMask)) {

        }
        else {

        }
    }

    public void StopBuild() {
        buildRotation = 0;
        ObjectPooler.instance.AddToPool(CurrentlyBuilding.itemName, CurrentBuild);
        Inventory.Instance.AddItem(CurrentlyBuilding);
    }

    void OnDisable() {
        StopBuild();
    }
}
