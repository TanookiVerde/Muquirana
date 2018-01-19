using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	[Header("Level")]
	[SerializeField] private int gemPerTen;
	[Range(10,20)]
	[SerializeField] private float cameraMaxVelocity;
	[SerializeField] private int moneyRequirementStep = 1000;

	[Header("Spawnable Objects")]
	[SerializeField] List<GameObject> packList;
	[SerializeField] List<GameObject> gemList;
	[SerializeField] GameObject spikeObstacle;
	[SerializeField] GameObject pintorDeVentoObstacle;
	[SerializeField] GameObject heartItem;
	[SerializeField] private GameObject boomPrefab;

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
	[SerializeField] private Text moneyText;
	[SerializeField] private Image transitor;

	[Header("Bosses")]
	public bool bossDefeated;
	[SerializeField] private List<GameObject> bossPrefab;

	//MISC.
	private Camera cameraInScene;
	private Vector3 initialPosition;
	private GameObject player;
	private float distanceFromLastTile;
	private float cameraSize;
	private bool canStart;
	private bool canIncreaseCameraVelocity;

	private void Start(){
		//GETTERS
		GetPlayer();
		GetCamera();
		GetInitialPosition();
		GetLevelInfo();
		GetCameraSize();
		GetMoneyText();

		//SETTERS
		canIncreaseCameraVelocity = true;

		//DESABILITA COISAS DESNECESSARIAS NO MOMENTO
		DisableGameOverText();
		ToggleExpScreen(false);
		
		//COMEÇA LOOP DO LEVEL
		StartCoroutine( LevelStructure() );
	}
	private IEnumerator LevelStructure(){
		yield return Transite(0);
		//ESTRUTURA P/ FASE INFINITA
		while(true){
			bossDefeated =  false;
			while(totalMoney < moneyRequirementStep){
				MoveCamera();
				SpawnRandomTile();
				IncreaseCameraVelocity();
				yield return new WaitForEndOfFrame();
			}
			yield return BossBattle();
			IncreaseMoneyRequirementStep();
		}
	}
	private IEnumerator Transite(int target){
		var current = Mathf.Abs(target - 1);
		transitor.fillAmount = current;
		while(transitor.fillAmount != target){
			transitor.fillAmount = Mathf.MoveTowards(transitor.fillAmount,target,0.05f);
			yield return new WaitForEndOfFrame();
		}
	}
	private IEnumerator BossBattle(){
		AudioManager a = GameObject.Find("MusicHandler").GetComponent<AudioManager>();
		a.ChangePitch();
		SpawnBoss();
		while(!bossDefeated){
			yield return new WaitForEndOfFrame();
		}
		DestroyAllSeeds();
		a.ResetPitch();
		print("FIM DO BOSS");
	}
	private void IncreaseCameraVelocity(){
		if(canIncreaseCameraVelocity && cameraVelocity < cameraMaxVelocity){
			cameraVelocity += 0.0005f;
		}
	}
	private void IncreaseMoneyRequirementStep(){
		moneyRequirementStep = (int) (moneyRequirementStep*2.5f);
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
		cameraVelocity = 8f;
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
			CreateFromTile(generalTileList[Random.Range(0,generalTileList.Count)]);
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
		}else if (Random.Range(0,10) > 7){
			return heartItem;
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
		//desativar UI ingame e so deixar expPanel
		print(GameObject.Find("ItemCatcher"));
		GameObject.Find("ItemCatcher").SetActive(false);
		cameraVelocity = 0;
		canIncreaseCameraVelocity = false;
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
		int random_number = Random.Range(0,bossPrefab.Count);
		DestroyAllTiles();
		//print(random_number);
		Instantiate( bossPrefab[random_number],
					 Vector3.zero,
					 Quaternion.identity,
					 cameraInScene.transform).transform.localPosition = Vector3.zero + Vector3.forward;
	}
	private void DestroyAllTiles(){
		var blocks = FindObjectsOfType<Block>();

		for(int i = blocks.Length - 1; i >= 0; i--){
			var b = Instantiate(boomPrefab,blocks[i].transform.position,Quaternion.identity);
			Destroy(b,0.5f);
			Destroy(blocks[i].gameObject);
		}
		var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		for(int i = obstacles.Length - 1; i >= 0; i--){
			var b = Instantiate(boomPrefab,obstacles[i].transform.position,Quaternion.identity);
			Destroy(b,0.5f);
			Destroy(obstacles[i].gameObject);
		}
		var tiles = GameObject.FindGameObjectsWithTag("Tiles");
		for(int i = tiles.Length - 1; i >= 0; i--){
			var b = Instantiate(boomPrefab,tiles[i].transform.position,Quaternion.identity);
			Destroy(b,0.5f);
			Destroy(tiles[i].gameObject);
		}
	}
	private void DestroyAllSeeds(){
		var seeds = FindObjectsOfType<Seed>();
		for(int i = seeds.Length - 1; i>= 0; i--){
			var b = Instantiate(boomPrefab,seeds[i].transform.position,Quaternion.identity);
			Destroy(b,0.5f);
			Destroy(seeds[i].gameObject);
		}
	}
}