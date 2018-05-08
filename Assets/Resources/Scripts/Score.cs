using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour{

	public static Score instance = null; 
	
	public Text scoreGUIText;
	public Text highScoreGUIText;
	public int GUIhighScore;
	public string name; // let Player input name somehow
	private int score;
	private int highScore;
	private string highScoreKey = "highScore";
	private string playerName = "playerName";

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		Initialise ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If the Score is higher than the High Score…
		if (highScore < score) {
			highScore = score;
		}
		//Debug.Log (score);
		// Display both the Score and High Score
		if (scoreGUIText == null || highScoreGUIText == null) return;
		scoreGUIText.text = score.ToString ();
		highScoreGUIText.text = "HighScore : " + highScore.ToString();
	}

	private void Initialise () {
		highScore = PlayerPrefs.GetInt (highScoreKey, 0);
		Debug.Log (highScore);
		GUIhighScore = highScore;
	}

	public void AddPoint(int point)
	{
		score = score + point;
	}

	public void Save() {
		//PlayerPrefs.SetName and shit 
		//Saving after scene ends
		//PlayerPrefs.SetString(playerName, name);
		PlayerPrefs.SetInt (highScoreKey, highScore);
		PlayerPrefs.Save ();
		Initialise ();
	}

	public void Reset()
	{
		score = 0;
	}
}

