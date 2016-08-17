using UnityEngine;
using System.Collections;

public class WaterPumper : MonoBehaviour {

	private bool isAppEditing;
	private bool touchStartedOnHydrant;
	private Vector2 centerOfHydrant;

	private GameObject hydrant;

	// Use this for initialization
	void Start () {
		print ("Water pumper script active.");
		isAppEditing = Application.isEditor;
		if (isAppEditing) {
			print ("Editing");
		} else {
			print ("On mobile");
		}

		touchStartedOnHydrant = false;

		hydrant = GameObject.Find (Constants.FIRE_HYDRANT_OBJECT);
		centerOfHydrant = hydrant.GetComponent<BoxCollider2D>().offset;
		centerOfHydrant = hydrant.GetComponent<BoxCollider2D> ().transform.TransformPoint (centerOfHydrant);
		Debug.Log ("Center of hydrant x = " + centerOfHydrant.x + ". y = " + centerOfHydrant.y);
	}
	
	// Update is called once per frame
	void Update () {

		if (isAppEditing) {
			EditorTouchHandler ();
		} else {
			MobileTouchHandler ();
		}

	}

	void MobileTouchHandler() {

		int fingerCount = 0;
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				fingerCount++;
			print (string.Format("Finger id = {0}, x = {1}, y = {2}", touch.fingerId, touch.position.x, touch.position.y));
		}
		if (fingerCount > 0)
			print("User has " + fingerCount + " finger(s) touching the screen");
	}

	void EditorTouchHandler() {

		if (Input.GetMouseButtonDown (0)) {
			
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			HandleInitialTouchPosition (pos);
		}

		if (touchStartedOnHydrant && Input.GetMouseButton(0)) {

			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			HandleTouchWhilePressed (pos);

		}
	}

	void HandleInitialTouchPosition(Vector2 pos) {

		RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
		// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
		if(hitInfo)
		{
			if(hitInfo.transform.gameObject.Equals(hydrant)) {

				Debug.Log("Hit the hydrant");
				touchStartedOnHydrant = true;
				// Here you can check hitInfo to see which collider has been hit, and act appropriately.
			}
		}
	}

	void HandleTouchWhilePressed(Vector2 pos) {
		// Assume the touch originated at the hydrant already.
		RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
		bool onHydrant = false;
		if(hitInfo)
		{
			if (hitInfo.transform.gameObject.Equals (hydrant)) {
				onHydrant = true;
				// Here you can check hitInfo to see which collider has been hit, and act appropriately.
			}
		}
		if (onHydrant) {
			Debug.Log ("On hydrant");
		} else {
			Debug.Log ("Off Hydrant");
		}

	}

}
