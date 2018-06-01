using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisible : MonoBehaviour {
    private Renderer myRenderer;
    private bool playerIsHere;
    private Transform par;

	// Use this for initialization
	void Start () {
        par = transform.parent;
        myRenderer = par.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!playerIsHere)
        {
            if(myRenderer.sharedMaterial.color.a != 1)
            {
                Color tempColor = myRenderer.sharedMaterial.color;
                tempColor.a = 1;
                myRenderer.sharedMaterial.color = tempColor;
            }
         
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            playerIsHere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerIsHere = false;
        }
    }
}
