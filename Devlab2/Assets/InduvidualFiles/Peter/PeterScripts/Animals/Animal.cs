using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public float patience = 9; //How long it takes if agent can't find path to generate a new one
    [HideInInspector]
    public bool idling;
    [HideInInspector]
    public bool canWander;
    [HideInInspector]
    public bool wandering;
    [HideInInspector]
    public bool hasTarget;
    [HideInInspector]
    public NavMeshAgent agent;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void Move();
  
    public virtual void Wander()
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
            if (transform.position == agent.destination.normalized)
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

        state = States.Idle;
    }


}
