using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpMovement : MonoBehaviour {

	public int moveSpeed;
	public Rigidbody2D rb;
	public Transform obj;
	public float boundaryLeft = -9f;
	public float boundaryRight = 9f;
	public bool isActive;
	public Vector3 poolLoc;
	public GameManager gm;


	// Use this for initialization
	void Start () {
		// Record our starting position
		gm = FindObjectOfType<GameManager>();

		poolLoc = GetComponentInParent<Transform> ().position;

		// Start moving
		rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (moveSpeed, 0);

		// Assign this object's transform
		obj = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (obj.position.x < boundaryLeft) {
			rb.velocity = new Vector2 (0, 0);
			obj.localPosition = new Vector3(0, 0, 0);
			gameObject.GetComponent<destroySelf> ().destroyNow = true;
			if (gameObject.GetComponent<pickup> ().pickUpType == "sale") {
				gm.saleActive = false;
			}
		}
	}

	public void ToggleActive() {
		if (!isActive) {
			rb.velocity = new Vector2 (moveSpeed, 0);
			gm.saleActive = true;
		} else {
			rb.velocity = new Vector2 (0, 0);
			obj.localPosition = new Vector3(0, 0, 0);
			gameObject.GetComponent<destroySelf> ().destroyNow = true;
			gm.saleActive = false;
		}
	}
}
