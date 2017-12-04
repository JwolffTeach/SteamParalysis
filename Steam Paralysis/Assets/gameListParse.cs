using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class gameListParse : MonoBehaviour {

	public List<string> games;
	public List<string> gamesCollected;
	public List<string> gamesPlayed;

	// Use this for initialization
	void Start () {
		games = System.IO.File.ReadAllLines("Assets/Input Files/gamelist.txt").ToList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string getGame(){
		int randomLine = Random.Range(0, games.Count);
		string theGame = games [randomLine];
		gamesCollected.Add (theGame);
		games.RemoveAt (randomLine);
		return theGame;
	}

	public string playGame(){
		int randomLine = Random.Range (0, gamesCollected.Count);
		string theGame = gamesCollected [randomLine];
		gamesPlayed.Add (theGame);
		gamesCollected.RemoveAt (randomLine);
		return theGame;
	}
}
