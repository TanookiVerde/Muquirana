using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelManager : MonoBehaviour {
	[Header("Level")]
	[SerializeField] private SOLevel levelData;
	[SerializeField] private int gemPerTen;

	[Header("Spawnable Objects")]
	[SerializeField] List<GameObject> packList;
	[SerializeField] List<GameObject> tileList;
	[SerializeField] List<GameObject> gemList;
	[SerializeField] GameObject leafObstacle;
	[SerializeField] GameObject spikeObstacle;
	[SerializeField] GameObject pegaPegaObstacle;
	[SerializeField] GameObject pintorDeVentoObstacle;
	[SerializeField] GameObject bolaDePenaObstacle;

	[Header("Runtime")]
	[SerializeField] private float levelSoFar;
	[SerializeField] public float cameraVelocity;
	[SerializeField] public List<GameObject> collectedItems;
	[SerializeField] public int totalMoney;
	[SerializeField] private bool bossDefeated;

	[Header("UI")]
	[SerializeField] private GameObject textGameOver;
	[SerializeField] private GameObject textInit;
	[SerializeField] private Text velocityText;
	[SerializeField] private Slider levelSlider;
	[SerializeField] private Slider moneySlider;
	private Text moneyText;

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
		GetMoneyText();
		DisableGameOverText();
		ToggleInitText(true);

		StartCoroutine( LevelStructure() );
	}
	private IEnumerator LevelStructure(){
		//PREPARATION
		UpdateVelocityText();
		while(!Input.GetMouseButtonDown(0)){
			yield return new WaitForFixedUpdate();
		}
		ToggleInitText(false);
		Debug.Log("END_OF_PREPARATION");
		//MAIN
		while(GetDistanceSoFar() < levelData.levelSize){
			UpdateVelocityText();
			UpdateSlider();
			MoveCamera();
			SpawnRandomTile();
			IncreaseCameraVelocity();
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
	private void ToggleInitText(bool b){
		textInit.SetActive(b);
	}
	private void UpdateVelocityText(){
		velocityText.text = "VELOCIDADE: " + cameraVelocity;
	}
	private void UpdateSlider(){
		levelSlider.value = levelSoFar/levelData.levelSize;
		moneySlider.value = int.Parse(moneyText.text)/levelData.requiredMoney;
	}
	private void IncreaseCameraVelocity(){
		if(cameraVelocity < levelData.finalCameraVelocity){
			cameraVelocity += 0.001f;
		}
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
				GameObject go = Instantiate(spikeObstacle,
											tile.transform.GetChild(i).position,
											Quaternion.identity,
											temp.transform);
				go.transform.localPosition = tile.transform.GetChild(i).position;
				go.transform.localRotation = tile.transform.GetChild(i).localRotation;
			}else if(tile.transform.GetChild(i).CompareTag("tile_PACK")){
				GameObject go = Instantiate(packList[Random.Range(0,packList.Count)],
											tile.transform.GetChild(i).position,
											Quaternion.identity,
											temp.transform);
				go.transform.localPosition = tile.transform.GetChild(i).position;
				go.GetComponent<Block>().GetGem( GetRandomGem() );
			}else if(tile.transform.GetChild(i).CompareTag("tile_PINTOR") && levelData.pintorDeVento){
				//Debug.Log("VIVA_O_PINTOR");
				GameObject go = Instantiate(pintorDeVentoObstacle,
											tile.transform.GetChild(i).position,
											Quaternion.identity,
											temp.transform);
				go.transform.localPosition = tile.transform.GetChild(i).position;
			}else if(tile.transform.GetChild(i).CompareTag("tile_PEGAPEGA") && levelData.pegaPega){
				Debug.Log("VIVA_O_PEGAPEGA");
				GameObject go = Instantiate(pegaPegaObstacle,
											tile.transform.GetChild(i).position,
											Quaternion.identity,
											temp.transform);
				go.transform.localPosition = tile.transform.GetChild(i).position;
			}else if(tile.transform.GetChild(i).CompareTag("tile_BOLA") && levelData.bolaDePena){
				Debug.Log("VIVA_O_BOLA");
				GameObject go = Instantiate(bolaDePenaObstacle,
											tile.transform.GetChild(i).position,
											Quaternion.identity,
											temp.transform);
				go.transform.localPosition = tile.transform.GetChild(i).position;
			}
		}

	}
	private GameObject GetRandomGem(){
		int randN = Random.Range(0,10);
		if(randN > 10 - gemPerTen){
			return gemList[ Random.Range(0,gemList.Count) ];
		}
		return null;
	}
	public IEnumerator UpdateMoneyText(){
		int past = int.Parse(moneyText.text);

		while(past-1 < totalMoney){
			moneyText.text = past.ToString();
			yield return new WaitForEndOfFrame();
			past++;
		}
	}
	private void GetMoneyText(){
		moneyText = GameObject.Find("Money_Text").GetComponent<Text>();
	}
	public IEnumerator GameOver(){
		player.GetComponent<SpriteRenderer>().enabled = false;
		textGameOver.SetActive(true);
		yield return new WaitForSeconds(5);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("_mainMenu");
	}
	private void DisableGameOverText(){
		textGameOver.SetActive(false);
	}
}
