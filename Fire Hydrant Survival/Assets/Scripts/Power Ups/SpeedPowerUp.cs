using UnityEngine;
using System.Collections;

public class SpeedPowerUp : PowerUp
{
	public override void Activate() {
		//Debug.Log ("Speed Power Activate");
		WaterPumper pumper = GameObject.Find (Constants.OBJ_WATER_PUMPER).GetComponent<WaterPumper> ();
		pumper.SpeedUpWater ();
	}
}

