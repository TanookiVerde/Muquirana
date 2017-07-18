using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

	[SerializeField] private int initialHeartsNumber, maxHeartsNumber;
	[SerializeField] private GameObject heartUI;
	[SerializeField] private Text moneyText;
	[SerializeField] private int money;
	[SerializeField] private GameObject grid;

	private void Start(){
		InitHeartsNumber();
	}
	public void AddLife(){
		if(grid.transform.childCount < maxHeartsNumber){
			Instantiate(heartUI, Vector3.zero, Quaternion.identity, grid.transform);
		}
	}
	public void LoseLife(){
		if(grid.transform.childCount > 0){
 			GameObject.Destroy(grid.transform.GetChild(0).gameObject);
			StartCoroutine( isDead() );
 		}
	}
	private void InitHeartsNumber(){
		for(int i = 0; i < initialHeartsNumber; i++){
			AddLife();
		}
	}
	private IEnumerator isDead(){
		yield return new WaitForEndOfFrame();
		if(grid.transform.childCount == 0){
			StartCoroutine( GameObject.Find("Player").GetComponent<LevelManager>().GameOver() );
		}
	}
}
