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

    public bool walking;

    public Animator anim;

    private bool mouseDown;

    private bool usingNavmesh;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        if(anim == null)
        {
            Debug.LogError("Player animator is null");
        }
	}

    private void Update()
    {
        if (usingNavmesh)
        {
            if (transform.position == agent.destination)
            {
                usingNavmesh = false;
                anim.SetBool("Playerwalk", false);
            }
        }

        
       
    }

    // Update is called once per frame
    void FixedUpdate () {
        LookAtMouse();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle"))
        {
            Move();
            ClickMove();
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walking"))
        {
            Move();
            ClickMove();

        }
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

         
            transform.Translate(Vector3.right * x * moveSpeed * Time.deltaTime, Space.Self);
            transform.Translate(Vector3.forward * y * moveSpeed * Time.deltaTime, Space.Self);

            if (x != 0 || y != 0)
            {
                usingNavmesh = false;

                //if (!walking)
                //{
                //    walking = true;
                //    anim.SetTrigger("Player_Walk");
                //}

                if (!anim.GetBool("Playerwalk"))
                {
                    anim.SetBool("Playerwalk", true);
                }

                if(x == 1)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
                }
                else if(x == -1)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
                }
                else if (x > 0 && y > 0)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 30, transform.rotation.z);
                }
                else if (x < 0 && y < 0)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, -30, transform.rotation.z);
                }


                agent.updatePosition = false;
            }
            else
            {
                if (!usingNavmesh)
                {
                    anim.SetBool("Playerwalk", false);
                }
                //if (walking)
                //{
                //    walking = false;
                //    anim.SetTrigger("Player_Walk");

                //}
            }

            if(Input.GetKeyUp("w") || Input.GetKeyUp("a" )|| Input.GetKeyUp("s") || Input.GetKeyUp("d"))
            {
                anim.SetBool("Playerwalk", false);
            }
        }

            if(agent.nextPosition != transform.position)
            {
                agent.nextPosition = transform.position; //Updates the agents position to the objects position
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

                anim.SetBool("Playerwalk", true);

                agent.SetDestination(hit.point);
                usingNavmesh = true;
            }
        }

       


    }

}
