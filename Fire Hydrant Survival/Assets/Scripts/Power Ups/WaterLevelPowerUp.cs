using UnityEngine;
using System.Collections;

public class WaterLevelPowerUp : PowerUp
{

	public override void Activate() {
		Debug.Log ("Water Power Activate");
		WaterPumper pumper = GameObject.Find (Constants.OBJ_WATER_PUMPER).GetComponent<WaterPumper> ();
		pumper.IncreaseWaterLevel ();
	}
}

