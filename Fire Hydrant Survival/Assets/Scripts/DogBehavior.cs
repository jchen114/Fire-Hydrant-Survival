using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum DogState {MOVING, PEEING, DEFEATED};
enum DogAction {LEFT, RIGHT, UP, DOWN, IDLE};

public class DogBehavior : MonoBehaviour {

	private static string STATE_VARIABLE = "State";

	private Animator anim;
	DogState myState;
	DogAction currentAction;

	GameObject fireHydrant;
	GameObject me;

	Vector2 centerOfHydrant;
	Vector2 myCenter;

	Vector2 goalDirection;

	float timeForAction;
	float timeLeftForAction = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		myState = DogState.MOVING;

		fireHydrant = GameObject.Find (Constants.FIRE_HYDRANT_OBJECT);
		centerOfHydrant = fireHydrant.GetComponent<BoxCollider2D> ().offset;
		centerOfHydrant = fireHydrant.GetComponent<BoxCollider2D> ().transform.TransformPoint (centerOfHydrant);

		me = this.gameObject;

	}

	void FixedUpdate() {
		switch (myState) {
		case DogState.MOVING:
			{
				//Debug.Log ("Moving");
				if (timeLeftForAction > 0) {
					//Debug.Log ("Performing action");
					timeLeftForAction -= Time.deltaTime;

					switch (currentAction) {

					case DogAction.LEFT:
						MoveLeft ();
						break;
					case DogAction.UP:
						MoveUp ();
						break;
					case DogAction.RIGHT:
						MoveRight ();
						break;
					case DogAction.DOWN:
						MoveDown ();
						break;
					case DogAction.IDLE:
						MoveIdle ();
						break;
					}

				} else {
					//Debug.Log ("New Action");
					HandleActionChoice ();
				}

			}
			break;
		case DogState.PEEING:
			{
				Debug.Log ("Peeing");

			}
			break;
		case DogState.DEFEATED:
			{
				Debug.Log ("Defeated");
			}
			break;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void HandleActionChoice() {

		// See how far I am away from the fire hydrant.
		myCenter = me.GetComponent<BoxCollider2D> ().offset;
		myCenter = me.GetComponent<BoxCollider2D> ().transform.TransformPoint (myCenter);
		goalDirection = centerOfHydrant - myCenter;
		float distanceToGoal = goalDirection.magnitude;
		goalDirection.Normalize ();

		// Compute probability of correct move or evasive maneuver
		float p_correctMove = ComputeProbabilityOfCorrectManeuver (distanceToGoal);

		DogAction actionToPerform;

		// If correct move selected
		if (Sample (p_correctMove)) {

			//Debug.Log ("Correct Action chosen");
			// Select from existing moves that move closer to the fire hydrant.
			float x_dir = goalDirection.x;
			float y_dir = goalDirection.x;

			List<DogAction> correctActions = new List<DogAction>();

			if (x_dir < 0) {
				correctActions.Add (DogAction.LEFT);
			} else if (x_dir == 0 && y_dir < 0) {
				correctActions.Add (DogAction.DOWN);
			} else if (x_dir == 0 && y_dir > 0) {
				correctActions.Add (DogAction.UP);
			} else {
				correctActions.Add (DogAction.RIGHT);
			}

			if (y_dir < 0) {
				correctActions.Add (DogAction.DOWN);
			} else if (y_dir == 0 && x_dir < 0) {
				correctActions.Add (DogAction.LEFT);
			} else if (y_dir == 0 && x_dir > 0) {
				correctActions.Add (DogAction.RIGHT);
			} else {
				correctActions.Add (DogAction.UP);
			}

			actionToPerform = ActionToChoose (correctActions);

		} else {
			// Select all moves with equal probability.
			//Debug.Log ("Evasive Maneuver chosen");
			List<DogAction> allActions = new List <DogAction> {
				DogAction.LEFT,
				DogAction.UP,
				DogAction.RIGHT,
				DogAction.DOWN,
				DogAction.IDLE
			};
			actionToPerform = ActionToChoose (allActions);
		}

		// perform action.
		currentAction = actionToPerform;
		//PrintAction ();
		SetAnimation ();

		timeForAction = SampleTimeForAction ();
		timeLeftForAction = timeForAction;
	}

	void OnCollisionEnter2D(Collision2D col) {

		if (col.gameObject.name == Constants.FIRE_HYDRANT_OBJECT) {
			myState = DogState.PEEING;
		}

		if (col.gameObject.name == Constants.WATER_SEGMENT) {
			// Get pushed back.
			Debug.Log("Got hit by water");

		}

	}

	float ComputeProbabilityOfCorrectManeuver(float distance) {
		float probability = 2.0f / 5.0f * 2.0f / Mathf.PI * Mathf.Atan (1 / 10 * distance) + 0.6f;
		return probability;
	}

	bool Sample(float probability) {
		float random = Random.Range (0.0f, 1.0f);
		if (random <= probability) {
			return true;
		}
		return false;
	}

	DogAction ActionToChoose(List<DogAction> actions) {
		float probabilitySegments = 1.0f / actions.Count;
		float random = Random.Range (0.0f, 1.0f);
		int index = Mathf.FloorToInt (random / probabilitySegments);
		return actions[index];
	}

	void SetAnimation() {
		switch (currentAction) {
		case DogAction.LEFT:
			anim.SetInteger (STATE_VARIABLE, 2);
			break;
		case DogAction.UP:
			anim.SetInteger (STATE_VARIABLE, 3);
			break;
		case DogAction.RIGHT:
			anim.SetInteger (STATE_VARIABLE, 4);
			break;
		case DogAction.DOWN:
			anim.SetInteger (STATE_VARIABLE, 1);
			break;
		case DogAction.IDLE:
			anim.SetInteger (STATE_VARIABLE, -1);
			break;
		}
	}

	void MoveLeft() {
		//anim.SetInteger (STATE_VARIABLE, 2);
	}

	void MoveRight() {
		//anim.SetInteger (STATE_VARIABLE, 4);
	}

	void MoveUp() {
		//anim.SetInteger (STATE_VARIABLE, 3);
	}

	void MoveDown() {
		//anim.SetInteger (STATE_VARIABLE, 1);
	}

	void MoveIdle() {
		//anim.SetInteger (STATE_VARIABLE, -1);
	}

	float SampleTimeForAction() {
		return Random.Range (1.0f, 2.0f); // from 1 second to 2 seconds
	}

	void PrintAction() {
		switch (currentAction) {

		case DogAction.LEFT:
			Debug.Log ("Left");
			break;
		case DogAction.UP:
			Debug.Log ("Up");
			break;
		case DogAction.RIGHT:
			Debug.Log ("Right");
			break;
		case DogAction.DOWN:
			Debug.Log ("Down");
			break;
		case DogAction.IDLE:
			Debug.Log ("Idle");
			break;

		}
	}

}
