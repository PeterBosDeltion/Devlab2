﻿using System.Collections;
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
    public GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        agent = player.GetComponent<NavMeshAgent>();

        if(anim == null)
        {
            Debug.LogError("Player animator is null");
        }
	}

    private void Update()
    {
        if (usingNavmesh)
        {
            if (player.transform.position == agent.destination)
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


            player.transform.Translate(transform.right * x * moveSpeed * Time.deltaTime, Space.World);
            player.transform.Translate(transform.forward * y * moveSpeed * Time.deltaTime, Space.World);

       


            if (x != 0 || y != 0)
            {
                usingNavmesh = false;
                Vector3 movement = new Vector3(x, 0.0f, y);

                player.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(player.transform.rotation.eulerAngles, movement, rotateSpeed * Time.deltaTime));
                //if (!walking)
                //{
                //    walking = true;
                //    anim.SetTrigger("Player_Walk");
                //}

                if (!anim.GetBool("Playerwalk"))
                {
                    anim.SetBool("Playerwalk", true);
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
            Vector3 lookPos = hit.point - player.transform.position;

            if(Vector3.Distance(player.transform.position, hit.point) > 1.8F)
            {
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, Time.deltaTime * rotateSpeed);

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
