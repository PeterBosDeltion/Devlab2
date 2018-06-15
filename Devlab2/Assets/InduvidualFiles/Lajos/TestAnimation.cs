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
        anim.SetBool("Player_AxeSwing", true);
    }

    public void PlayerStopSwing()
    {
        anim.SetBool("Player_AxeSwing", false);
        anim.SetTrigger("Player_AxeStop");
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
