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
	[SerializeField] private Slider moneySlider;
	[SerializeField] private Text moneyText;
	[SerializeField] private Image transitor;

	[Header("Bosses")]
	[SerializeField] private bool bossDefeated;
	[SerializeField] private List<GameObject> bossPrefab;

	//MISC.
	private Camera cameraInScene;
	private Vector3 initialPosition;
	private GameObject player;
	private float distanceFromLastTile;
	private float cameraSize;
	private bool canStart;
	private bool canIncreseCameraVelocity;

	private void Start(){
		//GETTERS
		GetPlayer();
		GetCamera();
		GetInitialPosition();
		GetLevelInfo();
		GetCameraSize();
		GetMoneyText();

		//SETTERS
		canIncreseCameraVelocity = false;

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
			while(totalMoney <= moneyRequirementStep){
				MoveCamera();
				UpdateSlider();
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
		SpawnBoss();
		while(!bossDefeated){
			yield return new WaitForEndOfFrame();
		}
	}
	private void UpdateSlider(){
		moneySlider.value = int.Parse(moneyText.text)/(float)moneyRequirementStep;
	}
	private void IncreaseCameraVelocity(){
		if(canIncreseCameraVelocity && cameraVelocity < cameraMaxVelocity){
			cameraVelocity += 0.001f;
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
		canIncreseCameraVelocity = false;
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
		print(random_number);
		Instantiate( bossPrefab[random_number],
					 Vector3.zero,
					 Quaternion.identity,
					 cameraInScene.transform).transform.localPosition = Vector3.zero;
	}
	public IEnumerator FinishLevel(){
		//Termina o level
		//Eh chamada quando se mata chefao ou gameover
		yield return new WaitForEndOfFrame();
	}
	private void DestroyAllTiles(){
		//read the function name
	}
}