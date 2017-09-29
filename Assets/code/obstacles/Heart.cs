using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
	
	void Start()
	{
	}

	private void OnMouseDown()
	{
		GameObject.Find("Player").GetComponent<PlayerStatus>().AddLife ();
		GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition (transform.position.y);
		Destroy (this.gameObject);
	}
}
