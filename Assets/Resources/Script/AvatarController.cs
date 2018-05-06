using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AvatarController : MonoBehaviour
{

	public int Health;
	public int Attack;
	public int Shield;
	public int Type;
	
	void Start()
	{
		Health = Random.Range(1, 5);
		Attack = Random.Range(1, 5);
		Shield = Random.Range(1, 5);
		Type = Random.Range(1, 5);
	}
	
	private void OnCollisionEnter2D(Collision2D c)
	{
		if (c.gameObject.CompareTag("Player"))
		{
			transform.parent = null;
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
