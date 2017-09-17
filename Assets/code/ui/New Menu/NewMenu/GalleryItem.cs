using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItem : MonoBehaviour {

	public void ShowDescription(){
		GameObject.Find("Text_Box").transform.GetChild(0).GetComponent<Text>().text = GetComponent<Gem>().gemData.name;
		GameObject.Find("Text_Box").transform.GetChild(1).GetComponent<Text>().text = GetComponent<Gem>().gemData.description;
	}
}
