using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : Animal {

    public float idleTime = 2;
   
    private bool canLayEgg = true;

	// Use this for initialization
	void Start () {
        state = States.Idle;
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Egg?" + canLayEgg);

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
        Debug.Log("Idle");
        yield return new WaitForSeconds(idleTime);

        LayEgg();

        state = States.Wander;

    }

    private void LayEgg()
    {
        float chance = Random.value;

        if(chance > .99F) //0.7F = 30% chance
        {
            if (canLayEgg)
            {
                Debug.Log(transform.name + "Laid an egg");
                canLayEgg = false;
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
