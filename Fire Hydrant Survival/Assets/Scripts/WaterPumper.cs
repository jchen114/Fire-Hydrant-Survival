using UnityEngine;
using System.Collections;

public enum PumpState {INACTIVE, ACTIVE};

public class WaterPumper : MonoBehaviour, IManageable {

	private const float MIN_THRUST = 750.0f;
	private const float MIN_FREQ = 0.16f;

	private float timeBetweenSquirts = 0.16f;

	private bool isAppEditing;
	private bool touchStartedOnHydrant;
	private Vector2 centerOfHydrant;
	public float thrust;

	private float max_thrust = 1500;
	private float speedup = 50;

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

	private float timeSpent;

	private float min_time = 0.08f;
	private float dec_amount = 0.02f;

	PumpState myState = PumpState.INACTIVE;

	#region UNITY

	void Awake() {
		// Setup Water level
		water.waterBar = GameObject.Find(Constants.OBJ_WATER_TANK).GetComponent<WaterBar>();
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

		hydrant = GameObject.Find (Constants.OBJ_FIRE_HYDRANT);
		centerOfHydrant = hydrant.GetComponent<BoxCollider2D> ().offset;
		centerOfHydrant = hydrant.GetComponent<BoxCollider2D> ().transform.TransformPoint (centerOfHydrant);
		Debug.Log ("Center of hydrant x = " + centerOfHydrant.x + ". y = " + centerOfHydrant.y);

		myState = PumpState.INACTIVE;

		timeSpent = timeBetweenSquirts;

	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {


		switch (myState) {
		case PumpState.ACTIVE:
			{
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
			break;
		case PumpState.INACTIVE:
			{
				// Don't do anything
			}
			break;
		}

	}

	#endregion

	#region TOUCH_HANDLERS

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
			timeSpent -= Time.deltaTime;
			if (timeSpent <= 0.0f) {
				Squirt (centerOfHydrant, Camera.main.ScreenToWorldPoint (pos));
				water.CurrentValue = Mathf.Lerp (water.CurrentValue, water.CurrentValue - decreaseAmount, Time.deltaTime * lerpSpeed);
				shouldRecharge = false;
				timeSpent = timeBetweenSquirts;
			}
		}

	}

	void HandleTouchEnd(Vector2 pos) {
		touchStartedOnHydrant = false; // End the touch
	}

	#endregion

	#region WATER

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

	#endregion

	#region INTERFACE

	public void Reset() {
		water.MaxValue = 100;
		water.CurrentValue = 100;
		timeBetweenSquirts = MIN_FREQ;
		thrust = MIN_THRUST;
	}

	public void Activate() {
		myState = PumpState.ACTIVE;
	}

	public void Deactivate() {
		myState = PumpState.INACTIVE;
	}

	public void IncreaseFrequency() {
		// TODO
		timeBetweenSquirts -= dec_amount;
		Debug.Log ("Increase Frequency");
		if (timeBetweenSquirts <= min_time) {
			timeBetweenSquirts = min_time;
		}
	}

	public void SpeedUpWater() {
		// TODO
		thrust += speedup;
		Debug.Log ("Increase thrust");
		if (thrust >= max_thrust) {
			thrust = max_thrust;
		}
	}

	#endregion

}

