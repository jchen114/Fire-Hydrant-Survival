﻿using UnityEngine;
using System.Collections;

public class WaterPumper : MonoBehaviour {

	private bool isAppEditing;
	private bool touchStartedOnHydrant;
	private Vector2 centerOfHydrant;
	public float thrust;

	private GameObject hydrant;
	public GameObject waterSegment; // Prefab connected through inspector

	[SerializeField]
	private Stat water;

	[SerializeField]
	private float decreaseAmount;

	[SerializeField]
	private float increaseAmount;

	private bool incurredPenalty = false;

	[SerializeField]
	private float rechargePenaltyTime;

	private float penaltyTimeLeft;

	private bool shouldRecharge = true;

	[SerializeField]
	private float lerpSpeed;

	void Awake() {
		// Setup Water level
		water.waterBar = GameObject.Find(Constants.WATER_TANK).GetComponent<WaterBar>();
		water.MaxValue = 100;
		water.CurrentValue = 100;
	}

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
		centerOfHydrant = hydrant.GetComponent<BoxCollider2D> ().offset;
		centerOfHydrant = hydrant.GetComponent<BoxCollider2D> ().transform.TransformPoint (centerOfHydrant);
		Debug.Log ("Center of hydrant x = " + centerOfHydrant.x + ". y = " + centerOfHydrant.y);

	}

	// Update is called once per frame
	void Update () {

		shouldRecharge = true;

		if (incurredPenalty) {
			penaltyTimeLeft -= Time.deltaTime;
			//Debug.Log ("Penalty time left = " + penaltyTimeLeft);
			incurredPenalty = (penaltyTimeLeft <= 0) ? false: true;
			if (!incurredPenalty) {
				water.CurrentValue = Mathf.Lerp(water.CurrentValue, water.CurrentValue + increaseAmount, Time.deltaTime * lerpSpeed);
			}
		}

		if (!incurredPenalty) {
			
			if (isAppEditing) {
				EditorTouchHandler ();
			} else {
				MobileTouchHandler ();
			}
			UpdateWater ();
		}

		water.SetLevel ();
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

		Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		if (Input.GetMouseButtonDown (0)) {
			HandleInitialTouchPosition (pos);
		}

		if (touchStartedOnHydrant && Input.GetMouseButton(0)) {
			HandleTouchWhilePressed (pos);
		}

		if (touchStartedOnHydrant && Input.GetMouseButtonUp (0)) {
			HandleTouchEnd (pos);
		}
	}

	void HandleInitialTouchPosition(Vector2 pos) {

		RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
		// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
		if(hitInfo)
		{
			if(hitInfo.transform.gameObject.Equals(hydrant)) {

				//Debug.Log("Hit the hydrant");
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
			//Debug.Log ("On hydrant");
			shouldRecharge = true;
		} else {
			//Debug.Log ("Off Hydrant");
			Squirt (centerOfHydrant, Camera.main.ScreenToWorldPoint(pos));
			water.CurrentValue = Mathf.Lerp(water.CurrentValue, water.CurrentValue - decreaseAmount, Time.deltaTime * lerpSpeed);
			shouldRecharge = false;
		}

	}

	void HandleTouchEnd(Vector2 pos) {
		touchStartedOnHydrant = false; // End the touch
	}

	void Squirt(Vector2 initialPos, Vector2 endPos) {
		//Debug.Log ("Drawing water");
		Debug.DrawLine(initialPos, endPos, Color.red);

		// Get the orientation of the end pos
		Vector2 v2 = endPos - initialPos;
		float angle = 90 + Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
		//Debug.Log ("Angle = " + angle);
		Quaternion initialRotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));

		// Instantiate the water segment prefab.
		GameObject waterSeg = (GameObject) Instantiate(waterSegment, initialPos, initialRotation);
		// Apply velocity to it
		Rigidbody2D rigidBody =  waterSeg.GetComponent<Rigidbody2D>();
		Vector2 vectorDir = endPos - centerOfHydrant;
		vectorDir.Normalize();
		rigidBody.AddForce (vectorDir * thrust);
	}

	void UpdateWater() {
		
		if (water.CurrentValue <= 0) {
			penaltyTimeLeft = rechargePenaltyTime;
			incurredPenalty = true;
			Debug.Log ("Incurred penalty");
		}

		if (shouldRecharge && !incurredPenalty) {
			water.CurrentValue = Mathf.Lerp(water.CurrentValue, water.CurrentValue + increaseAmount, Time.deltaTime * lerpSpeed);
		}
	}

}

