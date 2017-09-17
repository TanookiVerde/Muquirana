using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
	#region VARIAVEIS
	[Header("Level")]
	[SerializeField] private SOLevel levelData;
	[SerializeField] private int gemPerTen;

	[Header("Spawnable Objects")]
	[SerializeField] List<GameObject> packList;
	[SerializeField] List<GameObject> gemList;
	[SerializeField] GameObject leafObstacle;
	[SerializeField] GameObject spikeObstacle;
	[SerializeField] GameObject pegaPegaObstacle;
	[SerializeField] GameObject pintorDeVentoObstacle;
	[SerializeField] GameObject bolaDePenaObstacle;

	[Header("Tiles")]
	[SerializeField] List<GameObject> generalTileList;

	[Header("Runtime")]
	[SerializeField] private float levelSoFar;
	[SerializeField] public float cameraVelocity;
	[SerializeField] public List<GameObject> collectedItems;
	[SerializeField] public int totalMoney;

	[Header("UI")]
	[SerializeField] private GameObject textGameOver;
	[SerializeField] private GameObject expScreenPanel;
	[SerializeField] private GameObject textInit;
	[SerializeField] private Text velocityText;
	[SerializeField] private Slider levelSlider;
	[SerializeField] private Slider moneySlider;
	[SerializeField] private Text moneyText;

	[Header("Bosses")]
	[SerializeField] private bool bossDefeated;
	[SerializeField] private GameObject ticoPrefab;

	//MISC.
	private Camera cameraInScene;
	private Vector3 initialPosition;
	private GameObject player;
	private float distanceFromLastTile;
	private float cameraSize;
	private bool canStart;

	#endregion

	private void Start(){
		GetPlayer();
		GetCamera();
		GetInitialPosition();
		GetLevelInfo();
		GetCameraSize();
		GetMoneyText();
		DisableGameOverText();
		ToggleInitText(true);
		ToggleExpScreen(false);

		StartCoroutine( LevelStructure() );
	}
	private IEnumerator LevelStructure(){
		//PREPARATION
		UpdateVelocityText();
		ToggleInitText(false);
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
		float timer = 0;
		while(timer < levelData.preparationTime)
		{
			timer += Time.deltaTime;
			MoveCamera();
			UpdateVelocityText();
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("END_OF_BREATHING_TIME");
		//BOSS
		SpawnBoss();
		while(!bossDefeated){
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("END_OF_BOSS");
		//EXP SCREEN
		ToggleExpScreen(true);
	}
	private void ToggleInitText(bool b){
		textInit.SetActive(b);
	}
	private void UpdateVelocityText(){
		velocityText.text = "VELOCIDADE: " + cameraVelocity;
	}
	private void UpdateSlider(){
		levelSlider.value = levelSoFar/( levelData.levelSize );
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
		cameraSize = 27.25436f;
	}
	private void SpawnRandomTile(){
		if(distanceFromLastTile > cameraSize){
			CreateFromTile(levelData.levelTiles[Random.Range(0,generalTileList.Count)]);
			distanceFromLastTile = 0;
		}
	}
	private void CreateFromTile(GameObject tile){
		GameObject temp = new GameObject("Pack");
		int objectsInTile = tile.transform.childCount;

		temp.transform.position = cameraInScene.transform.position + Vector3.right*cameraSize + Vector3.forward*10;
        Instantiate(tile, 
                    cameraInScene.transform.position + Vector3.right * cameraSize + Vector3.forward * 10, 
                    Quaternion.identity);
		for(int i = 0; i < objectsInTile; i++){
			if(tile.transform.GetChild(i).CompareTag("tile_PACK")){
				GameObject go = Instantiate(packList[Random.Range(0,packList.Count)],
											tile.transform.GetChild(i).position,
											Quaternion.identity,
											temp.transform);
				go.transform.localPosition = tile.transform.GetChild(i).position;
				go.GetComponent<Block>().GetGem( GetRandomGem() );
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
		GameObject.Find("ItemCatcher").SetActive(false);
		cameraVelocity = 0;
		player.GetComponent<SpriteRenderer>().enabled = false;
		textGameOver.SetActive(true);
		yield return new WaitForSeconds(2);
		textGameOver.SetActive(false);
		ToggleExpScreen(true);
	}
	private void DisableGameOverText(){
		textGameOver.SetActive(false);
	}
	private void ToggleExpScreen(bool b){
		expScreenPanel.SetActive(b);
	}
	public void StartGame(){
		canStart = true;
	}
	private void SpawnBoss(){
		Instantiate(levelData.bossPack,Vector3.zero,Quaternion.identity,cameraInScene.transform).transform.localPosition = Vector3.zero;
	}
	public IEnumerator FinishLevel(){
		//Termina o level
		//Eh chamada quando se mata chefao ou gameover
		yield return new WaitForEndOfFrame();
	}
}