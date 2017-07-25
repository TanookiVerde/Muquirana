using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour {

	private void OnMouseDown(){
		GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity*2;
	}
}
