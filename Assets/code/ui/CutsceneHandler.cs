using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour {

	[SerializeField] private float cutsceneTime = 2f;

	[Header("Fade preferences")]
	[SerializeField] private GameObject fadePanel;
	[SerializeField] private float fadeTime = 1f;

	private FadeInOut fadeScript;

	private List<GameObject> cutsceneList = new List<GameObject>();
	private int currentCutscene = 0;


	// Use this for initialization
	void Start () 
	{
		fadeScript = fadePanel.GetComponent<FadeInOut> ();

		// Add cutscene objects to a list and disable all of them
		for (int i = 0; i < transform.childCount; i++) 
		{
			GameObject obj = transform.GetChild (i).gameObject;
			cutsceneList.Add (obj);
			obj.SetActive (false);
		}

		StartCoroutine (HandleCutscenes ());
	}


	private IEnumerator HandleCutscenes ()
	{
		// Enable the first cutscene and fade out
		cutsceneList [0].SetActive (true);
		fadeScript.StartCoroutine (fadeScript.FadeOut (fadeTime));
		return null;

	}

	private IEnumerator ChangeCutscene ()
	{
		// Fade in
		yield return fadeScript.FadeIn (fadeTime);
		yield return new WaitForSeconds (fadeTime);

		// Change cutscene
		cutsceneList[currentCutscene].SetActive (false);
		currentCutscene++;
		cutsceneList [currentCutscene].SetActive (true);

		//Fade out
		yield return fadeScript.FadeOut (fadeTime);
		yield return new WaitForSeconds (fadeTime);
	}
}
