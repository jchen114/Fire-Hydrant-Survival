using UnityEngine;
using System.Collections;

public class WaterSegment : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnBecameInvisible() {
		//Debug.Log ("Destroy myself");
		DestroyObject (gameObject);
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == Constants.DOG_TAG) {
			// Get pushed back.
			Debug.Log("Hit a dog");

		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == Constants.DOG_TAG) {
			
		}   
	} 
}
