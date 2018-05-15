using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deer : Animal {

    public float idleTime = 1;
    public float runAwayRange = 5;

    private bool running;
    private GameObject player;
    private float startSpeed;
	// Use this for initialization
	void Start () {
        state = States.Idle;
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");
        startSpeed = agent.speed;

    }

    // Update is called once per frame
    private void Update()
    {
        if(state == States.Idle)
        {
            if (!idling)
            {
                StartCoroutine(Idle());
            }
        }
        else if(state == States.Wander)
        {
           if(agent.speed > startSpeed)
            {
                agent.speed = startSpeed;
            }
            Wander();

           
        }
        else if(state == States.Run)
        {
            running = true;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= runAwayRange)
        {
            state = States.Run;
        }
        else
        {
            state = States.Wander;
            running = false;
        }
    }

    private IEnumerator Idle()
    {
        idling = true;
        yield return new WaitForSeconds(idleTime);
        state = States.Wander;
        idling = false;
    }

    void FixedUpdate () {
        if (running)
        {
            Move();
        }
	}

    public override void Move()
    {
        Vector3 dir = transform.position - player.transform.position;

        if(agent.speed < startSpeed * 5)
        {
            agent.speed *= 5;
        }

        agent.SetDestination(transform.position + dir);

    }
}
