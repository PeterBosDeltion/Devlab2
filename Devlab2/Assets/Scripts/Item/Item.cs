using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject {
    public string itemName;
    public Sprite item2D;
    public bool placaBle;

    public List<string> myButtons = new List<string>();
    public string itemDiscription;

    public TypeOffItem itemType;
    public enum TypeOffItem{
        None,
        Helmet,
        ChestPiece,
        LegPeice,
        FootWear,
        Weapon
    }
}
