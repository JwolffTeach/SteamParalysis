using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamesPlayed : MonoBehaviour {

	public gameListParse glp;
	public Text txt;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		string newText = "";
		for (int i=0; i < glp.gamesPlayed.Count; i++){
			newText += glp.gamesPlayed [i] + "\n";
		}
		txt.text = newText;
	}
}
