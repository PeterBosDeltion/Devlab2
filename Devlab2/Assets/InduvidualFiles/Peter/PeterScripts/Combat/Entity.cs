using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public float hp = 100;
    public float armor = 0;

    public void TakeDamage(float damage) {
        hp -= (damage - armor);
        if(hp <= 0) {
            Die();
        }
    }

    public void Die() {
        if(hp <= 0) {
            Destroy(gameObject);
        }
    }
}
