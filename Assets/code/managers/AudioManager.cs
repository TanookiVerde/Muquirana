using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds{
	BLOCK_DESTROY, GET_ITEM, HITTED, CHANGE_SPOT
}
public class AudioManager : MonoBehaviour {

	[SerializeField] private AudioClip blockDestroy;
	[SerializeField] private AudioClip getItem;
	[SerializeField] private AudioClip hitted;
	[SerializeField] private AudioClip changeSpot;

	private AudioSource myAS;

	private void Start(){
		myAS = GetComponent<AudioSource>();
	}

	public void PlaySound(Sounds sound){
		if(sound == Sounds.CHANGE_SPOT){
			myAS.PlayOneShot(changeSpot);
		}else if(sound == Sounds.BLOCK_DESTROY){
			myAS.PlayOneShot(blockDestroy);
		}else if(sound == Sounds.GET_ITEM){
			myAS.PlayOneShot(getItem);
		}
	}
}
