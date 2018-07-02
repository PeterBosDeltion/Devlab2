using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
    public Item myResource; //Still exists so we don't need to re-add everything
    public List<Item> resourceDrops = new List<Item>();
    public Equippable.CanGather type;
    public int toughness = 1;
    public Animator myAnimator;

    private bool isGrowing;
    private bool canGather = true;
    public bool berries;
    public GameObject berryChild;
    public float berryGrowTime = 7;



    private void Start() {

        myAnimator = GetComponent<Animator>();
        if (!resourceDrops.Contains(myResource)) {
            resourceDrops.Add(myResource);
        }
    }

    public void Harvest(Gather g) {

        SoundPlayer sp = GetComponent<SoundPlayer>();


        if (Inventory.itemInHand != null && Inventory.itemInHand.myGathering == type && canGather) {

            if(sp != null)
            {
                sp.ResourcePlay();

            }
            if (toughness > 0) {
                toughness--;
                myAnimator.SetTrigger("Harvest");
            } else {
                g.use = false;
                g.beingUsed = false;
                g.waiting = false;

                int itemsInInv = 0;
                foreach (Slot s in Inventory.Instance.theInventory) {

                    if (s.myItem == null) {
                        foreach (Item i in resourceDrops) {
                            Inventory.Instance.AddItem(i); //Un comment when there is inventory in scene pl0x

                        }

                        break;
                    } else {
                        itemsInInv++;
                    }

                }

                if (itemsInInv >= Inventory.Instance.theInventory.Count) {
                    ObjectPooler.instance.GetFromPool(myResource.itemName, transform.position, Quaternion.Euler(new Vector3())); //No Place Chosen Yet
                    //Drop resource on floor
                }
                if (!berries) {
                    myAnimator.SetBool("Stop", true);
                } else {
                    berryChild.SetActive(false);
                    if (!isGrowing) {
                        StartCoroutine(RegrowBerries());
                        canGather = false;
                    }
                }
            }
        }
    }

    public void DestroyResource() {
        Destroy(gameObject);
    }

    private IEnumerator RegrowBerries() {
        isGrowing = true;
        yield return new WaitForSeconds(berryGrowTime);
        berryChild.SetActive(true);
        isGrowing = false;
        canGather = true;
    }
}