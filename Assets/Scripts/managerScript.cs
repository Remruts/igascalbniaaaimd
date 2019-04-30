using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class managerScript : MonoBehaviour {

	public static managerScript man;
	public int lives = 30;
	public int attack = 1;
	public int defense = 1;
	public int speed = 1;
	public bool debug = false;

	public int currentFloor = 1;

	public TMP_Text heartNumberUIText;
	public TMP_Text debugText;
	public TMP_Text doomTimerText;
	public TMP_Text floorNumberUIText;
	public TMP_Text attk_text;
	public TMP_Text def_text;
	public TMP_Text spd_text;

	public GameObject livesChangeUIPrefab;

	public AudioSource dungeonMusic;
	public GameObject dungeonUI;

	public GameObject playerPrefab;
	[HideInInspector]
	public GameObject playerChar;
	public List<GameObject> enemies;
	GameObject[] allEnemies;

	public Dictionary<string, bool> hasBubbles;

	[System.Serializable]
	public class Bubble
    {
		public string name;
        public float speed;
        public float spread;
        public float life;
		public float cooldown;
		public int size;
		public float attk;
		public int number;
		public float drag;
		public bool ricochet;
		public int divideOnDeath;
		public Sprite[] sprites;
    }
	public Bubble[] bubbleList;
	public Dictionary<string, Bubble> bubbleDict;

	public string currentBubble = "regular";
	public int currentBubbleNumber = 0;

	//public Yarn.Unity.DialogueRunner dialog;
	public Animator blackScreen;

	public bool transitioning = false;
	float doomTimer = 30.99f;

	int startLives;
	int startAttack;
	int startDefense;
	int startSpeed;

	void Awake(){
		if (man == null){
			DontDestroyOnLoad(this.gameObject);
			man = this;
		} else {
			Destroy(gameObject);
			return;
		}
	}

	public void Reset(){
		if (dungeonMusic.isPlaying){
			dungeonMusic.Stop();
		}

		shopItemsScript.shop.ResetItems();
		GoToScene("title_scene");
		dungeonUI.SetActive(false);
		lives = startLives;
		attack = startAttack;
		defense = startDefense;
		speed = startSpeed;

		currentFloor = 1;

		hasBubbles = new Dictionary<string, bool>();
		bubbleDict = new Dictionary<string, Bubble>();

		doomTimer = 30.99f;

		foreach (Bubble b in bubbleList){
			bubbleDict.Add(b.name, b);
			hasBubbles.Add(b.name, false);
		}
		hasBubbles["regular"] = true;
		currentBubble = "regular";
		currentBubbleNumber = 0;
	}

	void Start(){
		startLives = lives;
		startAttack = attack;
		startDefense = defense;
		startSpeed = speed;

		doomTimer = 30.99f;

		currentFloor = 1;

		hasBubbles = new Dictionary<string, bool>();
		bubbleDict = new Dictionary<string, Bubble>();

		foreach (Bubble b in bubbleList){
			bubbleDict.Add(b.name, b);
			hasBubbles.Add(b.name, false);
		}
		hasBubbles["regular"] = true;

		Scene scene = SceneManager.GetActiveScene();
		if (scene.name == "dungeon_scene"){
			startDungeon();
		}
	}

	public void getBubble(string bubble){
		hasBubbles[bubble] = true;
	}

	public void upStat(string stat){
		switch (stat){
		case "attack":
			attack += 1;
		break;
		case "defense":
			defense += 1;
		break;
		case "speed":
			speed += 1;
		break;
		}
	}

	public void startDungeon(){

		if (!dungeonMusic.isPlaying){
			dungeonMusic.Play();
		}

		doomTimer = 30.99f;

		dungeonUI.SetActive(true);

		heartNumberUIText.text = lives.ToString();
		floorNumberUIText.text = "F" + currentFloor.ToString();

		attk_text.text = "x" + attack.ToString();
		def_text.text = "x" + defense.ToString();
		spd_text.text = "x" + speed.ToString();

		if (playerChar != null){
			Destroy(playerChar);
		}
		playerChar = Instantiate(playerPrefab, transform.position, Quaternion.identity) as GameObject;

		dungeonGeneratorScript.dungeon.generateDungeon();
		AStar.pathfinding.createGrid();

		DestroyAllEnemies();
		allEnemies = GameObject.FindGameObjectsWithTag("enemy");
		updateEnemies();
	}

	void updateEnemies(){
		enemies = new List<GameObject>();
		foreach (GameObject enemy in allEnemies){
			if (enemy == null){
				continue;
			}
			if ((enemy.transform.position - Camera.main.transform.position).magnitude > 20){
				enemy.SetActive(false);
			} else {
				enemy.SetActive(true);
				enemies.Add(enemy);
			}
		}
	}

	public void DestroyAllEnemies(){
		if (allEnemies != null && allEnemies.Length > 0){
			foreach(GameObject enemy in allEnemies){
				Destroy(enemy);
			}
		}
	}

	public int numberOfEnemiesAround(Vector3 pos, float radius){
		int count = 0;
		foreach(GameObject enemy in enemies){
			if (enemy == null){
				continue;
			}
			if ((pos - enemy.transform.position).magnitude <= radius){
				count += 1;
			}
		}
		return count;
	}

	public void changeLivesBy(int lifePoints){
		lives += lifePoints;
		if (playerChar!= null){
			GameObject theHearts = Instantiate(livesChangeUIPrefab, transform.position, Quaternion.identity) as GameObject;
			theHearts.GetComponent<livesChangeUIScript>().setLives(lifePoints);

			theHearts.transform.position = playerChar.transform.position + Vector3.up;

			if (lives <= 0){
				lives = 0;
				playerChar.GetComponent<axolotlScript>().die();
				blackScreen.Play("fadeToBlack");
			}
			heartNumberUIText.text = lives.ToString();
		}
	}

	public void drawDebugText(string str){
		debugText.text = str;
	}

	// Update is called once per frame
	void Update () {
		Scene scene = SceneManager.GetActiveScene();
		if (scene.name == "dungeon_scene"){
			doomTimer -= Time.deltaTime;
			if (doomTimer <= 0){
				doomTimer = 30.99f;
				changeLivesBy(-100);
			}
			doomTimerText.text = (Mathf.Floor(doomTimer)).ToString();
			if (Input.mouseScrollDelta.y > 0){
				currentBubbleNumber = (currentBubbleNumber + 1) % bubbleList.Length;
				currentBubble = bubbleList[currentBubbleNumber].name;
				while (!hasBubbles[currentBubble]){
					currentBubbleNumber = (currentBubbleNumber + 1) % bubbleList.Length;
					currentBubble = bubbleList[currentBubbleNumber].name;
				}
			}
			if (Input.mouseScrollDelta.y < 0){
				currentBubbleNumber -= 1;
				if (currentBubbleNumber < 0){
					currentBubbleNumber = bubbleList.Length-1;
				}
				currentBubble = bubbleList[currentBubbleNumber].name;
				while (!hasBubbles[currentBubble]){
					currentBubbleNumber -= 1;
					if (currentBubbleNumber < 0){
						currentBubbleNumber = bubbleList.Length-1;
					}
					currentBubble = bubbleList[currentBubbleNumber].name;
				}
			}

		}
		if (scene.name == "title_scene"){
			if (Input.anyKeyDown){
				GoToDungeon(1);
			}
		}

		if (Input.GetKey("escape")){
			Application.Quit();
		}
		if (Input.GetKeyDown("f11")){
			Screen.fullScreen = !Screen.fullScreen;
		}
	}

	public void GoToNextFloor(){
		GoToDungeon(currentFloor+1);
	}

	public void GoToDungeon(int floor){
		currentFloor = floor;
		StartCoroutine(LoadDungeonAsync());
	}

	public void GoToScene(string scene){
		SceneManager.LoadScene(scene);
	}

	public void GoToShop(){
		dungeonUI.SetActive(false);
		GoToScene("shop_scene");
	}

	IEnumerator LoadDungeonAsync(){
	   AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("dungeon_scene");
	   while (!asyncLoad.isDone){
		   yield return null;
	   }
	   startDungeon();
   }
}
