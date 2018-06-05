using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour {

    public Animator anim;

    void Start ()
    {
		
	}
	

	void Update ()
    {
		
	}

    public void PlayerIdle()
    {
        anim.SetTrigger("Player_Idle");
    }

    public void PlayerWalk()
    {
        anim.SetTrigger("Player_Walk");
    }

    public void PlayerAxe()
    {
        anim.SetTrigger("Player_Axe");
    }

    public void PlayerSwing()
    {
        anim.SetTrigger("Player_Swing");
    }

    public void PlayerGrab()
    {
        anim.SetTrigger("Player_Grab");
    }

    public void PlayerUse()
    {
        anim.SetTrigger("Player_Use");
    }

    public void PlayerBow()
    {
        anim.SetTrigger("Player_Bow");
    }
}
