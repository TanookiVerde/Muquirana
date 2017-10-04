using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTitle : MonoBehaviour {

	[SerializeField] private Image image;
	[SerializeField] private float maxScale = 1;
	float scale;

	void Start () 
	{
		scale = 0;
		StartCoroutine(Appear());
	}

	private IEnumerator Appear()
	{
		var r_t = image.GetComponent<RectTransform>();
		var initial_scale = new Vector3(0,0,0);
		var target_scale = new Vector3(1,1,1)*maxScale;

		r_t.localScale = initial_scale;
		while(!Mathf.Approximately(r_t.localScale.x,1)){
			r_t.localScale = Vector3.Lerp(r_t.localScale,target_scale,0.1f);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(1);
		while(!Mathf.Approximately(r_t.localScale.x,0)){
			r_t.localScale = Vector3.Lerp(r_t.localScale,initial_scale,0.1f);
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}

	public void SetBossName (string name)
	{
		Text textComponent = GetComponentInChildren <Text> ();
		textComponent.text = name;
	}


}
