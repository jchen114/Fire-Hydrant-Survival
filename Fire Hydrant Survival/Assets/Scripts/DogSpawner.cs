using UnityEngine;
using System.Collections;

public class DogSpawner : MonoBehaviour {

	private enum SpawnType {SMALL_DOG, MEDIUM_DOG, BIG_DOG};

	float[] probabilities = new float[3] {1.0f, 0.0f, 0.0f};

	GameObject[] smallDogs;
	GameObject[] mediumDogs;
	GameObject[] bigDogs;

	float spawnTime = 1.0f;
	float remainingTime;

	float timeOfPlay = 0.0f;

	float screenHeight;
	float screenWidth;

	float sampleInterval;

	float intervalTime = 30.0f;
	int intervalCount;

	Vector2 spawnLocation = new Vector2(0,0);


	// Use this for initialization
	void Start () {
		remainingTime = spawnTime;
		screenHeight = Camera.main.orthographicSize * 2.0f;
		screenWidth = screenHeight * Screen.width / Screen.height;

		sampleInterval = screenHeight / 2 + screenWidth + screenHeight / 2;


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
			float val = Random.Range(0.0f, 1.0f);

			GameObject dogToSpawn;

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

			// Move Dog to spawn off screen.
			float position = Random.Range(0, sampleInterval);

			if (position < screenHeight / 2) {
				spawnLocation.x = 0.0f;
				spawnLocation.y = screenHeight / 2 + position;
			} else if (position > screenHeight / 2 && position < screenHeight / 2 + screenWidth) {
				spawnLocation.x = position;
				spawnLocation.y = screenHeight;
			} else {
				spawnLocation.x = screenWidth;
				spawnLocation.y = screenHeight / 2 + position - screenHeight / 2 - screenWidth;
			}

			//Debug.Log ("Spawn Location x = " + spawnLocation.x + " Spawn location y = " + spawnLocation.y);



			//dogToSpawn.transform.position = spawnLocation;

			// Reset the timer
			remainingTime = spawnTime;

			//dogToSpawn.GetComponent<DogBehavior> ().UnleashDog ();

		}

	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (spawnLocation, 5);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
