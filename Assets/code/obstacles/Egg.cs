using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

	[SerializeField] private GameObject[] eggPrizes;
	[SerializeField] private bool allowEmptyEgg = true;

	private GameObject prize;
	private Animation animation;
	private bool emptyEgg = false;
	private bool bossEgg = false;

	static private int emptyEggCounter = 0;
	static private int eggAmount;

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
		}
		else if (bossEgg)  // Se for o ovo do boss, não spawna nada, só reativa o renderer dele
		{
			transform.parent.GetComponent<SpriteRenderer>().enabled = true;
		}

		Destroy (this.gameObject);
	}
}
