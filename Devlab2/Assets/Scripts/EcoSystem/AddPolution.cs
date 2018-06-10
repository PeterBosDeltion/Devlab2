using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPolution : MonoBehaviour {
    public int pollution;
    public ParticleSystem myPartical;
    public bool canBurn;
    bool burning;
    public int burnMin, burnMax;
    public Resource myResource;
    public Item burnedResource;

    void Start() {
        EcoManager.instance.AddPollution(pollution);
    }

    void OnEnable() {
        if(EcoManager.instance != null) {
            EcoManager.instance.AddPollution(pollution);
            EcoManager.instance.GetTile(transform.position).myOccupant = this;
        }
    }

    void OnDisable() {
        EcoManager.instance.AddPollution(-pollution);
    }

    void OnDestroy() {
        EcoManager.instance.AddPollution(-pollution);
    }

    public void Burn(){
        if(canBurn == true){
            burning = true;
        }
    }

    public IEnumerator Burning(){
        myResource.myResource = burnedResource;
        float time = Random.Range(burnMin,burnMax);
        float wait = time;

        while(time <= 0){
            time -= Time.deltaTime;
            if(Random.Range(1,100) == 1){
                EcoManager.instance.StartFire(transform.position);
            }
            yield return null;
        }
    }
}
