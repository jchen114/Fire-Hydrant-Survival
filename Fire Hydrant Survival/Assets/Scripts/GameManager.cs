using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum GAME_STATE {START_MENU, START_PLAYING, PLAYING, PAUSE, GAME_OVER};

public class GameManager : MonoBehaviour {

	[System.Serializable]
	public class GameStats {
		public int HighScore;

		public GameStats(int score) {
			HighScore = score;
		}
	}


	#region Objects

	DogSpawner Spawner;

	public GAME_STATE myState;

	GameObject StartMenu;
	GameObject PauseMenu;
	GameObject EndMenu;
	GameObject[] FireHydrants;
	Avoided ScoreText;
	WaterPumper Pumper;
	PowerUpSpawner PUpSpawner;
	int HighScore;

	#endregion

	#region UNITY

	// Use this for initialization
	void Start () {

		myState = GAME_STATE.START_MENU;
	
		// Set up Game objects
		Spawner = GameObject.Find (Constants.OBJ_DOG_SPAWNER).GetComponent<DogSpawner>() as DogSpawner;
		FireHydrants = GameObject.FindGameObjectsWithTag (Constants.TAG_HYDRANTS);
		Pumper = GameObject.Find (Constants.OBJ_WATER_PUMPER).GetComponent<WaterPumper>();
		ScoreText = GameObject.Find (Constants.TEXT_SCORE).GetComponent<Avoided>();
		PUpSpawner = GameObject.Find (Constants.GOBJ_POWER_UP_SPAWNER).GetComponent<PowerUpSpawner> ();
		DeactivateGame ();

		// Instantiate Start menu
		StartMenu = Instantiate(Resources.Load(Constants.UI_START_MENU)) as GameObject;
	
		PauseMenu = Instantiate (Resources.Load (Constants.UI_GAME_MENU)) as GameObject;

		EndMenu = Instantiate (Resources.Load (Constants.UI_GAME_OVER_MENU)) as GameObject;

		ResetGame ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {

		switch (myState) {

		case GAME_STATE.START_MENU:
			{
				
			}
			break;
		case GAME_STATE.PLAYING:
			{
				
			}
			break;
		case GAME_STATE.PAUSE:
			{
				
			}
			break;
		case GAME_STATE.GAME_OVER:
			{
				
			}
			break;

		}

	}

	#endregion

	#region MANAGER

	public void PlayGame() {
		myState = GAME_STATE.START_PLAYING;
		// Remove the menu
		StartMenu.SetActive(false);
		ActivateGame ();
	}

	public void ResetGame() {
		PauseMenu.SetActive (false);
		EndMenu.SetActive (false);
		StartMenu.SetActive (true);
		myState = GAME_STATE.START_MENU;

		Reset ();

		// Reset Hydrants
		foreach (GameObject hydrant in FireHydrants) {
			hydrant.GetComponent<FireHydrant> ().Reset ();
		}

		HighScore = 0;
		// Load
		Load();
		ScoreText.SetHighScore (HighScore);

	}

	public void GameOver() {
		Debug.Log ("Game Over!");
		myState = GAME_STATE.GAME_OVER;
		// Display the Game over menu
		PauseMenu.SetActive(false);
		EndMenu.SetActive(true);
		StartMenu.SetActive (false);
		DeactivateGame();

		SetScore ();

	}

	public void PauseGame() {
		Debug.Log ("Pause Game!");
		myState = GAME_STATE.PAUSE;
		PauseMenu.SetActive (true);
		EndMenu.SetActive (false);
		StartMenu.SetActive (false);
		DeactivateGame ();
	}

	public void UnPauseGame() {
		Debug.Log ("Unpause Game");
		PauseMenu.SetActive (false);
		EndMenu.SetActive (false);
		StartMenu.SetActive (false);
		myState = GAME_STATE.PLAYING;
		ActivateGame ();
	}

	void SetScore() {
		int CurrentScore = ScoreText.GetScore ();
		if (CurrentScore > HighScore) {
			HighScore = CurrentScore;
			Save ();
		}
	}

	#endregion

	#region INTERFACE

	public void ActivateGame() {
		Spawner.Activate ();
		Pumper.Activate ();
		PUpSpawner.Activate ();
	}

	public void DeactivateGame() {
		Spawner.Deactivate ();
		Pumper.Deactivate ();
		PUpSpawner.Deactivate ();
	}

	public void Reset() {
		Spawner.Reset ();
		PUpSpawner.Reset ();
		Pumper.Reset ();
	}

	#endregion

	#region MEMORY

	void Save() {
		// TODO
		// Save High Score
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
		GameStats gs = new GameStats(HighScore);
		bf.Serialize(file, gs);
		file.Close();

	}

	public void Load() {
		// TODO
		// Load the high score
		if (File.Exists (Application.persistentDataPath + "/gameInfo.dat")) {
			Debug.Log ("Loading High Score");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
			GameStats gs = (GameStats)bf.Deserialize (file);
			file.Close ();
			HighScore = gs.HighScore;
		}

	}

	#endregion

}
