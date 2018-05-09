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
	
	private void OnEnable()
	{
		PlayerSprite = PlayerData.GetComponent<SpriteRenderer>().sprite;
		EnemySprite = EnemyData.GetComponent<SpriteRenderer>().sprite;
		PlayerImage.sprite = PlayerSprite;
		EnemyImage.sprite = EnemySprite;
		PlayerHealth.text = "HP : "+PlayerData.CurrentHealth;
		PlayerAttack.text = "ATK : "+PlayerData.CurrentAttack;
		PlayerDefense.text = "DEF : "+PlayerData.CurrentShield;
		PlayerType.text = "TYP : "+PlayerData.CurrentType;
		EnemyHealth.text = "HP : "+EnemyData.Health;
		EnemyAttack.text = "ATK : "+EnemyData.Attack;
		EnemyDefense.text = "DEF : "+EnemyData.Shield;
		EnemyType.text = "TYP : "+EnemyData.Type;
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
