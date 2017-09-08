using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureList : MonoBehaviour {

	[SerializeField] private SO_PlayerData _playerData;
	[SerializeField] private List<GameObject> _itemList;

	private void Start(){
		int total = _itemList.Count;
		print(total);
		for(int i = 0;i<total;i++){
			_itemList[i].GetComponent<Selectable>().interactable = _playerData.collectedItems[i];
			print("i: "+i+"item: "+_playerData.collectedItems[i]);
		}
	}
}
