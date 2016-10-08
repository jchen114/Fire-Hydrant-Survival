using UnityEngine;
using System.Collections;

public class WaterSegment : MonoBehaviour {

	SpriteRenderer mySpriteRenderer;

	Sprite[] waterSplashes;

	float timeForAnimation = 0.05f;
	float displayTime;

	bool hasCollided = false;

	int spriteIdx;

	DogBehavior dogBehavior;

	// Use this for initialization
	void Start () {
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		waterSplashes = Resources.LoadAll <Sprite> ("Sprites/Water_Splash");
		displayTime = timeForAnimation / waterSplashes.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (hasCollided) {

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

			Vector2 velocity = GetComponent<Rigidbody2D> ().velocity;
			velocity.Normalize ();
			dogBehavior.DogWasHit (velocity);

		}
	}

	void OnBecameInvisible(		) {
		//Debug.Log ("Destroy myself");
		DestroyObject (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == Constants.DOG_TAG) {
			//Debug.Log("Hit a dog");
			spriteIdx = 0;
			mySpriteRenderer.sprite = waterSplashes[spriteIdx ++];
			gameObject.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 0);
			mySpriteRenderer.flipX = true;
			mySpriteRenderer.flipY = true;
			hasCollided = true;
			dogBehavior = other.gameObject.GetComponent<DogBehavior> () as DogBehavior;
		}   
	} 
}
