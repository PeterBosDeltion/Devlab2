using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explainer : MonoBehaviour {
    public GameObject explainText;

    public void MouseEnter(){
        explainText.SetActive(true);
    }

    public void MouseExit() {
        explainText.SetActive(false);
    }
}
