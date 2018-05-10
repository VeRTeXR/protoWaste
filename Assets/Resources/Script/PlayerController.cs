using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
	private float _walkSpeed;
	private int _faceDirection;
	private float _currentWalkCountdownInterval;
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
	private int _currentListIndex;
	public GameObject NewHero;
	public GameObject SpawnedHero;
	private bool _isSwappingHero;
	private bool _isColliderOverlapped;
	private GameObject _collectedHero;

	private float _yAngleRotation;
	private int _overlappingTimer;


	void Start()
	{
		InitializePlayer();
		UpdateTotalHealth();
		InvokeRepeating("Movement", 0.1f, _walkSpeed);
	}

	private void InitializePlayer()
	{
		_walkSpeed = 0.75f;
		_currentListIndex = -1;
		_vector = Vector2.up;
		CurrentHealth = Random.Range(1,5);
		CurrentShield = Random.Range(1, 5);
		CurrentAttack = Random.Range(1,5);
		CurrentType = Random.Range(1,3);
		_totalHealth = CurrentHealth;
	}

	void Update()
	{
		if (!_isColliderOverlapped)
		{
			if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (_hasPlayerInputBeenProcessed) return;
				if (_vector == Vector2.right)
					_vector = -Vector2.up;
				else if (_vector == -Vector2.up)
					_vector = -Vector2.right;
				else if (_vector == -Vector2.right)
					_vector = Vector2.up;
				else if (_vector == Vector2.up)
					_vector = Vector2.right;
				StartCoroutine(ResetPlayerInputCountdown());
				_hasPlayerInputBeenProcessed = true;
			}
			else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (_hasPlayerInputBeenProcessed) return;
				if (_vector == Vector2.right)
					_vector = Vector2.up;
				else if (_vector == -Vector2.up)
					_vector = Vector2.right;
				else if (_vector == -Vector2.right)
					_vector = -Vector2.up;
				else if (_vector == Vector2.up)
					_vector = -Vector2.right;
				StartCoroutine(ResetPlayerInputCountdown());
				_hasPlayerInputBeenProcessed = true;
			}
		}
		_moveVector = _vector / 6f;

		if (_vector == Vector2.right)
			GetComponent<SpriteRenderer>().flipX = false;
		else if (_vector == -Vector2.right)
			GetComponent<SpriteRenderer>().flipX = true;


		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (_isSwappingHero && HeroList.Count == 0) return;
			SwapAvatarRightIfAppropriate();
			StartCoroutine(ResetSwapCountdown());
			_isSwappingHero = true;
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (_isSwappingHero && HeroList.Count == 0) return;
			SwapAvatarLeftIfAppropriate();
			StartCoroutine(ResetSwapCountdown());
			_isSwappingHero = true;
		}

		if (CurrentHealth <= 0)
		{
			if (HeroList.Count > 0)
			{
				SwapToReservedHeroAndDestroyCurrentHero();
			}
			else
			{
				Destroy(gameObject);
				Data.Instance.GameOver();
			}
		}
	}

	private IEnumerator ResetSwapCountdown()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		_isSwappingHero = false;
	}

	private IEnumerator ResetPlayerInputCountdown()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		_hasPlayerInputBeenProcessed = false;
	}

	public int GetTotalHealth()
	{
		UpdateTotalHealth();
		return _totalHealth;
	}

	private void SwapAvatarRightIfAppropriate()
	{
		if (HeroList.Count > 0)
		{
			if (_currentListIndex >= HeroList.Count - 1)
				_currentListIndex = 0;
			else
				Mathf.Clamp(_currentListIndex++, 0, HeroList.Count - 1);
			SwapAvatar(_currentListIndex);
		}
	}

	private void SwapAvatarLeftIfAppropriate()
	{
		if (HeroList.Count > 0)
		{
			if (_currentListIndex <= 0)
				_currentListIndex = HeroList.Count - 1;
			else
				Mathf.Clamp(_currentListIndex--, 0, HeroList.Count - 1);
			SwapAvatar(_currentListIndex);
		}
	}

	private void SwapToReservedHeroAndDestroyCurrentHero()
	{
		if (_currentListIndex == HeroList.Count - 1 && _currentListIndex != 0)
			Mathf.Clamp(_currentListIndex--, 0, HeroList.Count - 1);
		else if (_currentListIndex < HeroList.Count - 1 && _currentListIndex != 0)
			Mathf.Clamp(_currentListIndex++, 0, HeroList.Count - 1);

		var nextAvatar = HeroList[Mathf.Clamp(_currentListIndex, 0, HeroList.Count - 1)].gameObject
			.GetComponent<FriendlyHeroController>();
		if (nextAvatar != null)
		{
			CurrentHealth = nextAvatar.Health;
			CurrentAttack = nextAvatar.Attack;
			CurrentShield = nextAvatar.Shield;
			CurrentType = nextAvatar.Type;
			gameObject.GetComponent<SpriteRenderer>().sprite = nextAvatar.gameObject.GetComponent<SpriteRenderer>().sprite;
			HeroList.RemoveAt(_currentListIndex);
			Destroy(nextAvatar.gameObject);
		}
	}

	private void SwapAvatar(int nextAvatarIndex)
	{
		var nextAvatar = HeroList[Mathf.Clamp(nextAvatarIndex, 0, HeroList.Count)].gameObject
			.GetComponent<FriendlyHeroController>();
		if (nextAvatar != null)
		{
			var tempHealth = CurrentHealth;
			var tempAttack = CurrentAttack;
			var tempShield = CurrentShield;
			var tempType = CurrentType;
			var tempSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
			CurrentHealth = nextAvatar.Health;
			CurrentAttack = nextAvatar.Attack;
			CurrentShield = nextAvatar.Shield;
			CurrentType = nextAvatar.Type;
			gameObject.GetComponent<SpriteRenderer>().sprite = nextAvatar.gameObject.GetComponent<SpriteRenderer>().sprite;
			nextAvatar.Health = tempHealth;
			nextAvatar.Attack = tempAttack;
			nextAvatar.Shield = tempShield;
			nextAvatar.Type = tempType;
			nextAvatar.gameObject.GetComponent<SpriteRenderer>().sprite = tempSprite;
		}
	}


	private void Movement()
	{
		Vector2 ta = transform.position;
		if (HeroList.Count > 0)
		{
			HeroList[0].position = ta;
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
			g.GetComponent<FriendlyHeroController>().SetHeroStat(avatarController.Health, avatarController.Attack,
				avatarController.Shield, avatarController.Type);
		}
	}

	private void UpdateTotalHealth()
	{
		_totalHealth = CurrentHealth;
		for (var i = 0; i < HeroList.Count; i++)
		{
			if (HeroList[i] != null)
			{
				var friendlyStat = HeroList[i].gameObject.GetComponent<FriendlyHeroController>();
				_totalHealth = _totalHealth + friendlyStat.Health;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.CompareTag("Level"))
		{
			_vector = -_vector;
			if (HeroList.Count > 0)
				SwapToReservedHeroAndDestroyCurrentHero();
			else
			{
				Destroy(gameObject);
				Data.Instance.GameOver();
			}
		}

		if (c.gameObject.CompareTag("Enemy"))
		{
			EngageEnemy(c.gameObject);
		}

		if (c.gameObject.CompareTag("Avatar"))
		{
			SpawnedHero = null;
			CollectAvatar(c.gameObject);
			SpawnedHero = Instantiate(NewHero, transform.position, Quaternion.identity);
			InitializeNewHero(SpawnedHero);
			HeroList.Insert(0, SpawnedHero.transform);
			Destroy(c.gameObject);
		}
	}

	private void OnTriggerStay2D(Collider2D c)
	{
		if (c.gameObject.CompareTag("Level"))
		{
			_isColliderOverlapped = true;
			if (_isColliderOverlapped)
			{
				_overlappingTimer++;
				if (_overlappingTimer > 3)
				{
					if (c.gameObject.name == "TopBound")
						_vector = -Vector2.up;
					else if (c.gameObject.name == "BottomBound")
						_vector = Vector2.up;
					else if (c.gameObject.name == "RightBound")
						_vector = -Vector2.right;
					else if (c.gameObject.name == "LeftBound")
						_vector = Vector2.right;
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Level"))
		{
			_isColliderOverlapped = false;
		}
	}

	private void CollectAvatar(GameObject cGameObject)
	{
		_collectedHero = cGameObject;
	}

	private void EngageEnemy(GameObject collideGameObject)
	{
		var enemyInfo = collideGameObject.GetComponent<EnemyController>();
		var enemyAttack = enemyInfo.Attack;
		if (CurrentType == enemyInfo.Type)
			enemyAttack = enemyAttack * 2;
		var totalDamage = Mathf.Clamp(enemyAttack - CurrentShield, 1, Int32.MaxValue);
		CurrentHealth = CurrentHealth - totalDamage;
		CancelInvoke("Movement");
		var engageEnemyPanelController = Data.Instance.StartMenu.GetComponent<ShowPanels>().EngageEnemyPanel
			.GetComponent<EngageEnemyPanelController>();
		engageEnemyPanelController.SetData(gameObject, collideGameObject);
		engageEnemyPanelController.EnableEngagePanel(true);
		StartCoroutine(InvokeAfterEngageSequence());
		FloatingTextController.CreateFloatingText(totalDamage.ToString(), transform);
	}

	private IEnumerator InvokeAfterEngageSequence()
	{
		yield return new WaitForSecondsRealtime(0.6f);
		InvokeRepeating("Movement", 0.5f, Mathf.Clamp(_walkSpeed, 0.1f, 1f));
	}

	public void CombatResolved()
	{
		_walkSpeed -= 0.035f;
		Data.Instance.AlterSpawnTime();
		if (_totalHealth > 0)
			Data.Instance.AddPoint(_totalHealth);
	}
}
