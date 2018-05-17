using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    public float moveSpeed;
    public float rotateSpeed = 16;
    private NavMeshAgent agent;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Move();
        LookAtMouse();
        //ClickMove();
	}

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if(x != 0 || y != 0)
        {
            agent.updatePosition = false;
        }

        transform.Translate(Vector3.left *- x * moveSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.forward * y * moveSpeed * Time.deltaTime, Space.World);
    }

    public void LookAtMouse()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 999))
        {
            Vector3 lookPos = hit.point - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
        }
    }

    public void ClickMove()
    {
       
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                agent.updatePosition = true;

                agent.SetDestination(hit.point);
            }
        }
    }

}
