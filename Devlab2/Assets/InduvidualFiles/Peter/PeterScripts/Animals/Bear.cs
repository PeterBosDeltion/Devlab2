using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : Animal {
    public float idleTime;
    public float aggroRange;
    public float attackRange;
    private GameObject player;

    public float damage;
    public float attackCooldown;
    private bool cooling;

	// Use this for initialization
	void Start () {
        state = States.Idle;
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");
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
            if(Vector3.Distance(transform.position, player.transform.position) <= aggroRange)
            {
                state = States.Attack;
            }

        }
        else if(state == States.Attack)
        {
            Attack();
            if (Vector3.Distance(transform.position, player.transform.position) > aggroRange)
            {
                state = States.Wander;
            }
        }
	}

    public override void Move()
    {
        /////////////////////////////////
    }

    public void Attack()
    {
        agent.SetDestination(player.transform.position);
        if (!cooling)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {

                player.GetComponent<Entity>().TakeDamage(damage);
                StartCoroutine(Cool());
            }
        }
    }

    private IEnumerator Cool()
    {
        cooling = true;
        yield return new WaitForSeconds(attackCooldown);
        cooling = false;
    }

    private IEnumerator Idle()
    {
        idling = true;
        yield return new WaitForSeconds(idleTime);
        idling = false;
        
        state = States.Wander;

    }
}
