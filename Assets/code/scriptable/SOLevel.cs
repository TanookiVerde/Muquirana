using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TreasureType{
	SAPPHIRE, OPAL, AMETHYST, PLATINUM, EMERALD
}
[CreateAssetMenu(menuName = "Custom/LevelModel")]
public class SOLevel : ScriptableObject {
	[Header("Preferences")]
	[SerializeField] public int levelSize;
	[SerializeField] public float preparationTime;
	[SerializeField] public float initialCameraVelocity, 
		  				   		  finalCameraVelocity,
								  requiredMoney;
	[SerializeField] public TreasureType treasure;

	[Header("Obstacles")]
	[SerializeField] public bool pegaPega;
	[SerializeField] public bool pintorDeVento;
	[SerializeField] public bool leafBarrier;
	[SerializeField] public bool spikedBarrier;
	[SerializeField] public bool bolaDePena;
}
