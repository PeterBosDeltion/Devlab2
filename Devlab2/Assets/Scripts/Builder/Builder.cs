﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {
    public static Builder instance;

    static Item CurrentlyBuilding;
    public Camera mainCamera;
    public LayerMask buildRayMask;
    public LayerMask buildCollisionMask;
    public BoxCollider displayCollider;
    public Material displayMaterial;
    static GameObject CurrentBuild;
    public float scrollweelSpeed;
    static int buildRotation;

    private void Awake() {
        instance = this;
    }


    public void CheckBuilderDisplay() {
        if(CurrentlyBuilding != null) {
            RaycastHit rayHit;
            if(Physics.Raycast(instance.mainCamera.ScreenPointToRay(Input.mousePosition), out rayHit, 10000, instance.buildRayMask)) {
                buildRotation += Input.GetButtonDown("Fire3") == true ? 90 : 0;
                buildRotation += Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheell") * Time.deltaTime * instance.scrollweelSpeed);

                DisplayBuild(new Vector3(0, buildRotation, 0), rayHit.point);
                if(Physics.CheckBox(displayCollider.bounds.center, displayCollider.bounds.size / 2, CurrentBuild.transform.rotation, instance.buildCollisionMask)) {
                    displayMaterial.color = Color.red;
                }
                else {
                    displayMaterial.color = Color.green;
                }
            }
        }
    }

    public void StartBuilder(Item build) {
        CurrentlyBuilding = build;
        CurrentBuild = ObjectPooler.instance.GetFromPool(CurrentlyBuilding.itemName + " Display");
        CheckBuilderDisplay();
    }

    static void DisplayBuild(Vector3 rotation, Vector3 position) {
        CurrentBuild.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
    }

    public void PlaceBuild() {
        if(CurrentBuild != null && !Physics.CheckBox(displayCollider.bounds.center, displayCollider.bounds.size / 2, CurrentBuild.transform.rotation, instance.buildCollisionMask)) {
            ObjectPooler.instance.GetFromPool(CurrentlyBuilding.itemName, CurrentBuild.transform.position, CurrentBuild.transform.rotation);
            ObjectPooler.instance.AddToPool(CurrentlyBuilding.itemName + " Display", CurrentBuild);

            buildRotation = 0;
            CurrentlyBuilding = null;
            UIManager.instance.SetCanvas(UIManager.UIState.BaseCanvas);
        }
    }

    public void CancelBuild() {
        StopBuild();
        UIManager.instance.SetCanvas(UIManager.UIState.Inventory);
    }

    public void StopBuild() {
        buildRotation = 0;
        if(CurrentlyBuilding != null) {
            ObjectPooler.instance.AddToPool(CurrentlyBuilding.itemName + " Display", CurrentBuild);
            Inventory.Instance.AddItem(CurrentlyBuilding);
            CurrentlyBuilding = null;
            UIManager.instance.SetCanvas(UIManager.UIState.BaseCanvas);

        }
    }
}