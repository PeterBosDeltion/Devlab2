using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gather : Weapon {

    public Equippable tool;


    public PlayerMovement playerMov;

    public bool waiting;
    public bool use;
	// Use this for initialization
	void Start () {

        playerMov = FindObjectOfType<PlayerMovement>();
        tool = equippable;
        Inventory.itemInHand = tool;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && playerMov.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle"))
        {
            Use();
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.name);

        if (use)
        {
            if (col.transform.tag == "Resource")
            {
                col.transform.GetComponent<Resource>().Harvest(this);
            }
          

        }
    }

    public override void Use()
    {

       
        if (!waiting)
        {
            use = true;

            if(Inventory.itemInHand != playerMov.player.GetComponent<Player>().hand)
            {
                playerMov.anim.SetBool("Playeraxestop", false);
                playerMov.anim.SetBool("Playeraxe", true);
                playerMov.anim.SetBool("Player_AxeSwing", true);
            }
            else
            {
                if (!playerMov.anim.GetBool("Playergrab"))
                {
                    playerMov.anim.SetBool("Playergrab", true);
                }
            }
          
            beingUsed = true;
            StartCoroutine(WaitForAnim());
        }
      
    }

    private IEnumerator WaitForAnim()
    {
        waiting = true;
        yield return new WaitForSeconds(playerMov.anim.GetCurrentAnimatorStateInfo(0).length);
        use = false;
        if (Inventory.itemInHand != playerMov.player.GetComponent<Player>().hand)
        {
            playerMov.anim.SetBool("Player_AxeSwing", false);
            playerMov.anim.SetBool("Playeraxestop", true);
            playerMov.anim.SetBool("Playeraxe", false);
        }
        else
        {
            playerMov.anim.SetBool("Playergrab", false);
        }
          
        beingUsed = false;
        waiting = false;
    }
}
