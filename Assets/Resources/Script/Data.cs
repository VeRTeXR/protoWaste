using UnityEngine;

public class  Data : MonoBehaviour
{
	public GameObject Player;
	public GameObject StartMenu;
	
	public float MaxAvatarSpawnTimer;
	public float CurrentAvatarSpawnTimer;
	public float MaxEnemySpawnTimer;
	public float CurrentEnemySpawnTimer;
	private int _highScore;
	private int _currentSessionScore;

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

		_highScore = PlayerPrefs.GetInt("HighScore");		
		InitializeGameObject(); 
		InitializeTimer();
		InitializePlayerValue();
	}
	public int CurrentLevel { get; set; }

	private void InitializeGameObject()
	{
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
		_currentSessionScore = 0;
	}

	void Update ()
	{
		if (_isPlaying)
		{
			CurrentAvatarSpawnTimer -= Time.deltaTime;
			CurrentEnemySpawnTimer -= Time.deltaTime;
		}
		
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
	
	public void GameOver()
	{
		CurrentAvatarSpawnTimer = MaxAvatarSpawnTimer;
		CurrentEnemySpawnTimer = MaxEnemySpawnTimer;
		if (_currentSessionScore > _highScore)
		{
			PlayerPrefs.SetInt("HighScore", _currentSessionScore);
		}
		_isPlaying = false;
		StartMenu.GetComponent<StartOptions>().SessionOver();
	}

	public void Retry()
	{
		ResetRemainFromLastSession();
		InitializeGameObject(); 
		InitializeTimer();
		InitializePlayerValue();
		_currentSessionScore = 0;
		StartMenu.GetComponent<StartOptions>().Retry();
	}

	private void ResetRemainFromLastSession()
	{
		var remainedAvatar = GameObject.FindGameObjectsWithTag("Avatar");
		var remainedEnemy = GameObject.FindGameObjectsWithTag("Enemy");
		for (var i = 0; i < remainedAvatar.Length; i++)
			Destroy(remainedAvatar[i].gameObject);
		for (var j = 0; j < remainedEnemy.Length; j++)
			Destroy(remainedEnemy[j].gameObject);
	}

	public void AddPoint(int totalHealth)
	{
		_currentSessionScore += totalHealth;
		if (_currentSessionScore > _highScore)
		{
			_highScore = _currentSessionScore;
		}
	}

	public int GetCurrentScore()
	{
		return _currentSessionScore;
	}

	public void AlterSpawnTime()
	{
		if (MaxAvatarSpawnTimer < 5f)
			MaxAvatarSpawnTimer += 1f;
		if (MaxEnemySpawnTimer > 0.3f)
			MaxEnemySpawnTimer -= 0.05f;
	}

	public int GetHighScore()
	{
		return _highScore;
	}
}
