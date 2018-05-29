using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour {
    public GameObject player;
    public float speed = 3;
    public float rotSpeed = 30;

    public float zOffset = 5;

    public float rotateOffset;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        FollowPlayer();
        Rotate();
	}

    public void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - zOffset), speed * Time.deltaTime);
    }

    public void Rotate()
    {
        Vector3 mPos = Input.mousePosition;
        if (Input.GetMouseButton(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0, Space.World);

        }
        if (Input.GetMouseButtonUp(2))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        transform.Rotate(0, Input.GetAxis("RotateCam") * rotSpeed * Time.deltaTime, 0, Space.World);
    }
}
