using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {

	private SpriteRenderer mySR;
	private BoxCollider2D myBC;
	public float duration;
	public float maxSize;

	private void Start(){
		mySR = GetComponent<SpriteRenderer>();
		myBC = GetComponent<BoxCollider2D>();
		StartCoroutine( ChangeScale() );
		
	}
	private IEnumerator ChangeScale(){
		while(true){
			if(transform.localScale.x < maxSize){
				transform.localScale *= 1.1f;
				myBC.size = mySR.bounds.size/4f;
				
			}else{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine( ChangeOpacity() );
	}
	private IEnumerator ChangeOpacity(){
		yield return new WaitForSeconds(2);
		Color target = new Color(mySR.color.r,mySR.color.g,mySR.color.b,0);
		float a = mySR.color.a;
		while(a > 0.01f){
			a = Mathf.Lerp(a, 0, 0.005f);
			mySR.color = new Color(mySR.color.r,mySR.color.g,mySR.color.b,a);
			yield return new WaitForEndOfFrame();
		}
		StopAllCoroutines();
		Destroy(gameObject);
	}
}
