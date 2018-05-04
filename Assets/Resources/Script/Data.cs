using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class  Data : MonoBehaviour
{

//	public GameObject Timer;
	public GameObject Player;
	public float MaxTimer;
	public float CurrentTimer;
	private bool _playerCompletedTheLevel;
	private int _highScore;
	private int _currentSessionScore;
	private int _lastLevelReached;

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
			Instantiate(Resources.Load("Prefab/Player"));
	}
//
	private void InitializeTimer()
	{
		MaxTimer = 10;
		CurrentTimer = MaxTimer;
	}

	private void InitializePlayerValue()
	{
//		CurrentLevel = 0;
//		_lastLevelReached = 0;
//		_currentSessionScore = 0; 
	}

	void Update ()
	{
		CurrentTimer = CurrentTimer - Time.deltaTime;
//		Timer.GetComponent<Text>().text = CurrentTimer.ToString("F3");
//
		if (CurrentTimer <= 0)
		{
			var Avatar = Resources.Load("Prefab/Avatar");
			var a = Instantiate(Avatar, transform);
			CurrentTimer = MaxTimer;
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
