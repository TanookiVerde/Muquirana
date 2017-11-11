using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour 
{

	private ParticleSystem particleSystem;
	private BoxCollider2D playerCollider;
	private BoxCollider2D explosionArea;
	private GameObject player;

	// Use this for initialization
	void Start () 
	{
		particleSystem = GetComponent <ParticleSystem> ();
		playerCollider = GameObject.Find ("PlayerCollider").GetComponent<BoxCollider2D> ();
		explosionArea = GetComponent<BoxCollider2D> ();
		player = GameObject.Find ("Player");
	}


	public void Explode ()
	{
		particleSystem.Play ();

		if (explosionArea.IsTouching (playerCollider)) 
		{
			player.GetComponent<PlayerStatus>().LoseLife();
			StartCoroutine (player.GetComponent<Muquirana>().DamageAnimation());
		}
		Destroy (gameObject, 3.0f);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) 
		{
			print ("Player in range");
			GetComponent<Animator> ().SetTrigger ("PlayerInRange");
		}
	}
}
