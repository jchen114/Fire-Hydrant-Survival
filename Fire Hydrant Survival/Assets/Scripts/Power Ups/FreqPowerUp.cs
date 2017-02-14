using UnityEngine;
using System.Collections;

public class FreqPowerUp : PowerUp
{

	public override void Activate() {
		//Debug.Log ("Frequency Power Activate");
		WaterPumper pumper = GameObject.Find (Constants.OBJ_WATER_PUMPER).GetComponent<WaterPumper> ();
		pumper.IncreaseFrequency ();
	}
}

