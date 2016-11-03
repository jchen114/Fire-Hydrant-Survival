using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class FireHydrant : MonoBehaviour {

	const int lives = 3;
	int currentLife = 0;
	SpriteRenderer spriteRenderer;

	List<string> spritePaths = new List<string> {
		"Sprites/Fire Hydrant_0",
		"Sprites/Fire Hydrant_1",
		"Sprites/Fire Hydrant_2",
		"Sprites/Fire Hydrant_3"
	};

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetSprite() {
		spriteRenderer.sprite = Resources.Load (spritePaths [currentLife]) as Sprite;
	}

	public void GotPeedOn() {
		currentLife++;
		if (currentLife > lives) {
			GameObject gameManager = GameObject.Find (Constants.GOBJ_GAME_MANAGER);
			GameManager gm = gameManager.GetComponent<GameManager> () as GameManager;
			gm.GameOver ();

		} else {
			SetSprite();
		}
	}

	public void GotRestored() {
		currentLife--;
		SetSprite ();
	}
}
