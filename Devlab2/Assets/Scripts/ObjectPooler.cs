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
            Queue<GameObject> _objectPool = new Queue<GameObject>();

            for(int ii = 0; ii < Pools[i].startSize; ii++) {
                GameObject _prefab = Instantiate(Pools[i].prefab);
                _prefab.SetActive(false);
                _objectPool.Enqueue(_prefab);
            }

            Pools[i].myQueue = _objectPool;
            poolDicrionary.Add(Pools[i].poolTag, i);
        }
    }

    //call This Void To Get A Object From The Desired Pool
    public GameObject GetFromPool(string _poolTag, Vector3 _position, Quaternion _rotation) {

        if(!poolDicrionary.ContainsKey(_poolTag)) {
            Debug.Log("Dictionary does not contain" + tag);
            return (null);
        }

        Pool _currentPool = Pools[poolDicrionary[_poolTag]];
        GameObject _objectToGet = null;

        if(_currentPool.myQueue.Count == 0) {
            if(_currentPool.autoExpand == true){
                _objectToGet = Instantiate(_currentPool.prefab);
                _currentPool.startSize++;
                Debug.Log("ObjectPooler: Pool Expanded(" + _poolTag + ")");
            }
            else {
                _objectToGet = _currentPool.myQueue.Dequeue();
                _currentPool.myQueue.Enqueue(_objectToGet);
            }
        }
        else {
            _objectToGet = _currentPool.myQueue.Dequeue();
        }

        _objectToGet.SetActive(true);
        _objectToGet.transform.SetPositionAndRotation(_position, _rotation);

        return (_objectToGet);
    }

    //call This Void To Retrieve A Object To The Desired Pool
    public void AddToPool(string _poolTag, GameObject _ObjectToAdd) {

        if(!poolDicrionary.ContainsKey(_poolTag)) {
            Debug.Log("Dictionary does not contain" + tag);
            return;
        }

        Pool currentPool = Pools[poolDicrionary[_poolTag]];

        _ObjectToAdd.SetActive(false);

        if(currentPool.autoExpand == false) {
            currentPool.myQueue.Enqueue(_ObjectToAdd);
        }
    }
}