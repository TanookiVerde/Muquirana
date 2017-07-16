using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelManager : MonoBehaviour {
	[Header("Level")]
	[SerializeField] private SOLevel levelData;

	[Header("Spawnable Objects")]
	[SerializeField] List<GameObject> packList;
	[SerializeField] List<GameObject> tileList;
	[SerializeField] GameObject leafObstacle;
	[SerializeField] GameObject spikeObstacle;
	[SerializeField] GameObject pegaPegaObstacle;
	[SerializeField] GameObject pintorDeVentoObstacle;
	[SerializeField] GameObject bolaDePenaObstacle;

	[Header("Runtime")]
	[SerializeField] private float levelSoFar;
	[SerializeField] private float cameraVelocity;
	[SerializeField] private List<GameObject> collectedItems;
	[SerializeField] private int totalMoney;
	[SerializeField] private bool bossDefeated;

	private Camera cameraInScene;
	private Vector3 initialPosition;
	private GameObject player;
	private float distanceFromLastTile;
	private float cameraSize;

	private void Start(){
		GetPlayer();
		GetCamera();
		GetInitialPosition();
		GetLevelInfo();
		GetCameraSize();

		StartCoroutine( LevelStructure() );
	}
	private IEnumerator LevelStructure(){
		//PREPARATION
		while(true){
			yield return new WaitForSeconds(levelData.preparationTime);
			break;
		}
		Debug.Log("END_OF_PREPARATION");
		//MAIN
		while(GetDistanceSoFar() < levelData.levelSize){
			MoveCamera();
			SpawnRandomTile();
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("END_OF_MAIN");
		//BOSS
		while(!bossDefeated){
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("END_OF_BOSS");
		//EXP SCREEN
	}
	private void MoveCamera(){
		float offset = cameraVelocity*Time.deltaTime;
		cameraInScene.transform.position += Vector3.right*offset;
		distanceFromLastTile += offset;
	}
	private float GetDistanceSoFar(){
		return levelSoFar = cameraInScene.transform.position.x - initialPosition.x;
	}
	private void GetLevelInfo(){
		cameraVelocity = levelData.initialCameraVelocity;
	}
	private void GetCamera(){
		cameraInScene = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	private void GetInitialPosition(){
		initialPosition = player.transform.position;
	}
	private void GetPlayer(){
		player = GameObject.Find("Player");
	}
	private GameObject GetRandomPack(){
		return packList[ Random.Range(0,packList.Count) ];
	}
	private void GetCameraSize(){
		cameraSize = cameraInScene.aspect*cameraInScene.orthographicSize*2;
	}
	private void SpawnRandomTile(){
		if(distanceFromLastTile > cameraSize){
			CreateFromTile(tileList[Random.Range(0,tileList.Count)]);
			distanceFromLastTile = 0;
		}
	}
	private void CreateFromTile(GameObject tile){
		GameObject temp = new GameObject("Pack");
		int objectsInTile = tile.transform.childCount;

		temp.transform.position = cameraInScene.transform.position + Vector3.right*cameraSize + Vector3.forward*10;
		for(int i = 0; i < objectsInTile; i++){
			if(tile.transform.GetChild(i).CompareTag("tile_OBSTACLE")){
				GameObject go = Instantiate(leafObstacle,
											tile.transform.GetChild(i).position,
											Quaternion.identity,
											temp.transform);
				go.transform.localPosition = tile.transform.GetChild(i).position;
				go.transform.localScale = tile.transform.GetChild(i).localScale;
			}else if(tile.transform.GetChild(i).CompareTag("tile_PACK")){
				Instantiate(packList[Random.Range(0,packList.Count)],
							tile.transform.GetChild(i).position,
							Quaternion.identity,
							temp.transform).transform.localPosition = tile.transform.GetChild(i).position;
			}
		}

	}
	
}
