using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	[SerializeField] 
	private Image _transition;
	[SerializeField] 
	private bool _transition_activated;

	[SerializeField]
	private List<GameObject> _allScreens;

	[SerializeField] Text levelText;

	[SerializeField] private SO_PlayerData playerData;

	private int _currentIndex = -1;

	private void Start(){
		levelText.text = "Level "+ PlayerPrefs.GetInt("level");
	}
	public void Reset(){
		playerData.level = 1;
		for(int i = 0; i < playerData.collectedItems.Length;i++){
			playerData.collectedItems[i] = false;
		}
		PlayerPrefs.SetInt("exp",0);
		PlayerPrefs.SetInt("level",1);
		levelText.text = "Level "+ PlayerPrefs.GetInt("level");
	}
	public void ChangeScreen(int index){
		if(_currentIndex > 0){
			BackToMenu();
		}
		_currentIndex = index;
		OpenScreen();
	}
	public void BackToMenu(){
		StartCoroutine( Transite() );
		_allScreens[_currentIndex].SetActive(false);
		_currentIndex = -1;
	}
	private void OpenScreen(){
		StartCoroutine( OpenScreenAnimation() );
	}
	public void TransiteButton(){
		StartCoroutine( Transite() );
	}
	private IEnumerator Transite(){
		float target = 1;
		float current = 0;
		if(_transition_activated){
			target = 0;
			current = 1;
		}
		_transition.fillAmount = current;
		while(_transition.fillAmount != target){
			_transition.fillAmount = Mathf.MoveTowards(_transition.fillAmount,target,0.05f);
			yield return new WaitForEndOfFrame();
		}
		_transition_activated = !_transition_activated;
		EnableRaycastOnTransition();
	}
	private void EnableRaycastOnTransition(){
		_transition.raycastTarget = _transition_activated;
	}
	public void NewScene(string name){
		StartCoroutine( NewSceneAnimation(name) );
	}
	private IEnumerator NewSceneAnimation(string name){
		yield return Transite();
		SceneManager.LoadScene(name);
	}
	private IEnumerator OpenScreenAnimation(){
		yield return Transite();
		_allScreens[_currentIndex].SetActive(true);
	}

}
