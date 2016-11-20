using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GAME_STATE {START_MENU, START_PLAYING, PLAYING, PAUSE, GAME_OVER};

public class GameManager : MonoBehaviour {

	DogSpawner Spawner;

	public GAME_STATE myState;

	GameObject StartMenu;
	GameObject PauseMenu;
	GameObject EndMenu;
	GameObject[] FireHydrants;
	WaterPumper Pumper;


	// Use this for initialization
	void Start () {

		myState = GAME_STATE.START_MENU;

		// Instantiate Start menu
		StartMenu = Instantiate(Resources.Load(Constants.UI_START_MENU)) as GameObject;
		StartMenu.SetActive (true);

		PauseMenu = Instantiate (Resources.Load (Constants.UI_GAME_MENU)) as GameObject;
		PauseMenu.SetActive (false);

		EndMenu = Instantiate (Resources.Load (Constants.UI_GAME_OVER_MENU)) as GameObject;
		EndMenu.SetActive (false);

		// Set up Game objects
		Spawner = GameObject.Find (Constants.OBJ_DOG_SPAWNER).GetComponent<DogSpawner>() as DogSpawner;
		FireHydrants = GameObject.FindGameObjectsWithTag (Constants.TAG_HYDRANTS);
		Pumper = GameObject.Find (Constants.OBJ_WATER_PUMPER).GetComponent<WaterPumper>();
		DeactivateGame ();
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
		// Reset Dog Spawner
		Spawner.Reset ();
		// Reset Hydrants
		foreach (GameObject hydrant in FireHydrants) {
			hydrant.GetComponent<FireHydrant> ().Reset ();
		}
		// Reset water
		Pumper.Reset();
	}

	public void GameOver() {
		Debug.Log ("Game Over!");
		myState = GAME_STATE.GAME_OVER;
		// Display the Game over menu
		PauseMenu.SetActive(false);
		EndMenu.SetActive(true);
		StartMenu.SetActive (false);
		DeactivateGame();
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

	public void ActivateGame() {
		Spawner.Activate ();
		Pumper.Activate ();
	}

	public void DeactivateGame() {
		Spawner.Deactivate ();
		Pumper.Deactivate ();
	}

}
