using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PowerUpSpawner : MonoBehaviour, IManageable {

	private const float spawnTimeMean = 1.0f;
	private const float spawnTimeStdDev = 0.1f;
	private float timeUntilSpawn;
	List <float> powerUpPs = new List<float> {0.34f, 0.33f, 0.33f}; // Health, Speed, Frequency

	GameState myState;

	#region UNITY

	// Use this for initialization
	void Start () {
		 // Sample for spawn time.
		myState = GameState.INACTIVE;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		switch (myState) {
		case GameState.ACTIVE:
			{
				timeUntilSpawn -= Time.deltaTime;
				if (timeUntilSpawn <= 0) {
					Debug.Log ("Spawn Power up");
					Spawn ();
					timeUntilSpawn = RandomFromDistribution.RandomNormalDistribution(spawnTimeMean, spawnTimeStdDev);
				}
			}
			break;
		case GameState.INACTIVE:
			break;
		}
	}

	void Spawn() {
		// Spawn a power up3/sxfgl/;lc 
		int choice = RandomFromDistribution.RandomChoiceFollowingDistribution(powerUpPs);
		PowerUp pUp = null;
		switch (choice) {
		case 0:
			{
				GameObject health = Instantiate (Resources.Load (Constants.POWER_UP_HEALTH)) as GameObject;
				pUp = health.GetComponent<PowerUp> () as PowerUp;
			}
			break;
		case 1:
			{
				GameObject speed = Instantiate (Resources.Load (Constants.POWER_UP_SPEED)) as GameObject;
				pUp = speed.GetComponent<PowerUp> () as PowerUp;
			}
			break;
		case 2:
			{
				GameObject freq = Instantiate (Resources.Load (Constants.POWER_UP_FREQ)) as GameObject;
				pUp = freq.GetComponent<PowerUp> () as PowerUp;
			}
			break;
		}
		// TODO 
		// Place the power up somewhere random on top half of window
		float y = RandomFromDistribution.RandomRangeLinear(0.5f, 1.0f, 0.0f);
		float x = RandomFromDistribution.RandomRangeLinear (0.0f, 1.0f, 0.0f);

		Vector2 pos = Camera.main.ViewportToWorldPoint (new Vector2 (x, y));

		pUp.transform.position = pos;

		pUp.Begin ();
	}

	#endregion

	#region 

	public void Reset() {
		timeUntilSpawn = RandomFromDistribution.RandomNormalDistribution(spawnTimeMean, spawnTimeStdDev);
		myState = GameState.INACTIVE; 
		Debug.Log ("Power Up time until spawn: " + timeUntilSpawn);
	}

	public void Deactivate() {
		myState = GameState.INACTIVE;
	}

	public void Activate() {
		myState = GameState.ACTIVE;
	}

	#endregion

}
