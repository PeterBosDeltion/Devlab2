using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equippable", menuName = "Items/Equippable")]
public class Equippable : Item {
    public int damage;

    public enum CanGather {
        WoodGather,
        StonesGather,
        LandGather,
        HandGather
    }

    public CanGather myGathering = CanGather.HandGather;
}
