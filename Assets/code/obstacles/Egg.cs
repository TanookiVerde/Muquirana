using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

	[SerializeField] private GameObject[] eggPrizes;
	[SerializeField] private bool allowEmptyEgg = true;
	[SerializeField] private float prizeGrowSpeed = 1f;

	private GameObject prize;
	private Animation animation;
	private bool emptyEgg = false;
	private bool bossEgg = false;

	static private int emptyEggCounter = 0;
	static private int eggAmount;


	// Quando spawnar um ovo novo, destroi um coração antigo, se tiver
	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.CompareTag ("Heart")) 
		{
			StopCoroutine ("ShowEggPrize");
			Destroy (other.gameObject);
		}
	}

	public void SetEggAmount (int amount)
	{
		eggAmount = amount;
	}

	// Use this for initialization
	void Start () 
	{
		animation = GetComponent<Animation> ();

		int i = 0;
		if (allowEmptyEgg && emptyEggCounter < eggAmount-1) // Sempre garante uma chance do player se mover
			i = Random.Range (0, eggPrizes.Length+1);
		else
			i = Random.Range (0, eggPrizes.Length);

		if (i == eggPrizes.Length) // Ovo vazio
		{
			emptyEgg = true;
			emptyEggCounter++;
		}
		else
			prize = eggPrizes [i];

	}


	public void SetBossEgg ()
	{
		bossEgg = true;
	}

	public void Hatch ()
	{
		StartCoroutine (HatchCoroutine());
	}

	private IEnumerator HatchCoroutine ()
	{
		animation.Play ();
		yield return new WaitForSeconds (animation.clip.length);

		if (!emptyEgg && !bossEgg)
		{
			GameObject obj = (GameObject) Instantiate (prize, transform.position, Quaternion.identity);
			obj.transform.SetParent (transform.parent);

			// Se for um Pintor de Vento...
			Pintor pintorScript = obj.GetComponent<Pintor> ();
			if (pintorScript)
				pintorScript.SpawnedFromEgg ();
			
			yield return ShowEggPrize (obj);
		}
		else if (bossEgg)  // Se for o ovo do boss, não spawna nada, só reativa o renderer dele
		{
			transform.parent.GetComponent<Bartolomeu>().ActivateRenderer();
			yield return ShowEggPrize (transform.parent.gameObject);
		}
			
		Destroy (this.gameObject);
	}

	private IEnumerator ShowEggPrize (GameObject prize)
	{
		if (prize != null) 
		{
			Vector3 originalScale = prize.transform.localScale;
			MonoBehaviour[] scripts = prize.GetComponents <MonoBehaviour> ();
			foreach (MonoBehaviour script in scripts) 
			{
				script.enabled = false;
			}

			prize.transform.localScale = Vector3.one * 0.1f;
			while ((originalScale - prize.transform.localScale).magnitude > 0.01f) 
			{
				prize.transform.localScale = Vector3.Lerp (prize.transform.localScale, originalScale, prizeGrowSpeed);
				yield return new WaitForEndOfFrame ();
			}

			foreach (MonoBehaviour script in scripts) 
			{
				script.enabled = true;
			}
		}
	}

	// HOOK
	private void OnMouseDown()
	{
		GameObject.Find("Player").GetComponent<Muquirana>().ChangePosition (transform.position.y);
	}
}
