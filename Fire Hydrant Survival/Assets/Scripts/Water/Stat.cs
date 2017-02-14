using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class Stat
{
	public WaterBar waterBar;
	public Text levelText;

	private float maxValue;
	[SerializeField]
	public float MaxValue {
		get {
			return maxValue;
		}
		set { 
			this.maxValue = value;
			waterBar.maxWaterLvl = maxValue;
		}
	}

	private float currentValue;
	[SerializeField]
	public float CurrentValue {
		get { return currentValue; }

		set {
			this.currentValue = Mathf.Clamp(value, 0 , MaxValue);
			waterBar.currWaterLvl = currentValue;

		}
	}

	public void SetLevel() {

		// Convert level percentage to units.
		levelText.text = Mathf.RoundToInt(currentValue).ToString();
	}

}

