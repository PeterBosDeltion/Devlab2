using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject {
    public string itemName;
    public Sprite item2D;
    public string itemDiscription;

    public List<string> myButtons = new List<string>();

    public int amount;
    public int amountCap;
}
