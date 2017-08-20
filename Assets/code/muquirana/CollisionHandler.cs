using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {
	[SerializeField] PlayerStatus pStatus;
	[SerializeField] Muquirana player;

	private void Start(){
		pStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
		player = GameObject.Find("Player").GetComponent<Muquirana>();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Obstacle") || other.CompareTag("Tile") || other.CompareTag("Tongue"))
		{
			pStatus.LoseLife();
			StartCoroutine( player.DamageAnimation() );
		}
		else if (other.CompareTag("Seed"))
		{
			pStatus.LoseLife();
			Destroy(other.gameObject);
			StartCoroutine( player.DamageAnimation() );
		}
	}
}
