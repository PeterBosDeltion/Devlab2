using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool {
    public Queue<GameObject> myQueue;
    public string poolTag;
    public GameObject prefab;
    public bool autoExpand;
    public int startSize;
}

public class ObjectPooler : MonoBehaviour {
    [HideInInspector]
    public static ObjectPooler instance;

    public List<Pool> Pools = new List<Pool>();
    Dictionary<string, int> poolDicrionary;
    int poolsInt;

    //This Creates The Pools And Creates The Objects That You Can Pool
    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.Log("Instance " + typeof(ObjectPooler) + " Dubble Trouble: " + gameObject.name);
            Destroy(this);
        }

        poolDicrionary = new Dictionary<string, int>();

        for(int i = 0; i < Pools.Count; i++) {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int ii = 0; ii < Pools[i].startSize; ii++) {
                GameObject prefab = Instantiate(Pools[i].prefab);
                prefab.SetActive(false);
                objectPool.Enqueue(prefab);
            }

            Pools[i].myQueue = objectPool;
            poolDicrionary.Add(Pools[i].poolTag, i);
        }
    }

    //call This Void To Get A Object From The Desired Pool
    public GameObject GetFromPool(string poolTag, Vector3 position, Quaternion rotation) {

        if(!poolDicrionary.ContainsKey(poolTag)) {
            Debug.Log("Dictionary does not contain" + tag);
            return (null);
        }

        Pool currentPool = Pools[poolDicrionary[poolTag]];
        GameObject objectToGet = null;

        if(currentPool.myQueue.Count == 0) {
            if(currentPool.autoExpand == true){
                objectToGet = Instantiate(currentPool.prefab);
                currentPool.startSize++;
                Debug.Log("ObjectPooler: Pool Expanded(" + poolTag + ")");
            }
            else {
                objectToGet = currentPool.myQueue.Dequeue();
                currentPool.myQueue.Enqueue(objectToGet);
            }
        }
        else {
            objectToGet = currentPool.myQueue.Dequeue();
        }

        objectToGet.SetActive(true);
        objectToGet.transform.SetPositionAndRotation(position, rotation);

        return (objectToGet);
    }

    //call This Void To Retrieve A Object To The Desired Pool
    public void AddToPool(string poolTag, GameObject ObjectToAdd) {

        if(!poolDicrionary.ContainsKey(poolTag)) {
            Debug.Log("Dictionary does not contain" + tag);
            return;
        }

        Pool currentPool = Pools[poolDicrionary[poolTag]];

        ObjectToAdd.SetActive(false);

        if(currentPool.autoExpand == false) {
            currentPool.myQueue.Enqueue(ObjectToAdd);
        }
    }
}