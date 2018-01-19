using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds{
	BUTTON_MENU,LEAF_NOGEM,LEAF_GEM,DAMAGE,SHOOT,BEEP
}
public class AudioManager : MonoBehaviour {
	[SerializeField] private AudioSource myAS;//sfx
	[SerializeField] private AudioSource musicAS;//music

	[SerializeField] private List<AudioClip> audioClips;

	public void PlaySound(Sounds sound){
		AudioClip a = audioClips[sound.GetHashCode()];
		myAS.clip = a;
		myAS.Play();
	}
	public void ChangePitch(){
		musicAS.pitch = 1.15f;
	}
	public void ResetPitch(){
		musicAS.pitch = 1;
	}

}
