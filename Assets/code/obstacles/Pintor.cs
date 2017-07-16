using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Pintor : MonoBehaviour,IHook {

	[Header("Movement Properties")]
	[Range(0,1)]
	[SerializeField] private float velocity;
	private Vector3 targetPosition;

	[Header("Smoke Properties")]
	[SerializeField] private GameObject smokePrefab;
	[SerializeField] private float duration;
	[SerializeField] private bool RandomColor;
	[SerializeField] private Color initialColor;
	[SerializeField] private Color finalColor;
	[SerializeField] private Color actualColor;

	[Header("Resistance")]
	[SerializeField] private int resistance;

	private GameObject cameraInScene;
	private float cameraHeight;
	private float cameraWidth;

	private void Start(){
		actualColor = initialColor;
		GetCamera();
		GetTargetPosition();
		StartCoroutine( Movement() );
	}
	private void GetCamera(){
		cameraInScene = GameObject.Find("Main Camera");
		cameraHeight = cameraInScene.GetComponent<Camera>().orthographicSize*2;
		cameraWidth = cameraInScene.GetComponent<Camera>().aspect*cameraHeight;

		Debug.Log(cameraHeight+"/"+cameraWidth);
	}
	private void NewSmoke(){
		GameObject go = Instantiate(smokePrefab,transform.position + Vector3.right*2,Quaternion.identity);
		actualColor = Vector4.Lerp(actualColor,finalColor,0.25f);
		go.GetComponent<SpriteRenderer>().color = actualColor;
	}
	private IEnumerator Movement(){
		float x,y,a,b;
		x = transform.position.x; y = transform.position.y;
		a = targetPosition.x; b = targetPosition.y;
		StartCoroutine( SmokeHandler() );
		while(a != b || y != b){
			x = Mathf.Lerp(x, a, velocity);
			y = Mathf.Lerp(y, b, velocity);
			transform.position = new Vector3(x,y,0);
			yield return new WaitForEndOfFrame();
		}
	}
	private IEnumerator SmokeHandler(){
		GameObject player = GameObject.Find("Player");
		while(player.transform.position.x < this.transform.position.x){
			yield return new WaitForSeconds(duration);
			NewSmoke();
		}
	}
	private void GetTargetPosition(){
		Vector3 rand = new Vector3(Random.Range(-60f,-30f),Random.Range(-5f,5f),0);
		targetPosition = transform.position + rand;
	}
	public float MoveTo(){
		return transform.position.y;
	}
	private void OnMouseDown(){
		if( OnClick() ){
			GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition( MoveTo() );
		}
	}
	public bool OnClick(){
		resistance--;
		StartCoroutine( Damage() );
		if(resistance <= 0){
			Fall();
			return true;
		}
		return false;
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
	private void Fall(){
		StopAllCoroutines();
		GetComponent<BoxCollider2D>().enabled = false;
		Rigidbody2D myRB = gameObject.AddComponent<Rigidbody2D>();		
		myRB.gravityScale = 5;
	}

}
