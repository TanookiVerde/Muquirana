using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Pintor : MonoBehaviour,IHook {

	[Header("Movement Properties")]
	[SerializeField] private float velocity;
	private Vector3 targetPosition;

	[Header("Smoke Properties")]
	[SerializeField] private GameObject smokePrefab;
	[SerializeField] private float duration;
	[SerializeField] private bool randomColor;
	[SerializeField] private Color initialColor;
	[SerializeField] private Color finalColor;
	[SerializeField] private Color actualColor;

	[Header("Resistance")]
	[SerializeField] private int resistance;

	private GameObject cameraInScene;
	private float cameraHeight;
	private float cameraWidth;

	private bool acted;

	private void Start(){
		IfRandomColor();
		actualColor = initialColor;
		GetCamera();
		StartCoroutine( Fly() );
		StartCoroutine( SmokeHandler() );
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
	private IEnumerator SmokeHandler(){
		GameObject player = GameObject.Find("Player");
		while(true){
			yield return new WaitForSeconds(duration);
			NewSmoke();
		}
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
	private IEnumerator Fly(){
		float time = 0;
		while(true){
			time += Time.deltaTime;
			transform.position += new Vector3(-velocity,Mathf.Sin(time)*2,0)*Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
	private void IfRandomColor(){
		if(randomColor){
			initialColor = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
			finalColor = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
		}
	}
}
