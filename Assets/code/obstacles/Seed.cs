using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Hook {
	public bool clicked = false;

	private void OnMouseDown(){
		if( !clicked ){
			clicked = true;
			GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition( MoveTo() );
			GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity*2;
		}
	}
}
