using UnityEngine;

public class Pause : MonoBehaviour {
	
	private ShowPanels _showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	public bool _isPaused;								//Boolean to check if the game is paused or not
	private StartOptions _startScript;					//Reference to the StartButton script

	void Awake()
	{
		_showPanels = GetComponent<ShowPanels> ();
		_startScript = GetComponent<StartOptions> ();
	}
	
	private void Update () {
		if (Input.GetButtonDown ("Cancel") && !_isPaused && !_startScript.InMainMenu) 
		{
			DoPause();
			_startScript.SetPlayerState(false);
			_showPanels.ShowPausePanel();
		} 
		else if (Input.GetButtonDown ("Cancel") && _isPaused && !_startScript.InMainMenu)
		{
			UnPause ();
			HidePausePanelAndEnablePlayerControl();
		}
	
	}

	private void HidePausePanelAndEnablePlayerControl()
	{
		_startScript.SetPlayerState(true);
		_showPanels.HidePausePanel();
	}


	public void DoPause()
	{
		_isPaused = true;
		Time.timeScale = 0;
	}


	public void UnPause()
	{
		_isPaused = false;
		HidePausePanelAndEnablePlayerControl();
		Time.timeScale = 1;
	}


}
