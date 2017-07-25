using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	[Header("Boss Preferences")]
	[SerializeField] protected BossType type;
	[SerializeField] protected int maxHP;
	[SerializeField] protected float actionCoolDown;

	[Header("Boss in-battle")]
	[SerializeField] protected bool isActing;
	[SerializeField] protected int actualHP;

	[Header("Animation Preferences")]
	[SerializeField] private float damageMaxScale;

	private GameObject player;

	protected IEnumerator GoToPosition(float position){
		isActing = true;
		while(!MyApproximately(transform.position.y,position)){
			transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, position, 0.2f));
			yield return new WaitForEndOfFrame();
		}
		isActing = false;
	}
	private void OnMouseDown(){
		LoseLifeOnClick();
		if(!isActing){
			//StartCoroutine( Damage() );
		}
	}
	private IEnumerator Damage(){
		isActing = true;
		Vector3 originalScale = transform.localScale;
		Vector3 target = new Vector3(damageMaxScale,damageMaxScale,damageMaxScale);
		while(!MyApproximately(transform.localScale.y,target.y)){
			transform.localScale = Vector3.Lerp(transform.localScale,target,0.5f);
			yield return new WaitForEndOfFrame();
		}
		while(!MyApproximately(transform.localScale.y,originalScale.y)){
			transform.localScale = Vector3.Lerp(transform.localScale,originalScale,0.5f);
			yield return new WaitForEndOfFrame();
		}
		isActing = false;
	}
	private float MyLerp(float a, float b, float tax, float error = 0.05f){
		if(tax > 1) tax = 1;
		float temp = a + (b-a)*tax;
		if(temp <= error){
			temp = 0;
		}
		return temp;
	}
	private bool MyApproximately(float a, float b, float limit = 0.05f){
		float temp = Mathf.Abs(a-b);
		
		if(temp > limit){
			return false;
		}else{
			return true;
		}
	}
	protected void LessCoolDown(){
		actionCoolDown -= actionCoolDown/4;
	}
	protected void LoseLifeOnClick(){
		actualHP -= 2;
	}
	protected bool PlayerInFront(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position,
						  					 Vector2.left,
											 Mathf.Infinity,
											 1<<LayerMask.NameToLayer("Player"));
		return hit.collider != null;
	}
	protected void GetPlayer(){
		player = GameObject.Find("Player");
	}
	protected float GetPlayerHeight(){
		return player.transform.position.y;
	}
}

public enum BossType{
	TICO, BARTOLOMEU, ASDRUBA, APOLLO, FREDERICO
}
