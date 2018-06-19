using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    private bool waiting;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            Animator anim = other.GetComponentInChildren<Animator>();

            if (!anim.GetBool("Playeruse"))
            {
                anim.SetBool("Playeruse", true);
                StartCoroutine(WaitForAnimation(anim));
            }

           // Interact intr = other.GetComponent<Interact>();
           // StartInteract(intr);
        }
    }

    public void StartInteract(Interact intr)
    {
        Interact.currentlyInteracting = this;
    }

    private IEnumerator WaitForAnimation(Animator anim)
    {
        waiting = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("Playeruse", false);
        waiting = false;
    }
}
