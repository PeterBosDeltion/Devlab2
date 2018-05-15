using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : Animal {
    public float idleTime;
    
	// Use this for initialization
	void Start () {
        state = States.Idle;
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if(state == States.Idle)
        {
            if (!idling)
            {
                StartCoroutine(Idle());
            }
        }
        else if(state == States.Wander)
        {
            Wander();
        }
        else if(state == States.Attack)
        {
            Attack();
        }
	}

    public override void Move()
    {
        /////////////////////////////////
    }

    public void Attack()
    {

    }

    private IEnumerator Idle()
    {
        idling = true;
        yield return new WaitForSeconds(idleTime);
        idling = false;
        
        state = States.Wander;

    }
}
