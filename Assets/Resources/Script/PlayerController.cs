using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
	private int _CurrentListIndex;
	public GameObject NewHero;
	public GameObject SpawnedHero;
	private bool _isCollectingHero;
	private bool _isSwappingHero;
	private bool _isColliderOverlapped;
	private GameObject _collectedHero;

	private float _yAngleRotation;


	void Start ()
	{
		_walkSpeed = 0.5f;
		_CurrentListIndex = -1;
		_vector = Vector2.up;
		CurrentHealth = 3;
		CurrentAttack = 1;
		CurrentType = 1;
		_totalHealth = CurrentHealth;
//		HeroList.Insert(0,transform);
		UpdateTotalHealth();
		InvokeRepeating("Movement", 0.1f, _walkSpeed);
	}
	
	void Update ()
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
			if(_isSwappingHero && HeroList.Count == 0) return;
				SwapAvatarRightIfAppropriate();
			StartCoroutine(ResetSwapCountdown());
			_isSwappingHero = true;
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if(_isSwappingHero && HeroList.Count == 0) return;
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
				Data.Instance.GameOver(); // GameOver();
			}
		}
		
		
	}

	private void SwapToReservedHeroAndDestroyCurrentHero()
	{
		if (_CurrentListIndex == HeroList.Count)
		{
			Mathf.Clamp(_CurrentListIndex--, 0, HeroList.Count-1);
		}
		else
		{
			Mathf.Clamp(_CurrentListIndex++, 0, HeroList.Count-1);
		}
		Debug.LogError(_CurrentListIndex);
		var nextAvatar = HeroList[Mathf.Clamp(_CurrentListIndex,0,HeroList.Count-1)].gameObject.GetComponent<FriendlyHeroController>();
		if (nextAvatar != null)
		{
			CurrentHealth = nextAvatar.Health;
			CurrentAttack = nextAvatar.Attack;
			CurrentShield = nextAvatar.Shield;
			CurrentType = nextAvatar.Type;
			gameObject.GetComponent<SpriteRenderer>().sprite = nextAvatar.gameObject.GetComponent<SpriteRenderer>().sprite;
			HeroList.RemoveAt(_CurrentListIndex);
			Destroy(nextAvatar.gameObject);
		}
		else
		{
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
			if (_CurrentListIndex >= HeroList.Count - 1)
				_CurrentListIndex = 0;
			else
				Mathf.Clamp(_CurrentListIndex++, 0, HeroList.Count);
			SwapAvatar(_CurrentListIndex);
		}
	}
	
	private void SwapAvatarLeftIfAppropriate()
	{
		if (HeroList.Count > 0)
		{
			if (_CurrentListIndex < 0)
				_CurrentListIndex = HeroList.Count - 1;
			else
				Mathf.Clamp(_CurrentListIndex--, 0, HeroList.Count);
			SwapAvatar(_CurrentListIndex);
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
		if (_isCollectingHero)
		{
			HeroList.Insert(0, SpawnedHero.transform);
			_isCollectingHero = false;
		}
		else if (HeroList.Count > 0)
		{
			HeroList[0].position = ta;
			HeroList.Insert(0, HeroList.Last());
			HeroList.RemoveAt(HeroList.Count - 1);
		}
		Debug.LogError("translate coord : "+_moveVector);
		transform.Translate(_moveVector);
	}



	private void InitializeNewHero(GameObject g)
	{
		if (_collectedHero != null)
		{
			g.GetComponent<SpriteRenderer>().sprite = _collectedHero.GetComponent<SpriteRenderer>().sprite;
			var avatarController = _collectedHero.GetComponent<AvatarController>();
			g.GetComponent<FriendlyHeroController>().SetHeroStat(avatarController.Health, avatarController.Attack,avatarController.Shield, avatarController.Type);
		}
	}

	private void UpdateTotalHealth()
	{
		_totalHealth = CurrentHealth;
		for (var i = 0; i < HeroList.Count; i++)
		{
			var friendlyStat = HeroList[i].gameObject.GetComponent<FriendlyHeroController>();
			if (friendlyStat != null)
				_totalHealth = _totalHealth + friendlyStat.Health;
			else
				return;
		}
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.CompareTag("Level"))
		{
			Debug.LogError("level trigger collided");
			_vector = -_vector;
			if(HeroList.Count > 0)
				SwapToReservedHeroAndDestroyCurrentHero();
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

	private void OnTriggerStay2D(Collider2D c)
	{
		if (c.gameObject.CompareTag("Level"))
		{
			Debug.LogError("trigger stay");
			_isColliderOverlapped = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Level"))
		{
			_isColliderOverlapped = false;
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
		var enemyAttack = enemyInfo.Attack;
		if (CurrentType == enemyInfo.Type)
			enemyAttack = enemyAttack * 2;
		var totalDamage = Mathf.Clamp(enemyAttack - CurrentShield, 1, Int32.MaxValue);
		CurrentHealth = CurrentHealth - totalDamage;
		FloatingTextController.CreateFloatingText(totalDamage.ToString(), transform);
	}

	public void CombatResolved()
	{
		CancelInvoke("Movement");
		_walkSpeed -= 0.015f;
		Data.Instance.AlterSpawnTime();
		Debug.LogError("moveSpeed "+ _walkSpeed);
		InvokeRepeating("Movement", 0.5f, _walkSpeed);
		Data.Instance.AddPoint(_totalHealth);
	}
}
