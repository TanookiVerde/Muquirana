using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, IItem {
	public SOGem gemData;

	public void Effect(){
		return;
	}
	public IEnumerator CatchAnimation(){
		yield return new WaitForEndOfFrame();
	}
	public void Destroy(){
		AudioManager a = GameObject.Find("MusicHandler").GetComponent<AudioManager>();
		a.PlaySound(Sounds.LEAF_GEM);
		//GameObject.Destroy(this.gameObject);
		transform.position = Vector3.zero;
		transform.parent = null;
	}
	public int GetValue(){
		return gemData.value;
	}
	public string GetDescription(){
		return gemData.description;
	}
	public IEnumerator OnTouch(){
		int frameDuration = 5;
		float rotationAngle = 30;

		for(int i = 0; i < frameDuration; i++){
			transform.Rotate(0,0,Time.deltaTime*rotationAngle);
			yield return new WaitForEndOfFrame();
		}
		for(int i = 0; i < frameDuration; i++){
			transform.Rotate(0,0,-Time.deltaTime*rotationAngle);
			yield return new WaitForEndOfFrame();
		}
	}
	public IEnumerator OnFall(){
		Rigidbody2D temp = gameObject.AddComponent<Rigidbody2D>();
		temp.gravityScale = 5;
		temp.AddForceAtPosition(new Vector2(Random.Range(-3f,3f),Random.Range(-3f,3f))*50,
							    (Vector2)transform.position + Vector2.up,
								ForceMode2D.Force);
		transform.parent = null;
		yield return new WaitForEndOfFrame();
	}
}
