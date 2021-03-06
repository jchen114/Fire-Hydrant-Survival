﻿using UnityEngine;
using System.Collections;

public class WaterSegment : MonoBehaviour {

	SpriteRenderer mySpriteRenderer;

	Sprite[] waterSplashes;

	float timeForAnimation = 0.05f;
	float displayTime;

	bool hasCollidedWithDog = false;
	bool hasCollidedWithPowerUp = false;

	int spriteIdx;
	bool beginAnimation;

	DogBehavior dogBehavior;
	WaterPumper waterPumper;
	FireHydrant fireHydrant;

	// Use this for initialization
	void Start () {
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		waterSplashes = Resources.LoadAll <Sprite> ("Sprites/Water_Splash");
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (hasCollidedWithDog) {
			beginAnimation = true;
			Vector2 velocity = GetComponent<Rigidbody2D> ().velocity;
			velocity.Normalize ();
			dogBehavior.DogWasHit (velocity);
			hasCollidedWithDog = false;
		}

		if (hasCollidedWithPowerUp) {
			beginAnimation = true;
			hasCollidedWithPowerUp = false;
		}

		if (beginAnimation) {
			displayTime -= Time.deltaTime;

			if (displayTime <= 0) {
				// reset display time
				displayTime = timeForAnimation / waterSplashes.Length;

				if (spriteIdx == waterSplashes.Length) {
					// Delete object
					DestroyObject (gameObject);
				} else {
					// Change sprite
					mySpriteRenderer.sprite = waterSplashes[spriteIdx ++];
				}
			}
		}

	}

	void OnBecameInvisible() {
		//Debug.Log ("Destroy myself");
		DestroyObject (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == Constants.DOG_TAG && !beginAnimation) {
			//Debug.Log("WaterSegment: Hit a dog");
			spriteIdx = 0;
			mySpriteRenderer.sprite = waterSplashes[spriteIdx ++];
			gameObject.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 0);
			mySpriteRenderer.flipX = true;
			mySpriteRenderer.flipY = true;
			hasCollidedWithDog = true;
			dogBehavior = other.gameObject.GetComponent<DogBehavior> () as DogBehavior;
		}

		if (other.gameObject.tag == Constants.TAG_POWER_UP && !beginAnimation) {

			spriteIdx = 0;
			mySpriteRenderer.sprite = waterSplashes[spriteIdx ++];
			gameObject.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 0);
			mySpriteRenderer.flipX = true;
			mySpriteRenderer.flipY = true;

			hasCollidedWithPowerUp = true;

			GameObject powerUp = other.gameObject;
			PowerUp script = powerUp.GetComponent<PowerUp> () as PowerUp;

			script.Activate ();
			script.End ();


		}
	}

	public void Reset() {
		displayTime = timeForAnimation / waterSplashes.Length;
		beginAnimation = false;
	}

}
