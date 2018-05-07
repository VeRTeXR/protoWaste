using UnityEngine;
using UnityEngine.UI;

public class UITextController : MonoBehaviour
{

	public GameObject CurrentHpText;
	public GameObject TotalHpText;
	public GameObject RemainingAvatarText;
	public GameObject ScoreText;

	void Update()
	{
		if (Data.Instance.Player == null) return;
		UpdateCurrentHpText();
		UpdateRemainingAvatarText();
		UpdateTotalHpText();
		UpdateScoreText();
	}

	public void UpdateCurrentHpText()
	{
		CurrentHpText.GetComponent<Text>().text =
			"CurHP : " + Data.Instance.Player.GetComponent<PlayerController>().CurrentHealth;
	}

	public void UpdateScoreText()
	{
		ScoreText.GetComponent<Text>().text = "Score : " + Data.Instance.GetCurrentScore();
	}

	public void UpdateTotalHpText()
	{
		TotalHpText.GetComponent<Text>().text =
			"TotalHP : " + Data.Instance.Player.GetComponent<PlayerController>().GetTotalHealth();
	}

	public void UpdateRemainingAvatarText()
	{
		RemainingAvatarText.GetComponent<Text>().text =
			"Avatar : " + Data.Instance.Player.GetComponent<PlayerController>().HeroList.Count;
	}
}
