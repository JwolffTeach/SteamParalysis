using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMovement : MonoBehaviour {

	public int moveSpeed;
	public Rigidbody2D rb;
	public Transform obj;
	public float boundaryLeft = -9f;
	public float boundaryRight = 11.5f;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		obj = gameObject.GetComponent<Transform> ();
		rb.velocity = new Vector2 (moveSpeed, 0);
	}
	
	// Update is called once per frame
	void Update() {
		if (obj.position.x < boundaryLeft) {
			obj.position = new Vector3 (boundaryRight, obj.position.y, obj.position.z);
		}
	}
}
