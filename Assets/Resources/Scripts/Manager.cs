using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
	
	public static Manager instance = null; 
	//private Manager managerScript;
	public GameObject ExitTest;
	public GameObject[] enemy;
	public GameObject player;
	public GameObject spawn;
	//public GameObject ExitTest;
	public float levelStartDelay = 0.3f;
	public float levelStartCountdown;
	public float HP = 20;
	public int level;
	public GameObject StartMenu;
	public GameObject ScoreManager;

	private GameObject levelImage;
	private Text levelText;
	private bool doingSetup = true;
	public Pause PauseScript;

	// Title

	void Start () {
		
		HP = 20;
		if (instance == null) {
			instance = this;
		}
		else if (instance != this){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		StartMenu = GameObject.FindGameObjectWithTag("StartMenu");
		if (StartMenu != null)
		{
			PauseScript = StartMenu.GetComponent<Pause>();
		}
		InitGame ();
	}

	void OnLevelWasLoaded(int index)  {
		FindObjectOfType<Score> ().Save (); // save everything on level load
		level++;
		Debug.Log("level"+level);

		if(GameObject.FindGameObjectWithTag("Player") == null)
		{
			spawn = GameObject.FindGameObjectWithTag("Spawn");
			Instantiate(player, spawn.transform.position, spawn.transform.rotation);
		}
		else 
		{
			spawn = GameObject.FindGameObjectWithTag("Spawn");
			player = GameObject.FindGameObjectWithTag("Player");
			player.transform.position = spawn.transform.position;
		}

		InitGame ();
	}


	void InitGame() {
//		doingSetup = true;
//		levelStartCountdown += Time.unscaledTime;
//		Time.timeScale = 0;
//		levelImage = GameObject.Find ("LevelImage");
//		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
//		levelText.text = "Dive " + level;
//		levelImage.SetActive (true);
//		Invoke ("HideLevelImage", 1);
//		boardScript.SetupScene (level);
	}

	void Update () {
        if (level < 1)
        {
            HP = 20;
	        Score.instance.Reset(); //score = 0;
	        //check score, health, reset it! 
        }
		if (Input.GetKeyDown (KeyCode.Z)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			//Application.LoadLevel("StartScn");
			//level = -1;
			//score = 0;
			//HP = 20;
			//Debug.Log(level);
			//	reload will actually reload from beginning
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			Score.instance.AddPoint(100);
		}
//        if (levelImage.activeSelf) {
//			if (Input.GetKeyDown (KeyCode.R)) {
//				SceneManager.LoadScene("StartScn", LoadSceneMode.Single);
//				level = -1;
//				score = 0;
//				HP = 20;
//				//Debug.Log(level);
//				//reload will actually reload from beginning
//			}
//		}
	}

	void HideLevelImage () {
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver() {

		//display node diagram of story
		//level survived?
		//
		FindObjectOfType<Score> ().Save ();
		levelText.text = "You survived " + level + " levels";
		levelImage.SetActive (true);
	}

}
