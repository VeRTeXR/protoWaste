using UnityEngine;
using UnityEngine.UI;

public class ResultPanelController : MonoBehaviour
{
	public GameObject ScoreText;
	public GameObject HighScoreText;

	private void OnEnable()
	{
		for (var i = 0; i < ScoreText.transform.childCount; i++)
		{
			ScoreText.transform.GetChild(i).GetComponent<Text>().text = 
				Data.Instance.GetCurrentScore().ToString();
		}
		
		HighScoreText.SetActive(true);
		if (Data.Instance.GetCurrentScore() < Data.Instance.GetHighScore())
		{
			for (var i = 0; i < HighScoreText.transform.childCount; i++)
				HighScoreText.transform.GetChild(i).GetComponent<Text>().text =
					"Highscore : " + Data.Instance.GetHighScore();
		}
		else
		{
			for (var i = 0; i < HighScoreText.transform.childCount; i++)
				HighScoreText.transform.GetChild(i).GetComponent<Text>().text =
					"NEW HIGHSCORE!";
		}
	}

	private void OnDisable()
	{
		GetComponent<CanvasGroup>().alpha = 1;
		GetComponent<Animator>().ResetTrigger("fade");
	}
}
