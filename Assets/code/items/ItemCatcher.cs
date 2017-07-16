using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatcher : MonoBehaviour {
	LevelManager lManager;

	private void Start(){
		lManager = GameObject.Find("Player").GetComponent<LevelManager>();
	}
	private void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Gem")){
			NewGem(other.gameObject);
		}
	}
	private void PlayAnimation(IItem item){
		Debug.Log("Play_Animation");
		StartCoroutine( item.CatchAnimation() );
	}
	private void NewGem(GameObject gem){
		IItem item = gem.GetComponent<IItem>();
		item.Destroy();
		lManager.collectedItems.Add(gem.gameObject);
		lManager.totalMoney += item.GetValue();
		StartCoroutine ( lManager.UpdateMoneyText() );
	}
}
