using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour {
    public GameObject player;
    public float speed = 3;

    public float zOffset = 5;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        FollowPlayer();
	}

    public void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - zOffset), speed * Time.deltaTime);
    }
}
