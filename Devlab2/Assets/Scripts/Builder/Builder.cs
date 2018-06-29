using System.Collections;
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

    private Player player;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    void Update() {

        if(CurrentlyBuilding != null) {

            if(CurrentlyBuilding.itemType == Item.TypeOffItem.Crop)
            {
                buildRayMask = 16384; //Farmland layer
            }
            if (CurrentlyBuilding.furniture)
            {
                buildRayMask = 131072; //Building layer
            }

            RaycastHit rayHit;
            if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out rayHit, 10000, buildRayMask)) {
                buildRotation += Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheell") * Time.deltaTime * scrollweelSpeed);

                DisplayBuild(new Vector3(0, buildRotation, 0), new Vector3(rayHit.point.x,0, rayHit.point.z));

                if(Physics.CheckBox(displayCollider.bounds.center, displayCollider.bounds.size / 2, CurrentBuild.transform.rotation, instance.buildCollisionMask)) {
                    displayMaterial.color = Color.red + new Color(0, 0, 0, -0.5f);
                }
                else {
                    displayMaterial.color = Color.blue + new Color(0, 0, 0, -0.5f);
                    if(Input.GetButtonDown("Fire1")) {
                        PlaceBuild();
                    }
                }
            }
        }
    }

    public void StartBuilder(Item build) {
        CurrentlyBuilding = build;
        CurrentBuild = ObjectPooler.instance.GetFromPool(CurrentlyBuilding.itemName + " Display");
    }

    static void DisplayBuild(Vector3 rotation, Vector3 position) {
        CurrentBuild.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
    }

    public void PlaceBuild() {
        if(CurrentBuild != null && !Physics.CheckBox(displayCollider.bounds.center, displayCollider.bounds.size / 2, CurrentBuild.transform.rotation, instance.buildCollisionMask)) {
            ObjectPooler.instance.GetFromPool(CurrentlyBuilding.itemName, CurrentBuild.transform.position, CurrentBuild.transform.rotation);
            ObjectPooler.instance.AddToPool(CurrentlyBuilding.itemName + " Display", CurrentBuild);

            CurrentlyBuilding = null;
            Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].RemoveItem();
            StopBuild();
            player.ChangeEquippedItem(Inventory.Instance.SelectedToolbarSlot);

        }
    }


    public void StopBuild() {
        buildRotation = 0;
        if(CurrentlyBuilding != null) {
            ObjectPooler.instance.AddToPool(CurrentlyBuilding.itemName + " Display", CurrentBuild);
            CurrentlyBuilding = null;

            if(buildRayMask != 1024) //1024 = regular ground layer index
            {
                buildRayMask = 1024;
            }

        }
    }
}
