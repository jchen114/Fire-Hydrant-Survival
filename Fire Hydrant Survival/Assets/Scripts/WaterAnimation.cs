using UnityEngine;
using System.Collections;

public class WaterAnimation : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName (Constants.ANIM_WATER_SPLASH)) {
			// start a coroutine
			float animationTime = anim.GetCurrentAnimatorStateInfo(0).length;
			StartCoroutine (WaitForAnimationEnd (animationTime));
		}

	}

	IEnumerator WaitForAnimationEnd(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Debug.Log ("Animation done");

	}

	public void PlaySplash() {
		Debug.Log ("Play splash");
		//gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		anim.SetBool (Constants.BOOL_WATER_SPLASH, true);
	}

}
