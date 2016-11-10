using UnityEngine;
using System.Collections;


enum GAME_STATE {START_MENU, START_PLAYING, PLAYING, PAUSE, GAME_OVER};

public class GameManager : MonoBehaviour {

	DogSpawner dogSpawner;

	GAME_STATE myState;

	GameObject StartMenu;
	GameObject PauseMenu;
	GameObject EndMenu;


	// Use this for initialization
	void Start () {

		dogSpawner = GameObject.Find (Constants.OBJ_DOG_SPAWNER).GetComponent<DogSpawner>() as DogSpawner;
		dogSpawner.Deactivate ();

		myState = GAME_STATE.START_MENU;

		// Instantiate Start menu
		StartMenu = Instantiate(Resources.Load(Constants.UI_START_MENU)) as GameObject;
		StartMenu.SetActive (true);

		PauseMenu = Instantiate (Resources.Load (Constants.UI_GAME_MENU)) as GameObject;
		PauseMenu.SetActive (false);

		EndMenu = Instantiate (Resources.Load (Constants.UI_GAME_OVER_MENU)) as GameObject;
		EndMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {

		switch (myState) {

		case GAME_STATE.START_MENU:
			break;
		case GAME_STATE.START_PLAYING:
			{
				// Remove the menu
				StartMenu.SetActive(false);
				dogSpawner.Activate ();
			}
			break;
		case GAME_STATE.PLAYING:
			{
				
			}
			break;
		case GAME_STATE.PAUSE:
			
			break;
		case GAME_STATE.GAME_OVER:
			break;

		}

	}

	public void PlayGame() {
		myState = GAME_STATE.START_PLAYING;
	}

	public void ResetGame() {
		StartMenu.SetActive (true);
		myState = GAME_STATE.START_MENU;
	}

	public void GameOver() {
		Debug.Log ("Game Over!");
		myState = GAME_STATE.GAME_OVER;
		// Display the Game over menu
	}

	public void PauseGame() {
		Debug.Log ("Pause Game!");
		myState = GAME_STATE.PAUSE;
		PauseMenu.SetActive (true);
	}

	public void UnPauseGame() {
		Debug.Log ("Unpause Game");
		PauseMenu.SetActive (false);
		myState = GAME_STATE.PLAYING;
	}

}
