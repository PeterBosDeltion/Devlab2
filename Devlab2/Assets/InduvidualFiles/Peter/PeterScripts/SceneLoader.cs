using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class SceneLoader : MonoBehaviour {

    public GameObject startPanel;
    public TMP_Dropdown sizeDropdown;
    public SettingsMenu sm;

    public void StartTheGame()
    {
        sm.mainMenu.SetActive(false);
        sm.mainCam.enabled = false;
        sm.settingsCam.enabled = false;
        sm.startCam.enabled = true;
        startPanel.SetActive(true);
        GameManager.worldWidth = 1000;
    }

    public void LoadNextIndexScene()
    {
        SceneManager.LoadScene("Main Level");
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
            GameManager.worldWidth = 1000;
        }
        else if(index == 1)
        {
            GameManager.worldWidth = 2000;
        }
        else if(index == 2)
        {
            GameManager.worldWidth = 4000;
        }
    }
}
