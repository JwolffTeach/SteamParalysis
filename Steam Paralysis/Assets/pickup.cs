using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickup : MonoBehaviour {

	public string pickUpType;
	public float saleAmt = 0f;
	public GameManager gm;
	public Text saleTxt;
	public AudioClip pickUpSound;
	public AudioSource source;

	// Use this for initialization
	void Start () {
		// Setup the sounds
		source = gameObject.GetComponent<AudioSource> ();
		source.clip = pickUpSound;

		if (gm == null) {
			gm = FindObjectOfType<GameManager> ();
		}
		if (pickUpType == "sale") {
			int randomSaleInt = Random.Range (1, 19);
			randomSaleInt *= 5;
			float randomSaleAmt = (float)(randomSaleInt)/100;
			saleAmt = randomSaleAmt;
			saleTxt = gameObject.GetComponent<Transform> ().GetChild (0).GetComponent<Transform> ().GetChild (0).GetComponent<Text>();
			saleTxt.text = "- "+randomSaleInt+"%";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void collect(){
		// First Play Sound
		source.PlayOneShot(pickUpSound, 0.5f);

		if (pickUpType == "pickup") {
			gm.LibraryCount += 1;
			gm.CollectGame ();
		} else if (pickUpType == "play") {
			gm.GamesPlayed += 1;
			if (gm.GetComponent<gameListParse> ().gamesCollected.Count > 0) {
				gm.playGame ();
			}
		} else if (pickUpType == "sale") {
			Debug.Log ("GO CRAZY! STEAM SALES!");
			StartCoroutine (gm.steamSale (10f, saleAmt));
			gameObject.GetComponent<destroySelf> ().destroyTime = 10f + 1f;
			gameObject.GetComponent<destroySelf> ().destroyNow = true;
		} else if (pickUpType == "job") {
			Debug.Log ("You worked at your job!");
			gameObject.GetComponent<destroySelf> ().destroyNow = true;
			gm.workJob ();
		}
	}
	// Issue destroy command.
}
