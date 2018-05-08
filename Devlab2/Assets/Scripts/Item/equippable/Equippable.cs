using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Equippable")]
public class Equippable : Item {
    public int protection;
    public int heatBonus;
}
