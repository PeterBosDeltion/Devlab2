using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject {
    public string itemName;
    public Sprite item2D;
    public int itemListIndex; //If item should not be holdable, give hand index(Found in the items list of the Player script)
    public bool placaBle;
    public float placeAbleYoffset;
    public bool furniture;

    public List<string> myButtons = new List<string>();
    public string itemDiscription;

    public Equippable equippable;

    public TypeOffItem itemType;
    public enum TypeOffItem{
        None,
        Helmet,
        ChestPiece,
        LegPeice,
        FootWear,
        Weapon,
        Crop
    }
}
