using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPolution : MonoBehaviour {
    public int pollution;
    public GameObject myPartical;
    public bool canBurn;
    bool burning;
    public int burnMin, burnMax;
    public Resource myResource;
    public Item burnedResource;
    public Renderer myRenderer;
    public GameObject toDestroy;
    public Animator myAnimation;
    public bool fullDestroy;

    void OnEnable() {
        if (EcoManager.instance != null) {
            EcoManager.instance.AddPollution(pollution);
            EcoManager.instance.GetTile(transform.position).myTile.myOccupant = this;
        }
    }

    void OnDisable() {
        EcoManager.instance.AddPollution(-pollution);
    }

    void OnDestroy() {
        if (canBurn == true) {
            EcoManager.instance.AddPollution(-pollution);
        }
    }

    public void Burn() {
        if (canBurn == true) {
            burning = true;
        }
    }

    public IEnumerator Burning() {
        myResource.myResource = burnedResource;
        canBurn = false;
        float time = Random.Range(burnMin, burnMax);

        float baseTime = time;
        float wait = time;
        myPartical.SetActive(true);
        myAnimation.SetTrigger("Burn");

        while (time >= 0) {
            time -= Time.deltaTime;

            if (Random.Range(1, 1000)== 1) {
                EcoManager.instance.BurnGrounds(transform.position, 1, "Burned Because Fire Spread", "Burned Because Fire Spread", "Burned Because Fire Spread");
            }
            yield return null;
        }

        EcoManager.instance.AddPollution(-pollution);

        if (fullDestroy == true) {
            Destroy(gameObject);
        }

        myPartical.SetActive(false);
    }
}