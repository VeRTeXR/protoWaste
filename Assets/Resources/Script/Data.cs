using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class  Data : MonoBehaviour
{

//	public GameObject Timer;
	public GameObject Player;
	public float MaxAvatarSpawnTimer;
	public float CurrentAvatarSpawnTimer;
	public float MaxEnemySpawnTimer;
	public float CurrentEnemySpawnTimer;
	private bool _playerCompletedTheLevel;
	private int _highScore;
	private int _currentSessionScore;
	private int _lastLevelReached;

	public Transform rBorder;
	public Transform lBorder;
	public Transform tBorder;
	public Transform bBorder;
	
	private static Data _instance;
	public static Data Instance
	{
		get { return _instance; }
	}
	
	void Awake()
	{
		if (_instance == null)
			_instance = this;
		else 
			Destroy(this);

		CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
		
		InitializeGameObject(); 
		InitializeTimer();
		InitializePlayerValue();
	}
	public int CurrentLevel { get; set; }

	private void InitializeGameObject()
	{
//		if(Timer == null)
//			Timer = GameObject.FindWithTag("Timer");

		Player = GameObject.FindWithTag("Player");
		if (Player == null)
			Player = (GameObject)Instantiate(Resources.Load("Prefab/Player"));
	}
//
	private void InitializeTimer()
	{
		MaxAvatarSpawnTimer = 10;
		MaxEnemySpawnTimer = 5f;
		CurrentAvatarSpawnTimer = MaxAvatarSpawnTimer;
		CurrentEnemySpawnTimer = MaxEnemySpawnTimer;
	}

	private void InitializePlayerValue()
	{
//		CurrentLevel = 0;
//		_lastLevelReached = 0;
//		_currentSessionScore = 0; 
	}

	void Update ()
	{
		CurrentAvatarSpawnTimer -= Time.deltaTime;
		CurrentEnemySpawnTimer -= Time.deltaTime;
//		Timer.GetComponent<Text>().text = CurrentTimer.ToString("F3");
//
		if (CurrentAvatarSpawnTimer <= 0)
		{
			var Avatar = Resources.Load("Prefab/Avatar");
			int x = (int)Random.Range (lBorder.position.x, rBorder.position.x);
			int y = (int)Random.Range (bBorder.position.y, tBorder.position.y);
		
			Instantiate (Avatar, new Vector2 (x, y), Quaternion.identity);
			CurrentAvatarSpawnTimer = MaxAvatarSpawnTimer;
		}
		
		if (CurrentEnemySpawnTimer <= 0)
		{
			var Enemy = Resources.Load("Prefab/Enemy");
			int x = (int)Random.Range (lBorder.position.x, rBorder.position.x);
			int y = (int)Random.Range (bBorder.position.y, tBorder.position.y);
			
			Instantiate (Enemy, new Vector2 (x, y), Quaternion.identity);
			CurrentEnemySpawnTimer = MaxEnemySpawnTimer;
		}

//		if (_playerCompletedTheLevel)
//		{
//			LevelComplete();
//			Debug.LogError("increment level do shit");
//		} 
	}

	public void LevelComplete()
	{
		CurrentLevel++;
		PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
		Debug.LogError(CurrentLevel);
		PlayerPrefs.SetInt("LastLevelReached",CurrentLevel);
		RestartLevel();
	}

	private void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void GameOver()
	{
		Debug.LogError("do something on game over");
		RestartLevel();
//		CurrentTimer = MaxTimer;
	}

	public void ResetPlayerPref()
	{
		PlayerPrefs.DeleteKey("CurrentLevel");
	}

	private void OnApplicationQuit()
	{
		ResetPlayerPref();
	}

	public void AddPoint(int totalHealth)
	{
		_currentSessionScore += totalHealth;
	}
}
