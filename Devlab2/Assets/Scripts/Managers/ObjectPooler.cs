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
    public static ObjectPooler instance;

    public List<Pool> Pools = new List<Pool>();
    Dictionary<string, int> poolDicrionary;

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
    #region GetFromPool
    public GameObject GetFromPool(string poolInt, Vector3 position, Quaternion rotation) {
        return (TakeFromPool(poolInt, position, rotation));
    }

    public GameObject GetFromPool(string poolTag) {
        return (TakeFromPool(poolTag, Vector3.zero, Quaternion.Euler(Vector3.zero)));
    }

    public GameObject GetFromPool(int poolTag) {
        return (TakeFromPool(poolTag, Vector3.zero, Quaternion.Euler(Vector3.zero)));
    }

    public GameObject GetFromPool(int poolInt, Vector3 position, Quaternion rotation) {
        return (TakeFromPool(poolInt, position, rotation));
    }
    #endregion

    //GetFromPull Calls These Voids To Get The Object
    #region TakeFromPool
    GameObject TakeFromPool(string poolTag, Vector3 position, Quaternion rotation) {
        if(!poolDicrionary.ContainsKey(poolTag)) {
            Debug.Log("Dictionary does not contain" + tag);
            return (null);
        }

        Pool currentPool = Pools[poolDicrionary[poolTag]];
        GameObject objectToGet = null;

        if(currentPool.myQueue.Count == 0) {
            if(currentPool.autoExpand == true) {
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

    GameObject TakeFromPool(int poolInt, Vector3 position, Quaternion rotation) {
        if(Pools.Count <= poolInt) {
            Debug.Log("Dictionary does not contain" + tag);
            return (null);
        }

        Pool currentPool = Pools[poolInt];
        GameObject objectToGet = null;

        if(currentPool.myQueue.Count == 0) {
            if(currentPool.autoExpand == true) {
                objectToGet = Instantiate(currentPool.prefab);
                currentPool.startSize++;
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
    #endregion

    //Gets PoolInt Using String
    public int GetPoolInt(string poolTag) {
        if(!poolDicrionary.ContainsKey(poolTag)) {
            Debug.Log("Dictionary does not contain" + tag);
            return (0);
        }

        return (poolDicrionary[poolTag]);
    }

    //call This Void To Retrieve A Object To The Desired Pool
    public void AddToPool(string poolTag, GameObject ObjectToAdd) {

        if(!poolDicrionary.ContainsKey(poolTag)) {
            Debug.Log("Dictionary does not contain " + tag);
            return;
        }
        else if(ObjectToAdd == null) {
            Debug.Log("Object Is (Null) " + tag);
            return;
        }

        Pool currentPool = Pools[poolDicrionary[poolTag]];
        ObjectToAdd.SetActive(false);

        currentPool.myQueue.Enqueue(ObjectToAdd);
    }
}