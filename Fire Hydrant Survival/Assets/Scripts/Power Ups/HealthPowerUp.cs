using UnityEngine;
using System.Collections;

public class HealthPowerUp : PowerUp
{

	public override void Activate() {
		FireHydrant hydrant = GameObject.Find (Constants.OBJ_FIRE_HYDRANT).GetComponent<FireHydrant> ();
		hydrant.GotRestored ();
	}
}

