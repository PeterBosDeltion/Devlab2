using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    public float moveSpeed;
    public float rotateSpeed = 16;
    private NavMeshAgent agent;
    public LayerMask moveLayermask;

    private bool mouseDown;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void FixedUpdate () {
        Move();
        LookAtMouse();
        ClickMove();
	}

    public void Move()
    {
        if (Input.GetMouseButton(1))
        {
            mouseDown = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            mouseDown = false;
        }


        if (!mouseDown)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            transform.Translate(transform.right * x * moveSpeed * Time.deltaTime, Space.World);
            transform.Translate(transform.forward * y * moveSpeed * Time.deltaTime, Space.World);

            if(agent.nextPosition != transform.position)
            {
                agent.nextPosition = transform.position; //Updates the agents position to the objects position
            }

            if (x != 0 || y != 0)
            {
                agent.updatePosition = false;
            }

        }






    }

    public void LookAtMouse()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 999,moveLayermask))
        {
            Vector3 lookPos = hit.point - transform.position;

            if(Vector3.Distance(transform.position, hit.point) > 1.8F)
            {
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

            }
        }
    }

    public void ClickMove()
    {

        RaycastHit hit;
        if (Input.GetMouseButton(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                agent.updatePosition = true;

                agent.SetDestination(hit.point);
            }
        }
    }

}
