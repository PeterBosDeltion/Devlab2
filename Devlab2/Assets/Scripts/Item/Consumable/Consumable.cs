using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Consumable")]
public class Consumable : Item{
    public int food;
    public int water;
}
