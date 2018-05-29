using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBuild : MonoBehaviour {
    public BoxCollider myCollider;

    void OnEnable() {
        if(Builder.instance != null)
        {
            if (Builder.instance.displayCollider != null)
            {
                Builder.instance.displayCollider = myCollider;
            }
        }
      
    }
}
