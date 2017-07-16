using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour,IHook {

	[SerializeField] private uint resistance;
	[SerializeField] private uint maxResistance;

	private void OnMouseDown(){
		if( OnClick() ){
			GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition( MoveTo() );
		}
	}
	public bool OnClick(){
		resistance--;
		StartCoroutine( Damage() );
		if(resistance <= 0){
			StartCoroutine( Fall() );
			return true;
		}
		return false;
	}
	public float MoveTo(){
		return transform.position.y;
	}
	public void Restart(){
		resistance = maxResistance;
	}
	private IEnumerator Fall(){
		foreach(BoxCollider2D bc2D in gameObject.GetComponents<BoxCollider2D>()){
			bc2D.enabled = false;
		}
		Rigidbody2D myRB = gameObject.AddComponent<Rigidbody2D>();		
		myRB.gravityScale = 5;
		myRB.AddForceAtPosition(new Vector2(Random.Range(-3f,3f),Random.Range(-3f,3f))*50,
							    (Vector2)transform.position + Vector2.up,
								ForceMode2D.Force);
		yield return Destroy();
	}
	private IEnumerator Damage(){
		for(int i = 0;i < 5;i++){
			transform.localScale = transform.localScale*1.05f;
			yield return new WaitForEndOfFrame();
		}
		for(int i = 0;i < 5;i++){
			transform.localScale = transform.localScale/1.05f;
			yield return new WaitForEndOfFrame();
		}
	}
	private IEnumerator Destroy(){
		yield return new WaitForSeconds(3);
		GameObject.Destroy(this.gameObject);
	}
	
}
