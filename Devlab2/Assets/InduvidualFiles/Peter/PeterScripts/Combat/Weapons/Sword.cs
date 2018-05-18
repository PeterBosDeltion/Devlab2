using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    public Animator anim;
    private bool waiting;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Use();
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (anim.GetBool("using"))
        {
            Entity en = other.GetComponent<Entity>();
            if (en != null && en.transform.tag != "Player")
            {
                en.TakeDamage(equippable.damage);
            }
        }
    }

    public override void Use()
    {
        if (!waiting)
        {
            anim.SetBool("using", true);
            beingUsed = true;
            StartCoroutine(WaitForAnim());
        }

    }

    private IEnumerator WaitForAnim()
    {
        waiting = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("using", false);
        beingUsed = false;
        waiting = false;
    }
}
