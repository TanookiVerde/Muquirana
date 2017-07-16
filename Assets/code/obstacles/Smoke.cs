using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {

	private SpriteRenderer mySR;
	public float duration;
	public float maxSize;

	private void Start(){
		mySR = GetComponent<SpriteRenderer>();
		StartCoroutine( ChangeScaleAndOpacity() );
	}
	private IEnumerator ChangeScaleAndOpacity(){
		Color target = new Color(mySR.color.r,mySR.color.g,mySR.color.b,0);
		float a = mySR.color.a;
		while(a > 0.1f){
			a = Mathf.Lerp(a, 0, 0.005f);
			mySR.color = new Color(mySR.color.r,mySR.color.g,mySR.color.b,a);
			transform.localScale *= 1.005f;
			yield return new WaitForEndOfFrame();
		}
	}
}
