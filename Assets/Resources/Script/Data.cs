using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Persistence;

public class  Data : MonoBehaviour
{

//	public GameObject Timer;
	public GameObject Player;

	public GameObject StartMenu;
	public float MaxAvatarSpawnTimer;
	public float CurrentAvatarSpawnTimer;
	public float MaxEnemySpawnTimer;
	public float CurrentEnemySpawnTimer;
	private bool _playerCompletedTheLevel;
	private int _highScore;
	private int _currentSessionScore;

	public Transform rBorder;
	public Transform lBorder;
	public Transform tBorder;
	public Transform bBorder;

	public Sprite[] EnemySprites;
	public Sprite[] AlliedSprites;

	public GameObject Spawner; 
	private static Data _instance;
	private bool _isPlaying;

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
	
	private void InitializeTimer()
	{
		MaxAvatarSpawnTimer = 3f;
		MaxEnemySpawnTimer = 5f;
		CurrentAvatarSpawnTimer = MaxAvatarSpawnTimer;
		CurrentEnemySpawnTimer = MaxEnemySpawnTimer;
		_isPlaying = true;
	}

	private void InitializePlayerValue()
	{
//		CurrentLevel = 0;
//		_lastLevelReached = 0;
//		_currentSessionScore = 0; 
	}

	void Update ()
	{
		if (_isPlaying)
		{
			CurrentAvatarSpawnTimer -= Time.deltaTime;
//			CurrentEnemySpawnTimer -= Time.deltaTime;
		}
//		Timer.GetComponent<Text>().text = CurrentTimer.ToString("F3");
//
		if (CurrentAvatarSpawnTimer <= 0)
		{
			var avatar = Resources.Load("Prefab/Avatar");
			var spawnedAvatar = Spawner.GetComponent<SpawnerController>().Spawn((GameObject)avatar);
			spawnedAvatar.GetComponent<AvatarController>().SetSprite(AlliedSprites[Random.Range(0, AlliedSprites.Length)]); 
			CurrentAvatarSpawnTimer = MaxAvatarSpawnTimer;
		}
		
		if (CurrentEnemySpawnTimer <= 0)
		{
			var enemy = Resources.Load("Prefab/Enemy");
			var spawnedEnemy = Spawner.GetComponent<SpawnerController>().Spawn((GameObject)enemy);
			spawnedEnemy.GetComponent<EnemyController>().SetSprite(EnemySprites[Random.Range(0, EnemySprites.Length)]);
			CurrentEnemySpawnTimer = MaxEnemySpawnTimer;
		}
	}

	
	private void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GameOver()
	{
		CurrentAvatarSpawnTimer = MaxAvatarSpawnTimer;
		CurrentEnemySpawnTimer = MaxEnemySpawnTimer;
		_isPlaying = false;
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

	public int GetCurrentScore()
	{
		return _currentSessionScore;
	}
}
