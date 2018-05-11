using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explainer : MonoBehaviour {
    public GameObject explainText;

    public void OnButtonClick(){
        explainText.SetActive(true);
    }

    public void Deselect() {
        explainText.SetActive(false);
    }
}
