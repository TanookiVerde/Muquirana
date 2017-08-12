using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Hook {

	private void OnMouseDown()
	{
		GameObject.Find("Player").GetComponent<PlayerStatus>().AddLife ();
		GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition( MoveTo() );
		Destroy (this.gameObject);
	}
}
