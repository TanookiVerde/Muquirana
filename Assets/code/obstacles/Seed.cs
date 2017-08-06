using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Hook {

	private void OnMouseDown(){
		if( OnClick() ){
			GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition( MoveTo() );
			GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity*2;
		}
	}
}
