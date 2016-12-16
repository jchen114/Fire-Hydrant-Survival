using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class FireHydrant : MonoBehaviour {

	const int lives = 4;
	int currentLife = 1;
	SpriteRenderer spriteRenderer;
	Sprite[] sprites;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentLife = 1;
		sprites = Resources.LoadAll<Sprite>("Sprites/Fire Hydrant");
		SetSprite ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetSprite() {
		spriteRenderer.sprite = sprites[currentLife];
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

	public void Reset () {
		currentLife = 1;
		SetSprite ();
	}

}
