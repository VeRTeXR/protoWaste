using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyHeroController : MonoBehaviour {
	public int Health;
	public int Attack;
	public int Type;
	public int Shield;


	public void SetHeroStat(int health, int attack, int shield, int type)
	{
		Health = health;
		Attack = attack;
		Type = type;
		Shield = shield;
	}
	
}
