using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : Buildable {
    public GameObject firePartical;

    public override void Interact() {
        base.Interact();

        firePartical.SetActive(true);
    }

    public override void StopInteraction() {
        base.StopInteraction();

        firePartical.SetActive(false);
    }
}
