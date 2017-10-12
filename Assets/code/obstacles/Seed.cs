using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Hook {
	[SerializeField] private bool badSeed;
	public bool clicked = false;
	[SerializeField] private GameObject boomPrefab;

	private GameObject player;
	[SerializeField] private float velocity;

	private float time = 0;
	[SerializeField] private float maxTime = 0.1f;

	private void Start(){
		time = 0;
		player = GameObject.Find("Player");
	}
	private void OnMouseDown(){
		if( !clicked && !badSeed){
			clicked = true;
			GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition( MoveTo() );
			GetComponent<Rigidbody2D>().velocity = Vector3.right*velocity*2;
			transform.up *= -1;
		}
	}
	public void EndLife(){
		var b = Instantiate(boomPrefab,transform.position,Quaternion.identity);
		Destroy(b,0.5f);
		Destroy(this.gameObject);
	}
	private void Update(){
		time += Time.deltaTime;
		if(!clicked && time < maxTime) FollowPlayer();
	}
	private void FollowPlayer(){
		GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized*velocity;
		var up = (player.transform.position - transform.position);
		transform.right = -up;
	}
}
