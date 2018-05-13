﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : Slot {
    public Animator slotAnimator;

    public void InspectItem() {
        if(myItem != null) {
            Inventory.Instance.InspectItem(this);
        }
    }

    public override void MouseEnter() {
        slotAnimator.SetBool("Enter", true);
        base.MouseEnter();
    }

    public override void MouseExit() {
        slotAnimator.SetBool("Enter", false);
        base.MouseExit();
    }
}
