﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartolomeu : Boss {

	[Header("General Variables")]
	[SerializeField] private float appearSpeed = 20;
	[SerializeField] private float movementSpeed = 50;
	[SerializeField] private float swapSpeed = 50;

	[Header("Attack Preferences")]
	[SerializeField] private GameObject eggPrefab, bombPrefab;
	[SerializeField] private float bombIntensity, bombRange;
	[SerializeField] private GameObject upperLimit, lowerLimit;

	[Header("Animations Preferences")]
	[SerializeField] private float maxScale;
	[SerializeField] private float growingTax;
	[SerializeField] private GameObject safePoint;

	[Header("Appearing Animation")]
	[SerializeField] private GameObject bossTitle;


	public bool defeated;

	private void Start()
	{
		//Instantiate(bossTitle);
		GetPlayer();
		StartCoroutine( Appear() );
		//GameObject.Find("Player").GetComponent<Muquirana>().changePosDelegate += MoveToPlayer;
	}

	public IEnumerator Appear()
	{
		print ("Appear");
		transform.position = safePoint.transform.position;

		yield return new WaitForSeconds(2);

		while(transform.position.x != upperLimit.transform.position.x){
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,upperLimit.transform.position.x,movementSpeed*Time.deltaTime),transform.position.y,transform.position.z);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(2);
		//Texto dizendo "boss: ###"
		yield return Loop();
	}

	private IEnumerator Loop()
	{
		print ("Loop");
		while(!defeated)
		{
			if(!isActing)
			{
				yield return PlaceEggs ();
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator PlaceEggs ()
	{
		print ("Place eggs");
		isActing = true;

		int egg_amount = Random.Range(2,4); // Sorteia o número 2 ou 3 para definir quantos ovos ele vai botar
		float offset = (upperLimit.transform.position.y - lowerLimit.transform.position.y) / (float) egg_amount;
		
		int i = 0;
		for (; i < egg_amount; i++)
		{
			Vector3 newPosition = new Vector3 (upperLimit.transform.position.x, upperLimit.transform.position.y - offset * i, 10f);
			yield return MoveToPosition (transform, newPosition);
			GameObject egg = (GameObject) Instantiate (eggPrefab, newPosition, Quaternion.identity);
			egg.GetComponent<Egg>().SetEggAmount (egg_amount);
			egg.transform.SetParent (transform.parent);
		}

		Vector3 bossPosition = new Vector3 (upperLimit.transform.position.x, upperLimit.transform.position.y - offset * i, 10f);
		yield return MoveToPosition (transform, bossPosition);
		GameObject bossEgg = (GameObject) Instantiate (eggPrefab, bossPosition, Quaternion.identity);
		bossEgg.transform.SetParent (transform);
		bossEgg.GetComponent<Egg>().SetBossEgg ();
		transform.GetComponent<SpriteRenderer>().enabled = false;

		yield return new WaitForSeconds(1);
		yield return SwapEggs ();
	}

	private IEnumerator SwapEggs ()
	{
		print ("Swap eggs");

		List <Transform> eggs = new List <Transform> ();
		for (int i = 0; i < transform.parent.childCount; i++)
		{
			eggs.Add (transform.parent.GetChild (i));
		}

		// DEBUG //
		int swapMaxAmount = 5;
		///////////

		for (int i = 0; i < swapMaxAmount; i++)
		{

			int firstEgg = Random.Range (0, eggs.Count);
			int secondEgg = firstEgg;
			while (secondEgg == firstEgg)
			{
				secondEgg = Random.Range (0, eggs.Count);
			}


			yield return SwapPositions (eggs[firstEgg], eggs[secondEgg]);
		}

		yield return HatchEggs (eggs);
	}


	private IEnumerator HatchEggs (List <Transform> eggs)
	{
		print ("Hatch eggs");

		for (int i = 0; i < eggs.Count; i++)
		{
			if (eggs[i].CompareTag ("Egg"))
			{
				eggs[i].GetComponent<Egg>().Hatch();
			}

			else if (eggs[i].CompareTag ("Boss"))
			{
				eggs[i].GetComponentInChildren<Egg>().Hatch();
			}
		}

		// BOSS ATTACK

		yield return new WaitForEndOfFrame();
	}

	private IEnumerator MoveToPosition (Transform objTransform, Vector3 newPosition, float error = 0.01f)
	{
		while ((objTransform.position-newPosition).magnitude > error)
		{
			transform.position = Vector3.MoveTowards (transform.position, newPosition, movementSpeed*Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator SwapPositions (Transform transform_a, Transform transform_b, float error = 0.01f)
	{
		Vector3 initialPos_a = transform_a.position;
		Vector3 initialPos_b = transform_b.position;
		while ((transform_a.position-initialPos_b).magnitude > error)
		{
			transform_a.position = Vector3.MoveTowards (transform_a.position, initialPos_b, swapSpeed*Time.deltaTime);
			transform_b.position = Vector3.MoveTowards (transform_b.position, initialPos_a, swapSpeed*Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

/*	private IEnumerator ShootLaser (ScreenPosition screenPos, bool triggerCooldown = true)
	{
		isActing = true;

		yield return Shoot();

		// Não sei se é necessário, fiz por precaução no caso de chamar duas vezes quando 2 pássaros atirarem
		if (triggerCooldown)
			yield return new WaitForSeconds(actionCoolDown);

		isActing = false;
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
	
	private void OnTriggerEnter2D(Collider2D other){
		Debug.Log(other.gameObject);
		if(other.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0){
			LoseLifeOnSeed();
		}
	}
	*/
}
