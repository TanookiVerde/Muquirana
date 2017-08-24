using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Hook {

	[SerializeField] private float lifeTime = 2f;

	private IEnumerator AutoDestroy (float time)
	{
		yield return new WaitForSeconds (time);
		Destroy (this.gameObject);
	}

	void Start()
	{
		StartCoroutine (AutoDestroy (lifeTime));
	}

	private void OnMouseDown()
	{
		GameObject.Find("Player").GetComponent<PlayerStatus>().AddLife ();
		GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition( MoveTo() );
		Destroy (this.gameObject);
	}
}
