using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public void OpenNewScene(string name){
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
	}
}
