using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DogState {INACTIVE, MOVING, PEEING, DEFEATED, ESCAPE, HIT};
enum DogAction {LEFT, RIGHT, UP, DOWN, IDLE};

public class DogBehavior : MonoBehaviour {

	private static string STATE_VARIABLE = "State";

	private Animator anim;
	public DogState myState;
	DogAction currentAction;

	public DogType myType;

	GameObject fireHydrant;
	GameObject me;

	BoxCollider2D fireHydrantCollider;
	Vector2 centerOfHydrant;
	Vector2 myCenter;

	Vector2 goalDirection;

	float timeForAction;
	float timeLeftForAction = 0;

	float moveMagnitude = 5.0f;

	Vector2 hitDirection;
	float hitMagnitude = 3.0f;

	float hitTime = 0.2f;
	float hitDuration = 0.0f;

	float peeAnimationTime;

	MoraleManager moraleManager;

	bool invincible = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		myState = DogState.MOVING;

		fireHydrant = GameObject.Find (Constants.OBJ_FIRE_HYDRANT);
		fireHydrantCollider = fireHydrant.GetComponent<BoxCollider2D> ();
		centerOfHydrant = fireHydrantCollider.offset;
		centerOfHydrant = fireHydrantCollider.transform.TransformPoint (centerOfHydrant);

		foreach (Transform child in transform) {
			if (child.name == Constants.UI_MORALE_CANVAS) {
				Debug.Log ("Canvas found");
				moraleManager = child.gameObject.GetComponent < MoraleManager>() as MoraleManager;
				moraleManager.SetDogType (DogType.SMALL_DOG);
			}
		}


