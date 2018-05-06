using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
	private int _CurrentListIndex;
	public GameObject NewHero;
	public GameObject SpawnedHero;
	private bool _isCollectingHero;
	private bool _isSwappingHero;
	private GameObject _collectedHero;

	void Start ()
	{
		_walkSpeed = 1f * Mathf.Clamp(Data.Instance.CurrentLevel, 1, 99);
		_keyDownInterval = 0.25f;
		_currentKeyDownInterval = _keyDownInterval;
		_CurrentListIndex = -1;
		CurrentHealth = 3;
		CurrentAttack = 1;
		CurrentType = 1;
		_totalHealth = CurrentHealth;
//		HeroList.Insert(0,transform);
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

		if (Input.GetKeyDown(KeyCode.Q))
		{
			if(_isSwappingHero && HeroList.Count == 0) return;
			SwapCurrentAvatarIfAppropriate();
			StartCoroutine(ResetSwapCountdown());
			_isSwappingHero = true;
		}
		
		if (CurrentHealth <= 0)
		{
			if (HeroList.Count > 0)
			{
				SwapToReservedHero();
				HeroList.RemoveAt(HeroList.Count-1);// TODO:: Swap Current Avatar
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

	private void SwapToReservedHero()
	{
		var nextAvatar = HeroList[Mathf.Clamp(_CurrentListIndex,0,HeroList.Count)].gameObject.GetComponent<FriendlyHeroController>();
		if (nextAvatar != null)
		{
			CurrentHealth = nextAvatar.Health;
			CurrentAttack = nextAvatar.Attack;
			CurrentShield = nextAvatar.Shield;
			CurrentType = nextAvatar.Type;
			gameObject.GetComponent<SpriteRenderer>().sprite = nextAvatar.gameObject.GetComponent<SpriteRenderer>().sprite;
			HeroList.RemoveAt(HeroList.Count - 1);
			Destroy(nextAvatar.gameObject);
		}
	}

	private IEnumerator ResetSwapCountdown()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		_isSwappingHero = false;
	}

	public int GetTotalHealth()
	{
		return _totalHealth;
	}

	private void SwapCurrentAvatarIfAppropriate()
	{
		if (HeroList.Count > 0)
		{
			var nextAvatar = HeroList[Mathf.Clamp(_CurrentListIndex,0,HeroList.Count)].gameObject.GetComponent<FriendlyHeroController>();
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
			
			if (_CurrentListIndex >= HeroList.Count-1)
				_CurrentListIndex = -1;
			else
				_CurrentListIndex++;
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
//			for (var i = 0; i < HeroList.Count; i++)
//			{
//				if (i > 0)
//				{
//					HeroList[i].GetComponent<FollowPartyPosition>().SetFollowPosition(HeroList[i-1].position);
//				}
//				else
//				{
//					HeroList[i].GetComponent<FollowPartyPosition>().SetFollowPosition(transform.position);
//				}
//			}
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
			g.GetComponent<FriendlyHeroController>().SetHeroStat(avatarController.Health, avatarController.Attack,avatarController.Shield, avatarController.Type);
//			g.transform.parent = transform;
		}
	}

	private void UpdateTotalHealth()
	{
		_totalHealth = 0;
		for (var i = 0; i < HeroList.Count; i++)
		{
			if (HeroList[i].gameObject.GetComponent<FriendlyHeroController>())
				_totalHealth = _totalHealth + HeroList[i].gameObject.GetComponent<FriendlyHeroController>().Health;
			else
				_totalHealth = _totalHealth + CurrentHealth;
		}
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
		var enemyAttack = enemyInfo.Attack;
		if (CurrentType == enemyInfo.Type)
			enemyAttack = enemyAttack*2;
		CurrentHealth = CurrentHealth - Mathf.Clamp(enemyAttack- CurrentShield, 1, Int32.MaxValue);
		Debug.LogError("cH : "+CurrentHealth+" attk : "+ enemyAttack);
	}
	
	public void CombatResolved()
	{
		Data.Instance.AddPoint(_totalHealth);
	}
}
