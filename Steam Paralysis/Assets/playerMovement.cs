using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

	public float xSpeed = 10f;
	public float playerJumpPower;
	public float maxJumpVelocity;
	public bool onGround;
	public float onGroundTimerStart;
	private float onGroundTimer;
	public Rigidbody2D rb;
	public GameManager gm;
	public AudioClip jumpSound;
	public AudioSource source;

	// Use this for initialization
	void Start () {
		// If Game Manager is not assigned, find it.
		if (gm == null) {
			gm = FindObjectOfType<GameManager> ();
		}
		rb = GetComponent<Rigidbody2D>();
		onGroundTimer = onGroundTimerStart;
		source = GetComponent<AudioSource> ();
		source.clip = jumpSound;
	}

	void FixedUpdate() {
		onGroundTimer -= Time.deltaTime;
		if (onGroundTimer < 0) {
			onGround = false;
		}
		//Debug.Log (onGround + " " + onGroundTimer);
	}


	// Update is called once per frame
	void Update () {
		PlayerMove ();
	}

	void PlayerMove() {
		//Controls movement of Player

		// Keep the player within the two velocity constraints.
		if (rb.velocity.y > maxJumpVelocity) {
			rb.velocity = new Vector2 (0, maxJumpVelocity);	
		} else if (rb.velocity.y < -maxJumpVelocity) {
			rb.velocity = new Vector2 (0, -maxJumpVelocity);
		}

		if (Input.GetButtonDown("Jump")){
			Jump ();
			Debug.Log ("Spacebar pressed.");
		}

	}

	void Jump() {
		//Jumping of Player
		if (onGroundTimer > 0) {
			rb.AddForce (transform.up * Mathf.Pow (playerJumpPower, 2));
			source.PlayOneShot (jumpSound, 0.5f);
		}
	}

	void OnCollisionStay2D(Collision2D collisionInfo){
		onGround = true;
		onGroundTimer = onGroundTimerStart;
		//Debug.Log ("Colliding with " + collisionInfo.transform.name);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "pickup") {
			Debug.Log ("Touched Pickup Item!");
			// Collect the item.
			pickup otherPU = other.GetComponent<pickup> ();
			otherPU.collect ();

			// Move the item.
			pickUpMovement otherMove = other.GetComponent<pickUpMovement> ();
			otherMove.ToggleActive ();
		}
	}
}