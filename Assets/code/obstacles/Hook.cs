using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
	int resistance = 1;

	public bool OnClick(){
		resistance--;
		//StartCoroutine( Damage() );
		if(resistance <= 0){
			StartCoroutine( Fall() );
			return true;
		}
		return false;
	}
	public float MoveTo(){
		return transform.position.y;
	}
	private IEnumerator Fall(){
		Rigidbody2D myRB;
		foreach(BoxCollider2D bc2D in gameObject.GetComponents<BoxCollider2D>()){
			bc2D.enabled = false;
		}
		if(GetComponent<Rigidbody2D>() == null){
			//ANY OTHER
			myRB = gameObject.AddComponent<Rigidbody2D>();
			myRB.gravityScale = 5;
			myRB.AddForceAtPosition(new Vector2(Random.Range(-3f,3f),Random.Range(-3f,3f))*50,
									(Vector2)transform.position + Vector2.up,
									ForceMode2D.Force);
		}else{
			//SEED
			myRB = GetComponent<Rigidbody2D>();
			myRB.velocity = myRB.velocity;
		}
		yield return Destroy();
	}
	private IEnumerator Destroy(){
		yield return new WaitForSeconds(3);
		GameObject.Destroy(this.gameObject);
	}
}
