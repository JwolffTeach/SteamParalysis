using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamesAvailable : MonoBehaviour {

	public gameListParse glp;
	public Text txt;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		string newText = "";
		for (int i=0; i < glp.gamesCollected.Count; i++){
			newText += glp.gamesCollected [i] + "\n";
		}
		txt.text = newText;
	}
}
