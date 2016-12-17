using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	private float spawnTime;

	// Use this for initialization
	void Start () {
		


	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		spawnTime -= Time.deltaTime;

		if (spawnTime <= 0) {
			Destroy (this);
		}

	}


	public void Begin() {
		spawnTime = Random.Range (3.0f, 6.0f);
	}

}
