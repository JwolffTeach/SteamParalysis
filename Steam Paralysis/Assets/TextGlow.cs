using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGlow : MonoBehaviour {

	public Color startColor;
	public Color endColor;
	public int startSize;
	public int endSize;
	public bool isActive;
	public Text glowText;
	public float saleAmt;
	public float timeLeft;

	// Use this for initialization
	void Start () {
		glowText = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			glowText.text = "Steam sale active! -" + saleAmt + "% Off!\n" + (int)timeLeft + " s left!";
			glowText.color = Color.Lerp (startColor, endColor, Mathf.PingPong (Time.time, 1));
			glowText.fontSize = (int)Mathf.Lerp (startSize, endSize, Mathf.PingPong (Time.time, 1));
		} else {
			glowText.color = new Color (0f, 0f, 0f, 0f);
		}
	}

	public void ToggleActive() {
		if (isActive) {
			isActive = false;
		} else {
			glowText.text = "Steam sale active! -" + saleAmt + "% Off!\n" + timeLeft + " s left!";
			isActive = true;
		}
	}
}