		me = this.gameObject;

	}

	public void UnleashDog() {
		myState = DogState.MOVING;
	}

	void FixedUpdate() {

		myCenter = this.gameObject.GetComponent<Transform> ().position;

		if (moraleManager.defeated) {
			//Debug.Log ("Dog was defeated");
			invincible = true;
			myState = DogState.DEFEATED;
		}

		switch (myState) {

		case DogState.HIT:
			{
				// Move in direction of hit for some duration.
				hitDuration -= Time.deltaTime;
				if (hitDuration <= 0) {
					myState = DogState.MOVING;
				} else {
					Move (hitDirection, hitMagnitude);
				}

			}
			break;
		case DogState.MOVING:
			{
				if (true) {
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

			}
			break;
		case DogState.PEEING:
			{
				peeAnimationTime -= Time.deltaTime;
				if (peeAnimationTime <= 0) {
					peeAnimationTime = 0;
					myState = DogState.ESCAPE;
				}

				invincible = true;

			}
			break;
		case DogState.ESCAPE:
			{
				//Debug.Log ("Escaping");
				if (myCenter.x >= centerOfHydrant.x) {
					currentAction = DogAction.RIGHT;
					MoveRight ();
				} else {
					currentAction = DogAction.LEFT;
					MoveLeft ();
				}

				SetAnimation ();

			}
			break;
		case DogState.DEFEATED:
			{
				currentAction = DogAction.UP;
				MoveUp ();
				SetAnimation ();
			}
			break;
		}
	}

	void OnBecameInvisible() {
		if (myState == DogState.ESCAPE || myState == DogState.DEFEATED) {
			Debug.Log ("Inactive");
			myState = DogState.INACTIVE;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	float GetAnimationlength(string animName) {
		Animator anim = GetComponent<Animator>();
		float length = 0.0f;
		if (anim != null) {
			RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
			for(int i = 0; i<ac.animationClips.Length; i++)                 //For all animations
			{
				if(ac.animationClips[i].name == animName)        //If it has the same name as your clip
				{
					length = ac.animationClips[i].length;
				}
			}
		}
		return length;
	}

	void HandleActionChoice() {

		// See how far I am away from the fire hydrant.
		myCenter = me.GetComponent<Transform> ().position;
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
			float y_dir = goalDirection.y;

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

	bool CheckPeeCondition() {

		Vector2 size = fireHydrantCollider.bounds.size;

		if (myCenter.x > centerOfHydrant.x - size.x / 2 - 0.0f &&
		    myCenter.x < centerOfHydrant.x + size.x / 2 + 0.0f &&
		    myCenter.y > centerOfHydrant.y - size.y / 2 - 0.0f &&
		    myCenter.y < centerOfHydrant.y + size.y / 2 + 0.0f) {

			//Debug.Log ("Peeing");
			float lengthOfAnimation = 0.0f;
			if (myCenter.x >= centerOfHydrant.x) {
				// Pee right
				anim.SetBool (Constants.BOOL_PEE_RIGHT, true);
				lengthOfAnimation = GetAnimationlength (Constants.ANIM_PEE_RIGHT);
			} else {
				// Pee left
				anim.SetBool (Constants.BOOL_PEE_LEFT, true);
				lengthOfAnimation = GetAnimationlength (Constants.ANIM_PEE_LEFT);
			}

			peeAnimationTime = lengthOfAnimation;

			myState = DogState.PEEING;
			return true;
		}
		return false;
	}

	float ComputeProbabilityOfCorrectManeuver(float distance) {
		//float probability = 2.0f / 5.0f * 2.0f / Mathf.PI * Mathf.Atan (1 / 2 * distance) + 0.6f;
		float probability = 19.0f/20.0f;
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
			anim.SetInteger (STATE_VARIABLE, 1);
			break;
		case DogAction.UP:
			anim.SetInteger (STATE_VARIABLE, 2);
			break;
		case DogAction.RIGHT:
			anim.SetInteger (STATE_VARIABLE, 3);
			break;
		case DogAction.DOWN:
			anim.SetInteger (STATE_VARIABLE, 4);
			break;
		case DogAction.IDLE:
			anim.SetInteger (STATE_VARIABLE, 0);
			break;
		}
	}

	void MoveLeft() {
		//Debug.Log ("Move left");
		Vector2 direction = new Vector2(-1, 0);
		Move (direction, moveMagnitude);
	}

	void MoveRight() {
		//Debug.Log ("Move right");
		Vector2 direction = new Vector2(1, 0);
		Move (direction, moveMagnitude);
	}

	void MoveUp() {
		//Debug.Log ("Move up");
		Vector2 direction = new Vector2(0, 1);
		Move (direction, moveMagnitude);
	}

	void MoveDown() {
		//Debug.Log ("Move down");
		Vector2 direction = new Vector2(0, -1);
		Move (direction, moveMagnitude);
	}

	void MoveIdle() {
		//Debug.Log ("Move idle");
		Vector2 direction = new Vector2(0, 0);
		Move (direction, moveMagnitude);
	}

	void Move (Vector2 direction, float magnitude) {
		Transform transform = GetComponent<Transform> ();
		Vector2 tfrm = (Vector2) transform.position + direction * magnitude * Time.deltaTime;
		GetComponent<Transform> ().position = tfrm;
		//Debug.Log ("transform after = (" + GetComponent<Transform> ().position.x + ", " + GetComponent<Transform> ().position.y + ")");
	}

	float SampleTimeForAction() {
		return Random.Range (0.3f, 1.0f); // from 1 second to 2 seconds
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

	public void DogWasHit(Vector2 direction) {

		if (myState == DogState.MOVING && !invincible) {
			moraleManager.DogWasHit ();
			hitDirection = direction;
			myState = DogState.HIT;
			hitDuration = hitTime;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		float lengthOfAnimation = 0.0f;

		if (other.gameObject.name == Constants.OBJ_FIRE_HYDRANT) {
			Debug.Log("Hit the hydrant");
			if (myCenter.x >= centerOfHydrant.x) {
				// Pee right
				anim.SetInteger(STATE_VARIABLE, 5);
				lengthOfAnimation = GetAnimationlength (Constants.ANIM_PEE_RIGHT);
			} else {
				// Pee left
				anim.SetInteger(STATE_VARIABLE, 6);
				lengthOfAnimation = GetAnimationlength (Constants.ANIM_PEE_LEFT);
			}

			peeAnimationTime = lengthOfAnimation;

			myState = DogState.PEEING;
		} 

	}

}
