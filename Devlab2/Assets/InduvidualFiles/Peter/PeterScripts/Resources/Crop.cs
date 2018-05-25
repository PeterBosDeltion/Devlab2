using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour {
    public GameObject fullyGrown;

    public float growTime;
    private bool growing;

	// Use this for initialization
	void Start () {
        if (!growing)
        {
            StartCoroutine(Grow());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator Grow()
    {
        growing = true;
        yield return new WaitForSeconds(growTime);
        fullyGrown.transform.SetParent(null);
        fullyGrown.SetActive(true);

        Destroy(gameObject);
    }
}
