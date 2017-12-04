using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void NewGamebtn(string newGameLevel){
		SceneManager.LoadScene (newGameLevel);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
