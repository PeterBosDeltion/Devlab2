using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : Animal {

    public float idleTime = 2;
    public float patience = 9; //How long it takes if agent can't find path to generate a new one
    private bool idling;
    private bool canWander;
    private bool wandering;
    private bool hasTarget;
    private bool canLayEgg = true;

    private NavMeshAgent agent;
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
            canWander = true;
        }
        else if(state != States.Wander && canWander)
        {
            canWander = false;
        }
	}

    private void FixedUpdate()
    {
        if (canWander)
        {
            Move();
        }

    }

    public override void Move()
    {
        if (!hasTarget)
        {
            float x = Random.Range(-wanderRadius, wanderRadius);
            float z = Random.Range(-wanderRadius, wanderRadius);

            Vector3 target = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            agent.SetDestination(target);
            hasTarget = true;
            if (!wandering)
            {
                wandering = true;
                StartCoroutine(TimeBeforeNewTarget());
            }
        }
        else
        {
            if(transform.position == agent.destination)
            {
                state = States.Idle;
            }
        }
      
    }

    private IEnumerator TimeBeforeNewTarget()
    {
        yield return new WaitForSeconds(patience);
        hasTarget = false;
        wandering = false;
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);
        LayEgg();

        state = States.Wander;

    }

    private void LayEgg()
    {
        float chance = Random.value;

        if(chance > .7F) //0.7F = 30% chance
        {
            if (canLayEgg)
            {
                Debug.Log(transform.name + "Laid an egg");
                canLayEgg = false;
                //Egg logic here
            }
        }
    }

}
