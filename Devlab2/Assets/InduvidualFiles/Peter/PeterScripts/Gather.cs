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
        if (Input.GetMouseButtonDown(0))
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

            if(Inventory.itemInHand != playerMov.GetComponent<Player>().hand)
            {
                playerMov.anim.ResetTrigger("Player_AxeStop");
                playerMov.anim.SetTrigger("Player_Axe");
                playerMov.anim.SetBool("Player_AxeSwing", true);
            }
            else
            {
                playerMov.anim.SetTrigger("Player_Grab");
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
        if (Inventory.itemInHand != playerMov.GetComponent<Player>().hand)
        {
            playerMov.anim.SetBool("Player_AxeSwing", false);
            playerMov.anim.SetTrigger("Player_AxeStop");
            playerMov.anim.ResetTrigger("Player_Axe");
        }
        else
        {
            playerMov.anim.ResetTrigger("Player_Grab");
        }
          
        beingUsed = false;
        waiting = false;
    }
}
