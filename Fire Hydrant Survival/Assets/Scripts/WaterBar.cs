using UnityEngine;
using System.Collections;

public class WaterBar : MonoBehaviour {

	float maxWaterLvl = 100.0f;
	float currWaterLvl = 100.0f;



	void OnGUI()
	{
		Rect position = new Rect (10, Screen.height - 110, 20, 100);
		DrawQuad (position, new Color(0.5f,0.5f,0.5f) );
	} 

	void Update()
	{
		if (currWaterLvl < 0) {
			currWaterLvl = 0;
		}

		if (currWaterLvl > maxWaterLvl) {
			currWaterLvl = maxWaterLvl;
		}



	}

	void DrawQuad(Rect position, Color color) {
		
	}

}
