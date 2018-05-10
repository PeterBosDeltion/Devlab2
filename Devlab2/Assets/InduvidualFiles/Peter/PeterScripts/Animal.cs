using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour {
    public enum Types
    {
        Passive,
        Neutral,
        Aggresive
    }

    public enum States
    {
        Idle,
        Wander,
        Run,
        Attack
    }


    public Types type;
    public States state;

    public float wanderRadius;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void Move();
  


}
