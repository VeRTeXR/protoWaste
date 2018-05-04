using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyHeroController : MonoBehaviour {
	public int Health;
	public int Attack;
	public int Type;


	public void SetHeroStat(int health, int attack, int type)
	{
		Health = health;
		Attack = attack;
		Type = type;
	}
	
}
