using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType{
	ANEL,BAU,BRACELETE,BRINCO, BROCHE, CALICE,CINTO,ESPELHO,LIVRO,MOEDA,PRENDEDOR,PULSEIRA,URNA
}
[CreateAssetMenu(menuName = "Custom/GemModel")]
public class SOGem : ScriptableObject {
	[Header("Properties")]
	[Range(0,200)]
	[SerializeField] public int value;
	[SerializeField] public string description;
	[Range(0,1)]
	[SerializeField] public float ratio;
	public ItemType itemType;
}
