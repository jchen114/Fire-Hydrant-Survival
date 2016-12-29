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
			End ();
		}

	}

	public void Begin() {
		spawnTime = RandomFromDistribution.RandomNormalDistribution (8, 1);
	}

	public void End() {
		Destroy (gameObject);
	}

}
