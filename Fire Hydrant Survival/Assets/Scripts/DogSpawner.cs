using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState {INACTIVE, ACTIVE};

public class DogSpawner : MonoBehaviour {

	private enum SpawnType {SMALL_DOG, MEDIUM_DOG, BIG_DOG};

	float[] probabilities = new float[3] {1.0f, 0.0f, 0.0f};

	List<GameObject> smallDogs = new List <GameObject> ();
	List<GameObject> mediumDogs = new List <GameObject> ();
	List<GameObject> bigDogs = new List <GameObject> ();

	float spawnTime = 5.0f;
	float remainingTime = 0.0f;

	float timeOfPlay = 0.0f;

	float screenHeight;
	float screenWidth;

	float sampleInterval = 2.0f;

	float intervalTime = 30.0f;
	int intervalCount;

	Vector2 pos;

	GameState myState;

	// Use this for initialization
	void Start () {

		myState = GameState.INACTIVE;

		remainingTime = spawnTime;

	}

	void FixedUpdate() {

		switch (myState) {
		case GameState.ACTIVE:
			{
				Spawner ();
			}
			break;
		case GameState.INACTIVE:
			{
				
			}
			break;
		}

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Activate() {

		myState = GameState.ACTIVE;

	}

	public void Deactivate() {

		myState = GameState.INACTIVE;

	}

	void Spawner() {
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
				spawnLocation.y = position + 0.5f;
			} else if (position > 0.5f && position < 1.5f) {
				Debug.Log ("Top");
				spawnLocation.x = position - 0.5f;
				spawnLocation.y = 1.0f;
			} else {
				Debug.Log ("Right");
				spawnLocation.x = 1.0f;
				spawnLocation.y = position - 1.0f;
			}

			Debug.Log ("spawn x,y = " + spawnLocation.x + " " + spawnLocation.y);

			pos = Camera.main.ViewportToWorldPoint (new Vector2 (spawnLocation.x, spawnLocation.y));

			Debug.Log ("pos x " + pos.x + " pos y = " + pos.y);

			float val = Random.Range(0.0f, 1.0f);

			GameObject dogToSpawn = null;

			if (val <= probabilities [0]) {
				// Spawn small dog

				foreach (GameObject dog in smallDogs) {
					DogBehavior script = dog.GetComponent<DogBehavior>() as DogBehavior;
					if (script.myState == DogState.INACTIVE) {
						dogToSpawn = dog;
					}
				}
				if (!dogToSpawn) {
					// Create a Small Dog
					dogToSpawn = Instantiate(Resources.Load(Constants.OBJ_SMALL_DOG)) as GameObject;
					//Debug.Log("Make a new dog");
				}
			} else if (val > probabilities [0] && val <= probabilities [0] + probabilities [1]) {
				// Spawn middle dog

			} else if (val > probabilities [0] + probabilities [1]) {
				// Spawn big dog
			}



			//Debug.Log ("Spawn Location x = " + spawnLocation.x + " Spawn location y = " + spawnLocation.y);

			dogToSpawn.transform.position = pos;

			// Reset the timer
			remainingTime = spawnTime;

			dogToSpawn.GetComponent<DogBehavior> ().UnleashDog ();

		}
		//		Debug.DrawLine (
		//			Camera.main.ViewportToWorldPoint (new Vector2 (0.5f, 0.0f)), 
		//			pos, 
		//			Color.red,
		//			1.0f
		//		);
	}

}
