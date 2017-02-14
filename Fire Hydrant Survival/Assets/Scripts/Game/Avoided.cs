using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Avoided : MonoBehaviour {

	int bodyCount = 0;
	int highScore = 0;
	Text bodyCountText;

	// Use this for initialization
	void Start () {
		bodyCount = 0;
		bodyCountText = this.gameObject.GetComponent<Text> ();
		UpdateText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHighScore (int score) {
		highScore = score;
		UpdateText ();
	}

	public void Increment() {
		bodyCount += 1;
		UpdateText ();
	}

	public void Reset() {
		bodyCount = 0;
		UpdateText ();
	}

	void UpdateText() {
		if (bodyCountText) {
			bodyCountText.text = "Score: " + bodyCount.ToString("00") + "\n" + "High: <color=red>" + highScore.ToString("00") + "</color>";
		}
	}

	public int GetScore() {
		return bodyCount;
	}

}
