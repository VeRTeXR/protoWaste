using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private float _walkSpeed;
	private int _faceDirection;
	private float _currentWalkCountdownInterval;
	private float _keyDownInterval;
	private float _currentKeyDownInterval;
	private bool _hasPlayerInputBeenProcessed;
	private int _totalHealth;
	
	private bool _vertical = false;
	private bool _horizontal = true;
	private Vector2 _vector = Vector2.up;
	private Vector2 _moveVector;
	
	
	public int CurrentHealth;
	public int CurrentAttack;
	public int CurrentShield;
	public int CurrentType;

	public List<Transform> HeroList;
	public GameObject NewHero;
	public GameObject SpawnedHero;
	private bool _isCollectingHero;
	private GameObject _collectedHero;

	void Start ()
	{
		_walkSpeed = 1f * Mathf.Clamp(Data.Instance.CurrentLevel, 1, 99);
		_keyDownInterval = 0.25f;
		_currentKeyDownInterval = _keyDownInterval;

		CurrentHealth = 3;
		CurrentAttack = 1;
		CurrentType = 1;
		_totalHealth = CurrentHealth;
		InvokeRepeating("Movement", 0.5f, 1);
	}
	
	void Update ()
	{
//		ProcessingInput();
//		MovePlayer();
		
//		if (_hasPlayerInputBeenProcessed)
//			HandlePlayerInputCooldownInterval();

		if (Input.GetKey (KeyCode.D) && _horizontal) {
			_horizontal = false;
			_vertical = true;
			_vector = Vector2.right;
		} else if (Input.GetKey (KeyCode.W) && _vertical) {
			_horizontal = true;
			_vertical = false;
			_vector = Vector2.up;
		} else if (Input.GetKey (KeyCode.S) && _vertical) {
			_horizontal = true;
			_vertical = false;
			_vector = -Vector2.up;
		} else if (Input.GetKey (KeyCode.A) && _horizontal) {
			_horizontal = false;
			_vertical = true;
			_vector = -Vector2.right;
		}
		_moveVector = _vector / 3f;

		if (CurrentHealth <= 0)
		{
			if (HeroList.Count > 0)
			{
				// TODO:: Swap Current Avatar
			}
			else
			{
				// GameOver();
			}
		}
		
		if (Input.GetKey(KeyCode.Space))
		{
			Data.Instance.ResetPlayerPref();
		}
	}

	private void Movement()
	{
		Vector2 ta = transform.position;
		if (_isCollectingHero)
		{
			HeroList.Insert(0, SpawnedHero.transform);
			UpdateTotalHealth();
			_isCollectingHero = false;
		}
		else if (HeroList.Count > 0)
		{
			HeroList.Last().position = ta;
			HeroList.Insert(0, HeroList.Last());
			HeroList.RemoveAt(HeroList.Count - 1);
		}
		transform.Translate(_moveVector);
	}

	private void InitializeNewHero(GameObject g)
	{
		if (_collectedHero != null)
		{
			g.GetComponent<SpriteRenderer>().sprite = _collectedHero.GetComponent<SpriteRenderer>().sprite;
			var avatarController = _collectedHero.GetComponent<AvatarController>();
			g.GetComponent<FriendlyHeroController>().SetHeroStat(avatarController.Health, avatarController.Attack, avatarController.Type);	
		}
	}

	private void UpdateTotalHealth()
	{
		_totalHealth = 0;
		for (var i = 0; i < HeroList.Count; i++)
		{
			_totalHealth += HeroList[i].gameObject.GetComponent<FriendlyHeroController>().Health;
			Debug.LogError("i : "+ i +" : "+HeroList[i].gameObject.GetComponent<FriendlyHeroController>().Health);
		}
		_totalHealth += CurrentHealth;
		Debug.LogError("_total H : "+_totalHealth);
	}

	private void OnCollisionEnter2D(Collision2D c)
	{
		if (c.gameObject.CompareTag("Level"))
		{
			_vector = -_vector;
		}

		if (c.gameObject.CompareTag("Enemy"))
		{
			EngageEnemy(c.gameObject);
		}

		if (c.gameObject.CompareTag("Avatar"))
		{
			CollectAvatar(c.gameObject);
			SpawnedHero = Instantiate(NewHero, transform.position, Quaternion.identity);
			InitializeNewHero(SpawnedHero);
			Destroy(c.gameObject);
		}
	}

	private void CollectAvatar(GameObject cGameObject)
	{
		_collectedHero = cGameObject;
		_isCollectingHero = true;	
	}

	private void EngageEnemy(GameObject collideGameObject)
	{
		var enemyInfo = collideGameObject.GetComponent<EnemyController>();
		var enemyAttack = enemyInfo.Attack - CurrentShield;
		if (CurrentType == enemyInfo.Type)
			CurrentHealth -= enemyAttack * 2;
		else
			CurrentHealth -= enemyAttack;
	}
	
	public void CombatResolved()
	{
		Data.Instance.AddPoint(_totalHealth);
	}
}
