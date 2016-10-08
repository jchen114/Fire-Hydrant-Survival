using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum DogType {SMALL_DOG, MEDIUM_DOG, BIG_DOG};

public class MoraleManager : MonoBehaviour {

	float max_morale_lvl = 100.0f;
	float current_morale_lvl;
	float lerpSpeed = 1.0f;
	float damage = 0.0f;
	Image moraleBar;

	bool dogWasHit;

	Color green = new Color(0.2f, 1.0f, 0.2f);
	Color red = new Color(1.0f ,0.2f, 0.2f);

	private DogType myDogType;

	public bool defeated;
	bool hit;

	// Use this for initialization
	void Start () {

		current_morale_lvl = max_morale_lvl;

		for (int i = 0; i < this.transform.childCount; i++) {

			if (this.transform.GetChild (i).transform.name == Constants.UI_MORALE_BAR) {
				Debug.Log ("Found the morale bar");
				moraleBar = this.transform.GetChild (i).gameObject.GetComponent<Image> ();
				moraleBar.color = green;
			}
		}

		defeated = false;

	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {

		if (hit) {

			InterpolateMorale ();
			hit = false;
		}

	}
		
	void InterpolateMorale() {
		switch (myDogType) {
		case DogType.SMALL_DOG:
			{
				damage = 20;
			}
			break;
		case DogType.MEDIUM_DOG:
			{
				damage = 10;
			}
			break;
		case DogType.BIG_DOG:
			{
				damage = 5.0f;
			}
			break;
		}

		float toLerp = current_morale_lvl - damage;


		//moraleBar.fillAmount = Mathf.Lerp (current_morale_lvl / max_morale_lvl, toLerp / max_morale_lvl, Time.deltaTime);
		moraleBar.fillAmount = toLerp/max_morale_lvl;

		current_morale_lvl = toLerp;

		if	 (current_morale_lvl <= 0) {
			current_morale_lvl = 0;
			defeated = true;
		}

		float moralePercentage = current_morale_lvl / max_morale_lvl;

		Debug.Log ("morale lvl = " + current_morale_lvl);

		Color interpolatedColor = new Color(
			moralePercentage * (green.r - red.r) + red.r, 
			moralePercentage * (green.g - red.g) + red.g,
			moralePercentage * (green.b - red.b) + red.b
		);

		moraleBar.color = interpolatedColor;

	} 

	public void SetDogType(DogType type) {
		myDogType = type;
	}

	public void DogWasHit() {
		hit = true;
	}
			

}
