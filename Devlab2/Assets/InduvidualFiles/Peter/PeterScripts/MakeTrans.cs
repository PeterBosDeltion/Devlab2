using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTrans : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);


        
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend)
            {
                // Change the material of all hit colliders
                // to use a transparent shader.
                if(rend.gameObject.layer == 16) //Roof layer
                {
                    Color tempColor = rend.sharedMaterial.color;
                    tempColor.a = 0.3F;
                    rend.sharedMaterial.color = tempColor;
                }
              
            }
        }
    }

   
}
