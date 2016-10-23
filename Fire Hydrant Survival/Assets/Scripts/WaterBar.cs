using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour {

	private float fillAmount;

	public float maxWaterLvl { get; set;}

	public float currWaterLvl {
		set { 
			fillAmount = Map (value, 0, maxWaterLvl, 0, 1);
			//Debug.Log ("value = " + value + " Fill amount = " + fillAmount);
		}
	}

	[SerializeField]
	private Image content;

	void Start () {

		Image tankImg = GetComponent<Image> ();
		Color temp = tankImg.color;
		temp.a = 0.5f;
		tankImg.color = temp;

	}

	void Update()
	{		
		
	}

	void FixedUpdate() {
		HandleBar ();
	}

	void HandleBar() {

		if (fillAmount != content.fillAmount) {
			content.fillAmount = fillAmount;
		}
	}

	private float Map(float value, float minValue, float maxValue, float outValueMin, float outValueMax) {
		return (value - minValue) / (maxValue - minValue) * (outValueMax - outValueMin) + outValueMin;
	}

} 
