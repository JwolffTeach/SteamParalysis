using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

	public Transform trans;
	public List<GameObject> objectPool = new List<GameObject>();

	// Use this for initialization
	void Start () {
		trans = gameObject.GetComponent<Transform> ();
		for (int i = 0; i < trans.childCount; i++) {
			objectPool.Add (trans.GetChild (i).gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
