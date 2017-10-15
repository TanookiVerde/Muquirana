using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatcher : MonoBehaviour {
	LevelManager lManager;
	[SerializeField] private SO_PlayerData playerData;

	private void Start(){
		lManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}
	private void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Gem")){
			NewGem(other.gameObject);
		}else if(other.CompareTag("Heart")){
			NewGem(other.gameObject,true);
		}
	}
	private void PlayAnimation(IItem item){
		Debug.Log("Play_Animation");
		StartCoroutine( item.CatchAnimation() );
	}
	private void NewGem(GameObject gem,bool heart = false){
		IItem item = gem.GetComponent<IItem>();
		item.Destroy();
		item.Effect();
		lManager.collectedItems.Add(gem.gameObject);
		
		if(!heart){
			playerData.collectedItems[gem.GetComponent<Gem>().gemData.itemType.GetHashCode()] = true;
			lManager.totalMoney += item.GetValue();
			StartCoroutine ( lManager.UpdateMoneyText() );
			return;
		}

		GameObject.Find("Player").GetComponent<PlayerStatus>().AddLife();
	}
}
