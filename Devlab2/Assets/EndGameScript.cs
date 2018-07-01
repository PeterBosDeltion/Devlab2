using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EndGameScript : MonoBehaviour {

	public void Menu() {
		Time.timeScale = 1;
		Application.LoadLevel("MainMenu");
	}

	public void Quit() {
		Application.Quit();
	}

	public void Restart() {
		Time.timeScale = 1;
		Application.LoadLevel("Main Level");
	}
}