using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tico : Boss {
	[Header("Attack Preferences")]
	[SerializeField] private GameObject seedPrefab,badSeedPrefab;
	[SerializeField] private float shootIntensity;
	[SerializeField] private GameObject upperLimit, lowerLimit;

	[Header("Animations Preferences")]
	[SerializeField] private float maxScale;
	[SerializeField] private float growingTax;
	[SerializeField] private GameObject safePoint;

	[Header("Appearing Animation")]
	[SerializeField] private GameObject bossTitlePrefab;

	public bool defeated;

	private void Start(){
		GameObject bossTitle = (GameObject) Instantiate(bossTitlePrefab);
		bossTitle.GetComponent<BossTitle> ().SetBossName ("Tico");
		GetPlayer();
		StartCoroutine( Appear() );
		//GameObject.Find("Player").GetComponent<Muquirana>().changePosDelegate += MoveToPlayer;
	}

	private IEnumerator SingleShoot(){
		isActing = true;

		yield return Shoot();
		yield return new WaitForSeconds(actionCoolDown);

		isActing = false;
	}
	private IEnumerator Loop(){
		while(!defeated){
			if(!PlayerInFront() && !isActing){
				yield return GoToPosition( GetPlayerHeight() );
			}else if(!isActing){
				int rand = Random.Range(0,4);
				if(rand > 2){
					yield return SingleShoot();
				}else if (rand > 1){
					yield return TripleShoot();
				}else if (rand > 0){
					yield return ColumnShootUp();
				}else{
					yield return ColumnShootDown();//nunca eh acessado
				}
			}
		}
	}
	private IEnumerator Shoot(float tax = 0.2f){

		float originalScale = transform.localScale.x;
		//AUMENTA TAMANHO
		Vector3 target = new Vector3(transform.localScale.x,maxScale,transform.localScale.z);
		while(!Mathf.Approximately(transform.localScale.y,target.y)){
			transform.localScale = Vector3.Lerp(transform.localScale,target,tax);
			yield return new WaitForEndOfFrame();
		}
		if(Random.Range(0,10)>1){
			Instantiate(seedPrefab,transform.position,Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(Vector2.left*shootIntensity);
		}else{
			Instantiate(badSeedPrefab,transform.position,Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(Vector2.left*shootIntensity);
		}
		
		//DIMINUI TAMANHO (VOLTAND PARA O TAMANHO ORIGINAL)
		target = new Vector3(transform.localScale.x,originalScale,transform.localScale.z);
		while(!Mathf.Approximately(transform.localScale.y,target.y)){
			transform.localScale = Vector3.Lerp(transform.localScale,target,tax);
			yield return new WaitForEndOfFrame();
		}
	}
	private IEnumerator TripleShoot(){
		isActing = true;

		for(int i = 0; i < 3;i++){
			yield return Shoot(0.4f);
		}
		yield return new WaitForSeconds(actionCoolDown);
		isActing = false;
	}
	private IEnumerator ColumnShootUp(){
		isActing = true;
		float offset = upperLimit.transform.position.y - lowerLimit.transform.position.y;
		yield return GoToPosition( upperLimit.transform.position.y );

		for(int i=1; i<6; i++){
			yield return Shoot(0.4f);
			yield return GoToPosition( upperLimit.transform.position.y - (offset/5)*i );
		}
		isActing = false;
	}
	private IEnumerator ColumnShootDown(){
		isActing = true;
		float offset = upperLimit.transform.position.y - lowerLimit.transform.position.y;
		yield return GoToPosition( lowerLimit.transform.position.y );

		for(int i=1; i<6; i++){
			yield return Shoot(0.4f);
			yield return GoToPosition( lowerLimit.transform.position.y + (offset/5)*i );
		}
		isActing = false;
	}
	private void LoseLifeOnSeed(){
		actualHP -= 5;
		UpdateHealthBar();
	}
	public void MoveToPlayer(float f){
		StartCoroutine( WaitSecondsToMove(0.2f) );
	}
	private IEnumerator WaitSecondsToMove(float time){
		yield return new WaitForSeconds(time);
		StartCoroutine( GoToPosition(GetPlayerHeight()) );
	}
	public IEnumerator Appear(){
		transform.position = safePoint.transform.position;

		yield return new WaitForSeconds(2);

		while(transform.position.x != upperLimit.transform.position.x){
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,upperLimit.transform.position.x,10f*Time.deltaTime),transform.position.y,transform.position.z);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(2);
		//Texto dizendo "boss: ###"
		yield return Loop();
	}
	private void OnTriggerEnter2D(Collider2D other){
		Debug.Log(other.gameObject);
		if(other.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0){
			LoseLifeOnSeed();
		}
	}
}
