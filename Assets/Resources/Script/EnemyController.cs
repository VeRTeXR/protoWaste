using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public int Health;
	public int Attack;
	public int Shield;
	public int Type;
	void Start ()
	{
		Health = Random.Range(1, 3);
		Attack = Random.Range(1, 3);
		Shield = Random.Range(1, 3);
		Type = Random.Range(1,3);
	}

	public void Engage(int avatarType, int damage)
	{
		if (avatarType == Type)
			damage = damage * 2;
		
		damage = Mathf.Clamp(damage - Shield,1, int.MaxValue);
		FloatingTextController.CreateFloatingText(damage.ToString(), transform);
		if (Health <= 0) return;
				Health = Health - damage;
	}

	void Update()
	{
		if (Health <= 0)
		{
			CombatResoved();
			Destroy(gameObject);
		}
	}

	private void CombatResoved()
	{
		Data.Instance.Player.GetComponent<PlayerController>().CombatResolved();
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.CompareTag("Player"))
		{
			var playerController = c.gameObject.GetComponent<PlayerController>();
			Engage(playerController.CurrentType, playerController.CurrentAttack);	
			
		}
	}

	public void SetSprite(Sprite enemySprite)
	{
		GetComponent<SpriteRenderer>().sprite = enemySprite;
	}
}
