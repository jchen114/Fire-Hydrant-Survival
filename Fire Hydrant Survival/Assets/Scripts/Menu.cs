﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {

	const float BLINK_TIME = 1.0f;

	Color redColor = new Color(1, 0, 0);
	Color blueColor = new Color(0, 0, 1);

	Image background;

	float blinkTime = 0.0f;

	Text titleText;
	Text playText;
	bool flipColor = false;

	// Use this for initialization
	void Start () {
		background = GetComponent<Image> ();

		Color temp = background.color;
		temp.a = 0.5f;
		background.color = temp;

		foreach (Transform child in transform) {
			if (child.name == Constants.TEXT_GAME_TITLE) {
				Debug.Log ("Found title");
				titleText = child.gameObject.GetComponent<Text>();
			}
			if (child.name == Constants.TEXT_PLAY_TITLE) {
				Debug.Log ("Found Play");
				playText = child.gameObject.GetComponent<Text> ();
				EventTrigger trigger = playText.GetComponent<EventTrigger> ();
				EventTrigger.Entry entry = new EventTrigger.Entry ();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener((data)=> {	
					OnPointerClickDelegate((PointerEventData) data);
				});
				trigger.triggers.Add (entry);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		
		blinkTime -= Time.deltaTime;

		if (blinkTime <= 0) {
			blinkTime = 0.0f;
			titleText.color = flipColor ? blueColor : redColor;
			flipColor = !flipColor;
			blinkTime = BLINK_TIME;
		}

	}

	public void OnPointerClickDelegate(PointerEventData data) {
		Debug.Log ("Play text called");
		GameObject gameManager = GameObject.Find (Constants.GOBJ_GAME_MANAGER);
		gameManager.GetComponent<GameManager> ().PlayGame ();
	}

}
