using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public float hp = 100;
    public float armor = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float damage)
    {
        hp -= (damage - armor);
        Debug.Log("HP: " + hp);
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
