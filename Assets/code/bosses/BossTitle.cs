using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTitle : MonoBehaviour {

	[SerializeField] private Image image;

	void Start () {
		image.fillAmount = 0;
		StartCoroutine(Appear());
	}
	private IEnumerator Appear(){
		while(!Mathf.Approximately(image.fillAmount,1)){
			image.fillAmount = Mathf.Lerp(image.fillAmount,1,0.1f);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(1);
		while(!Mathf.Approximately(image.fillAmount,0)){
			image.fillAmount = Mathf.Lerp(image.fillAmount,0,0.1f);
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}
}
