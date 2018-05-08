using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject {
    public string itemName;
    public bool craftable;
    public Sprite item2D;
    public string item3D;

}
