using UnityEngine;
using System.Collections;

public class DogSpawner : MonoBehaviour {

	private enum SpawnType {SMALL_DOG, MEDIUM_DOG, BIG_DOG};

	float[] probabilities = new float[3] {1.0f, 0.0f, 0.0f};

	GameObject[] smallDogs;
	GameObject[] mediumDogs;
	GameObject[] bigDogs;

	float spawnTime = 1.0f;
	float remainingTime = 0.0f;

	float timeOfPlay = 0.0f;

	float screenHeight;
	float screenWidth;

	float sampleInterval = 2.0f;

	float intervalTime = 30.0f;
	int intervalCount;

	Vector2 pos;

	// Use this for initialization
	void Start () {
		remainingTime = spawnTime;

	}

	void FixedUpdate() {

		timeOfPlay += Time.deltaTime;

		remainingTime -= Time.deltaTime;

		if (timeOfPlay >= intervalTime) {
			intervalCount++;
			timeOfPlay = 0;
		}

		if (remainingTime <= 0) {
			
			// Sample probability

			Vector2 spawnLocation = new Vector2(0,0);

			//GameObject dogToSpawn;

			// Move Dog to spawn off screen.
			float position = Random.Range(0, sampleInterval);

			if (position < 0.5f) {
				Debug.Log ("Left side");
				spawnLocation.x = 0.0f;
				spawnLocation.y = position;
			} else if (position > 0.5f && position < 1.5f) {
				Debug.Log ("Top");
				spawnLocation.x = position - 0.5f;
				spawnLocation.y = 1.0f;
			} else {
				Debug.Log ("Right");
				spawnLocation.x = 1.0f;
				spawnLocation.y = position - 1.0f;
			}

			pos = Camera.main.ViewportToWorldPoint (new Vector2 (spawnLocation.x, spawnLocation.y));

			Debug.Log ("pos x " + pos.x + " pos y = " + pos.y);

			Debug.DrawLine (
				Camera.main.ViewportToWorldPoint (new Vector2 (0.5f, 0.0f)), 
				pos, 
				Color.red
			);

			float val = Random.Range(0.0f, 1.0f);

//			if (val <= probabilities [0]) {
//				// Spawn small dog
//				bool availableDog = false;
//
//				foreach (GameObject dog in smallDogs) {
//					DogBehavior script = dog.GetComponent<DogBehavior>() as DogBehavior;
//					if (script.myState == DogState.INACTIVE) {
//						availableDog = true;
//						dogToSpawn = dog;
//					}
//				}
//				if (!availableDog) {
//					// Create a Small Dog
//					dogToSpawn = Instantiate(Resources.Load(Constants.OBJ_SMALL_DOG)) as GameObject;
//				}
//			} else if (val > probabilities [0] && val <= probabilities [0] + probabilities [1]) {
//				// Spawn middle dog
//				
//			} else if (val > probabilities [0] + probabilities [1]) {
//				// Spawn big dog
//			}



			//Debug.Log ("Spawn Location x = " + spawnLocation.x + " Spawn location y = " + spawnLocation.y);

			//dogToSpawn.transform.position = spawnLocation;

			// Reset the timer
			remainingTime = spawnTime;

			//dogToSpawn.GetComponent<DogBehavior> ().UnleashDog ();

		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
