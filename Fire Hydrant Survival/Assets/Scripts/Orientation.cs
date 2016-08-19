using UnityEngine;
using System.Collections;

public class Orientation : MonoBehaviour {

	// Use this for initialization
	void Start() {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Physics2D.gravity = Vector2.zero;
	}
}
