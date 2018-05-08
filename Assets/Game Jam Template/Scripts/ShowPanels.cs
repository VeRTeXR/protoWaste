using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject OptionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject OptionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject MenuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject PausePanel;							//Store a reference to the Game Object PausePanel
	public GameObject GameplayPanel;

	public void ShowOptionsPanel()
	{
		OptionsPanel.SetActive(true);
		OptionsTint.SetActive(true);
	}

	public void HideOptionsPanel()
	{
		OptionsPanel.SetActive(false);
		OptionsTint.SetActive(false);
	}

	public void ShowMenu()
	{
		MenuPanel.SetActive (true);
	}

	public void HideMenu()
	{
		MenuPanel.SetActive (false);
	}

	public void ShowGameplay()
	{
		GameplayPanel.SetActive(true);
	}

	public void HideGameplay()
	{
		GameplayPanel.SetActive(false);
	}
	
	public void ShowPausePanel()
	{
		PausePanel.SetActive (true);
		OptionsTint.SetActive(true);
	}

	public void HidePausePanel()
	{
		PausePanel.SetActive (false);
		OptionsTint.SetActive(false);
	}
}
