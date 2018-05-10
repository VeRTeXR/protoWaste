using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EngageEnemyPanelController: MonoBehaviour
{

	public PlayerController PlayerData;
	public EnemyController EnemyData;

	public Sprite PlayerSprite;
	public Sprite EnemySprite;
	public Image PlayerImage;
	public Image EnemyImage;
	public Text PlayerHealth;
	public Text PlayerAttack;
	public Text PlayerDefense;
	public Text PlayerType;
	public Text EnemyHealth;
	public Text EnemyAttack;
	public Text EnemyDefense;
	public Text EnemyType;

	private float currentLerpTime;
	private float lerpTime;
	private bool _isAnimating;
	
	private void OnEnable()
	{
		InitializeElementsData();
		transform.localScale = Vector3.zero;
		StartCoroutine(ScaleToSize(transform, Vector3.one, 0.2f));
		StartCoroutine(DisableAfterShown());
	}

	private void InitializeElementsData()
	{
		PlayerSprite = PlayerData.GetComponent<SpriteRenderer>().sprite;
		EnemySprite = EnemyData.GetComponent<SpriteRenderer>().sprite;
		PlayerImage.sprite = PlayerSprite;
		EnemyImage.sprite = EnemySprite;
		PlayerHealth.text = "HP : " + Mathf.Clamp(PlayerData.CurrentHealth,0,Int32.MaxValue);
		PlayerAttack.text = "ATK : " + PlayerData.CurrentAttack;
		PlayerDefense.text = "DEF : " + PlayerData.CurrentShield;
		EnemyHealth.text = "HP : " + Mathf.Clamp(EnemyData.Health,0,Int32.MaxValue);
		EnemyAttack.text = "ATK : " + EnemyData.Attack;
		EnemyDefense.text = "DEF : " + EnemyData.Shield;

		string playerTypeString = "";
		switch (PlayerData.CurrentType)
		{
			case 1:
				playerTypeString = "Fire";
				PlayerType.color = Color.red;
				break;
			case 2:
				playerTypeString = "Water";
				PlayerType.color = Color.blue;
				break;
			case 3:
				playerTypeString = "Air";
				PlayerType.color = Color.green;
				break;
		}
		PlayerType.text = "TYP : " + playerTypeString;
		
		string enemyTypeString = "";
		switch (EnemyData.Type)
		{
			case 1:
				enemyTypeString = "Red";
				EnemyType.color = Color.red;
				break;
			case 2:
				enemyTypeString = "Blue";
				EnemyType.color = Color.blue;
				break;
			case 3:
				enemyTypeString = "Green";
				EnemyType.color = Color.green;
				break;
		}
		EnemyType.text = "TYP : " + enemyTypeString;
	}

	public IEnumerator ScaleToSize(Transform transform, Vector3 size, float timeToMove)
	{
		var currentPos = transform.localScale;
		var t = 0f;
		while (t < 1)
		{
			t += Time.deltaTime / timeToMove;
			transform.localScale = Vector3.Lerp(currentPos, size, t);
			yield return null;
		}
	}

	private IEnumerator DisableAfterShown()
	{
		yield return new WaitForSecondsRealtime(0.5f);
		StartCoroutine(ScaleToSize(transform, Vector3.zero, 0.1f));
		yield return new WaitForSecondsRealtime(0.1f);
		EnableEngagePanel(false);
	}

	public void EnableEngagePanel(bool isEnable)
	{
		gameObject.SetActive(isEnable);
	}

	public void SetData(GameObject playerGameObject, GameObject enemyGameObject)
	{
		PlayerData = playerGameObject.GetComponent<PlayerController>();
		EnemyData = enemyGameObject.GetComponent<EnemyController>();
	}
}
