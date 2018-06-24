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
        EcoManager.instance.AddPollution(-pollution);
    }

    public void Burn() {
        if (canBurn == true) {
            burning = true;
        }
    }

    public IEnumerator Burning() {
        myResource.myResource = burnedResource;
        float time = Random.Range(burnMin, burnMax);
        float baseTime = time;
        float wait = time;
        myPartical.SetActive(true);

        while (time <= 0) {
            time -= Time.deltaTime;
            myRenderer.material.color = myRenderer.material.color - new Color(255 * time / baseTime, 255 * time / baseTime, 255 * time / baseTime, 255 * time / baseTime);

            if (Random.Range(1, 1000)== 1) {
                EcoManager.instance.StartFire(transform.position);
            }
            yield return null;
        }

        Destroy(toDestroy);

        myPartical.SetActive(false);
    }
}