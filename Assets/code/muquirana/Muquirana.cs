using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Muquirana : MonoBehaviour {

	[Header("Adjusts")]
	[SerializeField] private float paddingLeft;
	private Camera cameraInScene;
	

	[Header("Moving Properties")]
	[Range(0,1)]
	[SerializeField] private float velocity;

	[Header("DamageAnimation")]
	[SerializeField] private float rotationAngle;
	[SerializeField] private Color inDamageColor;
	[SerializeField] private int frameDuration;

	private SpriteRenderer mySR;

	private void Start(){
		GetCameraInScene();
		CorrectPosition();
		GetSpriteRender();
	}
	private void GetCameraInScene(){
		cameraInScene = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	private void GetSpriteRender(){
		mySR = GetComponent<SpriteRenderer>();
	}
	private void CorrectPosition(){
		//CAMERA
		float cameraProportion = cameraInScene.aspect;
		float cameraVertical = cameraInScene.orthographicSize;
		float cameraHorizontal = cameraProportion*cameraVertical;
		float offset = cameraHorizontal - this.paddingLeft;

		//POSITION
		float y = transform.position.y;
		this.transform.position = new Vector3(-offset,y,0);
	}
	public IEnumerator Move(float newY){
		float a = transform.position.y,
			  b = newY;
		while(Mathf.Abs(a - b) > 0.05){
			a = Mathf.Lerp(a, b, velocity);
			transform.position = new Vector3(transform.position.x, a, 0);
			yield return new WaitForEndOfFrame();
		}
	}
	public void ChangePosition(float newY){
		StartCoroutine( Move(newY) );
	}
	public IEnumerator DamageAnimation(){
		mySR.color = inDamageColor;
		for(int i = 0; i < frameDuration; i++){
			transform.Rotate(0,0,Time.deltaTime*rotationAngle);
			yield return new WaitForEndOfFrame();
		}
		for(int i = 0; i < frameDuration; i++){
			transform.Rotate(0,0,-Time.deltaTime*rotationAngle);
			yield return new WaitForEndOfFrame();
		}
		mySR.color = Color.white;
	}
}
