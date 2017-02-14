using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickPause() {
		Debug.Log ("Pause Button Clicked");
		GameManager gm = GameObject.Find (Constants.GOBJ_GAME_MANAGER).GetComponent<GameManager> ();
		gm.PauseGame ();
	}

}
