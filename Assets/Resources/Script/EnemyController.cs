using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public int Health;
	public int Attack;
	public int Type;
	void Start ()
	{
		Health = Random.Range(1, 3);
		Attack = Random.Range(1, 3);
		Type = Random.Range(1,3);
	}

	public void Engage(int avatarType, int damage)
	{
		if (Health <= 0) return;
			if (avatarType == Type)
			{
				Health = Health - (damage * 2);
			}
			else
			{
				Health = Health - damage;
			}
		Debug.LogError("hp: "+Health);
	}

	void Update()
	{
		if (Health <= 0)
		{
			CombatResoved();
//			Destroy(gameObject);
		}
	}

	private void CombatResoved()
	{
		Data.Instance.Player.GetComponent<PlayerController>().CombatResolved();
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		if (c.gameObject.CompareTag("Player"))
		{
			var playerController = c.gameObject.GetComponent<PlayerController>();
			Engage(playerController.CurrentType, playerController.CurrentAttack);	
		}
	}
	
}
