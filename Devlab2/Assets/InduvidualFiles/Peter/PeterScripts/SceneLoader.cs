using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


    public void LoadNextIndexScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSpecificIndexScene(int index)
    {
        SceneManager.LoadScene(index);
    }

   

    public void ExitGame()
    {
        Application.Quit();
    }
}
