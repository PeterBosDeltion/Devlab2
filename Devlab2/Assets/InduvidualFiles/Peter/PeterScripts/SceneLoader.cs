using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class SceneLoader : MonoBehaviour {

    public GameObject startPanel;
    public TMP_Dropdown sizeDropdown;

    public void StartTheGame()
    {
        startPanel.SetActive(true);
        GameManager.worldWidth = 128;
        GameManager.worldHeight = 128;
    }

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

    public void SetSize(int index)
    {
        if(index == 0)
        {
            GameManager.worldWidth = 128;
            GameManager.worldHeight = 128;
        }
        else if(index == 1)
        {
            GameManager.worldWidth = 328;
            GameManager.worldHeight = 328;
        }
        else if(index == 2)
        {
            GameManager.worldWidth = 648;
            GameManager.worldHeight = 648;
        }
    }
}
