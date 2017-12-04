using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySelf : MonoBehaviour {

	public bool destroyNow;
	public float destroyTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (destroyNow == true) {
			StartCoroutine (destroyMe (destroyTime));
			destroyNow = false;
		}
	}

	IEnumerator destroyMe(float t){
		while (Time.deltaTime / t < t) {
			t -= Time.deltaTime;
			yield return null;
		}
		Destroy (gameObject);
	}
}
