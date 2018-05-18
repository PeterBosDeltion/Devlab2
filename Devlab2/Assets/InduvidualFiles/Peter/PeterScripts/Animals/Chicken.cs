using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : Animal {

    public float idleTime = 2;
   
    private bool canLayEgg = true;
    public GameObject eggPrefab;

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
            state = States.Idle;
        }
	}

    private void FixedUpdate()
    {
        if (canWander)
        {
            Wander();
        }

    }

    public override void Move()
    {
      //Not neccesary here 
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

        if(chance > .999F) //0.7F = 30% chance
        {
            if (canLayEgg)
            {
                canLayEgg = false;
                Instantiate(eggPrefab, transform.position, Quaternion.identity);
                //Egg logic here
            }
            else
            {
                canLayEgg = true;
                return;
            }

        }
       
    }

}
