using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeInOut : MonoBehaviour {

	private Image panelImage;

	// Use this for initialization
	void Start () 
	{
		panelImage = GetComponent<Image> ();
	}

	// Escurecer
	public IEnumerator FadeIn (float time)
	{
		panelImage.CrossFadeAlpha (255, time, false);
		yield return new WaitForSeconds (time);
	}

	// Clarear
	public IEnumerator FadeOut (float time)
	{
		panelImage.CrossFadeAlpha (0.001f, time, false);
		yield return new WaitForSeconds (time);
	}
}
