using UnityEngine;
using System.Collections;


enum GAME_STATE {START_MENU, START_PLAYING, PLAYING, PAUSE, GAME_OVER};

public class GameManager : MonoBehaviour {

	DogSpawner dogSpawner;

	GAME_STATE myState;

	GameObject StartMenu;


	// Use this for initialization
	void Start () {

		dogSpawner = GameObject.Find (Constants.OBJ_DOG_SPAWNER).GetComponent<DogSpawner>() as DogSpawner;
		dogSpawner.Deactivate ();

		myState = GAME_STATE.START_MENU;

		// Instantiate Start menu
		StartMenu = Instantiate(Resources.Load(Constants.UI_START_MENU)) as GameObject;
		StartMenu.SetActive (true);

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
		myState = GAME_STATE.GAME_OVER;
		// Display the Game over menu
	}

}
