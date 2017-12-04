using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject player;

	public int LibraryCount = 0;
	public int GamesPlayed = 0;
	public int SatisfactionRating = 0;
	public float money = 100f;

	// Spawning Information
	public float spawnRate = 2;
	private float spawnTimer;
	public ObjectPooling objPool;
	public GameObject pickupGamePrefab;
	public GameObject playGamePrefab;
	public GameObject jobPrefab;
	public GameObject steamSalePrefab;

	//Spawn Chances
	public float collectChance;
	public float playChance;
	public float jobChance;
	public float saleChance;

	// Display the LibraryCount and GamesPlayed on the screen
	public Text libraryText;
	public Text playedText;

	// Game Collection Info
	public GameObject gameTextObj;
	public Text gameNotificationText;
	public Text gameNameText;
	public gameListParse gameListObj;
	private bool gameNotificationText_fadingIn = false;
	public bool saleActive = false;

	// Money Tracker
	public Text BankAccount;
	public float priceModifier;
	public Text dollarDeduct;
	public Vector2 dollarDeductOldPos;
	public TextGlow saleGlowText;

	// Dollar Signs
	public GameObject posDollar;
	public GameObject negDollar;

	float origJump;

	// Game Over Text
	public Text gameOverText;
	public bool isWarned;


	// Use this for initialization
	void Start () {
		spawnTimer = spawnRate;

		// Set text at beginning
		libraryText.text = "Games in Library: " + LibraryCount;
		playedText.text = "Games played: " + GamesPlayed;

		gameNotificationText.text = "";
		gameNameText.text = "";
		BankAccount.text = "Bank Account: $"+money;
		dollarDeduct.text = "";
		dollarDeductOldPos = dollarDeduct.GetComponent<RectTransform> ().position;
		origJump = player.GetComponent<playerMovement>().playerJumpPower;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void FixedUpdate() {
		
		// According to spawnRate, let's spawn a new random pickup.
		spawnTimer -= Time.fixedDeltaTime;
		if (spawnTimer <= 0.0) {

			GameObject newObj;

			float spawnType;
			// Check if steam sale is active. We dont want to spawn another one until its done.
			if (priceModifier < 1 || saleActive) {
				spawnType = Random.Range (0.0f, 1.0f - saleChance);
			} else {
				spawnType = Random.Range (0.0f, 1.0f);
			}
			// < 0.5 will spawn a pickup, > 0.5 will spawn a play

			if (spawnType < collectChance) { 
				if (money <= 20) {
					newObj = (GameObject)Instantiate (jobPrefab, objPool.GetComponent<Transform> ().position, objPool.GetComponent<Transform> ().rotation);
				} else {
					newObj = (GameObject)Instantiate (pickupGamePrefab, objPool.GetComponent<Transform> ().position, objPool.GetComponent<Transform> ().rotation);
				}
			} else if (spawnType < collectChance + playChance) {
				if (gameListObj.gamesCollected.Count <= 0) {
					newObj = (GameObject)Instantiate (pickupGamePrefab, objPool.GetComponent<Transform> ().position, objPool.GetComponent<Transform> ().rotation);
				} else {
					newObj = (GameObject)Instantiate (playGamePrefab, objPool.GetComponent<Transform> ().position, objPool.GetComponent<Transform> ().rotation);
				}
			} else if (spawnType < collectChance + playChance + jobChance) {
				newObj = (GameObject)Instantiate (jobPrefab, objPool.GetComponent<Transform> ().position, objPool.GetComponent<Transform> ().rotation);
			} else// if (spawnType < collectChance + playChance + saleChance) { //spawn a steam sale!
				{
				newObj = (GameObject)Instantiate (steamSalePrefab, objPool.GetComponent<Transform> ().position, objPool.GetComponent<Transform> ().rotation);
				saleActive = true;
			}

			// Get a random y location for new pickup
			float randY = Random.Range(-3.5f, 4.5f);
			newObj.GetComponent<Transform> ().position = new Vector2 (newObj.GetComponent<Transform> ().position.x, randY);
			newObj.GetComponent<Transform> ().SetParent (objPool.GetComponent<Transform>());
			spawnTimer = spawnRate;
		}

		// Check for many games unplayed.
		if (gameListObj.gamesCollected.Count > 0) {
			// make jumping harder
			if (!isWarned && gameListObj.gamesCollected.Count > 10) {
				StartCoroutine (BadStuff (3f, "Watch out! It is more difficult to move if you have a huge UnPlayed collection!"));
				isWarned = true;
			} else if (gameListObj.gamesCollected.Count > 20) {
				StartCoroutine (EndGame(5f, "Your collection has overwhelmed you. You have Steam Paralysis and can't decide what to play.\n Game Over."));
			}
			player.GetComponent<playerMovement>().playerJumpPower = origJump - (float)(gameListObj.gamesCollected.Count)/(2f); // origJump/gameListObj.gamesCollected.Count;
			Debug.Log(player.GetComponent<playerMovement>().playerJumpPower);
		} else {
			// make jumping easy again.
			player.GetComponent<playerMovement>().playerJumpPower = origJump;
		}

	}

	public void CollectGame() {
		if (gameListObj.games.Count <= 0) {
			StartCoroutine (EndGame (5f, "Congrats! You collected every single possible Steam game... Wow that took a while."));
		} else {
			
			libraryText.text = "Games in Library: " + LibraryCount;

			gameNameText.text = gameListObj.getGame ();
			gameNotificationText.text = "A new game has been added to your library!";

			// Move the text to a random location, rotate it randomly too.
			int rotateAmount = Random.Range (-30, 30);
			int xAmount = Random.Range (-233, 215);
			int yAmount = Random.Range (-300, 62);
			gameNameText.gameObject.GetComponent<Transform> ().parent.GetComponent<Transform> ().rotation = Quaternion.Euler (0, 0, rotateAmount);
			gameNameText.gameObject.GetComponent<Transform> ().parent.GetComponent<Transform> ().localPosition = new Vector3 (xAmount, yAmount, 0);

			// Fade the text in
			StartCoroutine (FadeTextToFullAlpha (2f, gameNotificationText));
			StartCoroutine (FadeTextToFullAlpha (2f, gameNameText));

			int numDollars = Random.Range (1, 4);
			for (int i = 0; i < numDollars; i++) {
				CreateDollars (negDollar);
			}
			DeductCost (numDollars * 5);
			if (money <= 0) {
				Debug.Log ("Bummer! You are broke! Game Over...");
				StartCoroutine (EndGame (5f, "Bummer! You are broke! Game Over..."));
			}
		}
	}

	public void playGame() {
		playedText.text = "Games played: " + GamesPlayed;

		gameNameText.text = gameListObj.playGame ();
		gameNotificationText.text = "You played a game!";

		// Move the text to a random location, rotate it randomly too.
		int rotateAmount = Random.Range(-30, 30);
		int xAmount = Random.Range (-233, 215);
		int yAmount = Random.Range (-300, 62);
		gameNameText.gameObject.GetComponent<Transform>().parent.GetComponent<Transform>().rotation = Quaternion.Euler(0,0,rotateAmount);
		gameNameText.gameObject.GetComponent<Transform> ().parent.GetComponent<Transform> ().localPosition = new Vector3 (xAmount, yAmount, 0);

		// Fade the text in
		StartCoroutine(FadeTextToFullAlpha(2f, gameNotificationText));
		StartCoroutine (FadeTextToFullAlpha (2f, gameNameText));
	}

	public void workJob() {
		int numDollars = Random.Range (1, 4);
		int moneyEarned;
		moneyEarned = numDollars * 5;
		money += moneyEarned;
		BankAccount.text = "Bank Account: $" + money;
		StartCoroutine (FadeTextFromGreen (1f, BankAccount));
		dollarDeduct.text = "+$" + (moneyEarned);
		dollarDeduct.GetComponent<RectTransform>().position = dollarDeductOldPos;
		dollarDeduct.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -10f);
		StartCoroutine (FadeTextFromGreen (1f, dollarDeduct));
		StartCoroutine(FadeTextToZeroAlpha(1f, dollarDeduct));
		for (int i = 0; i < numDollars; i++) {
			CreateDollars (posDollar);
		}
	}

	public IEnumerator steamSale(float t, float saleAmt) {
		saleGlowText.saleAmt = saleAmt*100;
		saleGlowText.ToggleActive ();
		while (Time.deltaTime/t < t) {
			// Set the price modifier for saleAmt
			priceModifier = 1f - saleAmt;
			t -= Time.deltaTime;
			Debug.Log ("CURRENT ON SALE! " + Time.deltaTime/t + " and " + t);
			saleGlowText.timeLeft = t;
			saleGlowText.saleAmt = saleAmt*100;
			yield return null;
		}
		saleGlowText.ToggleActive ();
		priceModifier = 1f;
		Debug.Log ("Sale has ended. " + priceModifier);
	}
		
	public void CreateDollars(GameObject dollarType){
		GameObject newObj;
		newObj = (GameObject)Instantiate (dollarType, player.GetComponent<Transform> ().position, player.GetComponent<Transform> ().rotation);

		Vector2 randomDirection = new Vector2 (Random.Range (-5, 5), Random.Range (-5, 5));
		newObj.GetComponent<Rigidbody2D> ().velocity = randomDirection;
		StartCoroutine(FadeSpriteToZeroAlpha(1f, newObj.GetComponent<SpriteRenderer>()));
	}

	public void DeductCost(int dollars){
		money -= dollars * priceModifier;
		BankAccount.text = "Bank Account: $" + money;
		StartCoroutine (FadeTextFromRed (1f, BankAccount));
		dollarDeduct.text = "-$" + (dollars * priceModifier);
		dollarDeduct.GetComponent<RectTransform>().position = dollarDeductOldPos;
		dollarDeduct.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -10f);
		StartCoroutine (FadeTextFromRed (1f, dollarDeduct));
		StartCoroutine(FadeTextToZeroAlpha(1f, dollarDeduct));
	}

	public IEnumerator FadeTextToFullAlpha(float t, Text i)
	{
		gameNotificationText_fadingIn = true;
		i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
		while (i.color.a < 1.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
			yield return null;
		}
		StartCoroutine(FadeTextToZeroAlpha(2f, i));
	}


	public IEnumerator FadeTextToZeroAlpha(float t, Text i)
	{
		gameNotificationText_fadingIn = false;
		i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
		yield return new WaitForSeconds (1);
		while (i.color.a > 0.0f)
		{
			if (gameNotificationText_fadingIn == false) {
				i.color = new Color (i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
				yield return null;
			} else {
				yield break;
			}
		}
	}

	public IEnumerator FadeSpriteToZeroAlpha(float t, SpriteRenderer i)
	{
		Debug.Log ("This sprite is changing alpha");
		i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
		yield return new WaitForSeconds (1);
		while (i.color.a > 0.0f)
		{
			i.color = new Color (i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
			yield return null;
		}
	}

	public IEnumerator FadeTextFromRed(float t, Text i)
	{
		Color white = Color.white;
		Color red = Color.red;
		// Start red
		i.color = red;

		// Slowly change it back to white
		while (i.color.g < 1) {
			float timeAmt = Time.deltaTime / t;
			i.color = new Color (i.color.r, i.color.g + timeAmt, i.color.b + timeAmt);
			yield return null;
		}
	}

	public IEnumerator FadeTextFromGreen(float t, Text i)
	{
		Color white = Color.white;
		Color green = Color.green;
		// Start red
		i.color = green;

		// Slowly change it back to white
		while (i.color.r < 1) {
			float timeAmt = Time.deltaTime / t;
			i.color = new Color (i.color.r + timeAmt, i.color.g, i.color.b + timeAmt);
			yield return null;
		}
	}

	public IEnumerator EndGame(float t, string loseText){
		gameOverText.text = loseText;
		StartCoroutine (FadeTextToFullAlpha (1f, gameOverText));
		while (Time.deltaTime / t < t) {
			t -= Time.deltaTime;
			yield return null;
		}
		gameObject.GetComponent<ButtonManager> ().NewGamebtn ("gameOver");
	}

	public IEnumerator BadStuff(float t, string badText){
		gameOverText.text = badText;
		StartCoroutine (FadeTextToFullAlpha (1f, gameOverText));
		while (Time.deltaTime / t < t) {
			t -= Time.deltaTime;
			yield return null;
		}
	}
}
