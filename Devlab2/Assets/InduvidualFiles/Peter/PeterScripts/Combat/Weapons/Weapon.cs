using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
   
    public Equippable equippable;
    public bool beingUsed;

    //public float durability; Do we want this?
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
      
	}

   

    public abstract void Use();
}
