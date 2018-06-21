using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : Weapon {
    public Animator anim;
    public Material farmLandMat;
    private bool waiting;
    private bool use;
    private PlayerMovement playerMov;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        playerMov = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Use();
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (beingUsed) {
            if (other.GetComponent<ThisTile>()) {
                //Change with layers to prevent water farming
                other.GetComponent<Renderer>().material = farmLandMat;
                other.gameObject.layer = 14;
            }
        }

    }

    public override void Use() {
        if (!waiting) {
            use = true;

            if (Inventory.itemInHand != playerMov.player.GetComponent<Player>().hand) {
                playerMov.anim.SetBool("Playeraxestop", false);
                playerMov.anim.SetBool("Playeraxe", true);
                playerMov.anim.SetBool("Player_AxeSwing", true);
            } else {
                if (!playerMov.anim.GetBool("Playergrab")) {
                    playerMov.anim.SetBool("Playergrab", true);
                }
            }

            beingUsed = true;
            StartCoroutine(WaitForAnim());
        }
    }

    private IEnumerator WaitForAnim() {
        waiting = true;
        yield return new WaitForSeconds(playerMov.anim.GetCurrentAnimatorStateInfo(0).length);
        use = false;
        if (Inventory.itemInHand != playerMov.player.GetComponent<Player>().hand) {
            playerMov.anim.SetBool("Player_AxeSwing", false);
            playerMov.anim.SetBool("Playeraxestop", true);
            playerMov.anim.SetBool("Playeraxe", false);
        } else {
            playerMov.anim.SetBool("Playergrab", false);
        }

        beingUsed = false;
        waiting = false;
    }
}