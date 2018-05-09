using System.Collections;
using UnityEngine;


public class StartOptions : MonoBehaviour {
	public bool InMainMenu = true;
	public Animator AnimColorFade;
	public Animator AnimMenuAlpha;
	public AnimationClip FadeColorAnimationClip;
	public AnimationClip FadeAlphaAnimationClip;
	public Pause PauseScript;
	public bool ChangeMusicOnStart;

	private PlayMusic _playMusic;
	private float fastFadeIn = .01f;
	private ShowPanels _showPanels;
	private GameObject _player;
	private GameObject _startOptionSelector; 

	public void Awake()
	{
		InitializeMenuAndPauseGameplay();
	}

	private void InitializeMenuAndPauseGameplay()
	{
		_showPanels = GetComponent<ShowPanels>();
		PauseScript = GetComponent<Pause>();
		_playMusic = GetComponent<PlayMusic>();
		_player = GameObject.FindGameObjectWithTag("Player");

		SetPlayerState(false);
		SetUnscaleUiAnimatorUpdateMode();
		PauseScript.DoPause();
	}

	public void SetPlayerState(bool isEnabled)
	{
//		if (_player != null)
//		{
//			_player.gameObject.GetComponent<Player>().enabled = isEnabled;
//			_player.gameObject.GetComponent<Controller2D>().enabled = isEnabled;
//		}
	}

	private void SetUnscaleUiAnimatorUpdateMode()
	{
		AnimMenuAlpha.updateMode = AnimatorUpdateMode.UnscaledTime;
		AnimColorFade.updateMode = AnimatorUpdateMode.UnscaledTime;
	}


	public void StartButtonClicked()
	{
		FadeOutMusicOnStartIfAppropriate();
		SetPlayerState(true);
		StartGameInScene();
	}

	private void FadeOutMusicOnStartIfAppropriate()
	{
		if (ChangeMusicOnStart)
		{
			_playMusic.FadeDown(FadeColorAnimationClip.length);
		}
	}

	public void StartGameInScene()
	{
		InMainMenu = false;
		ChangeMusicOnStartIfAppropriate();
		FadeAndDisablePanel(_showPanels.MenuPanel);
		StartCoroutine("UnpauseGameAfterMenuFaded");
		_showPanels.ShowGameplay();
	}

	public void SessionOver()
	{
		FadeAndDisablePanel(_showPanels.GameplayPanel);
		_showPanels.ShowResult();
	}
	
	public void Retry()
	{
		FadeAndDisablePanel(_showPanels.ResultPanel);
		_showPanels.ShowGameplay();
	}

	private void FadeAndDisablePanel(GameObject panelGameObject)
	{
		AnimMenuAlpha = panelGameObject.GetComponent<Animator>();
		if (AnimMenuAlpha != null)
		{
			AnimMenuAlpha.SetTrigger("fade");
		}
		StartCoroutine(HidePanelDelayed(panelGameObject));
	}

	private IEnumerator HidePanelDelayed(GameObject panelGameObject)
	{
		yield return new WaitForSecondsRealtime(FadeAlphaAnimationClip.length);
		_showPanels.HidePanel(panelGameObject);
	}

	

	private void FadeAndDisableMenuPanel()
	{
		AnimMenuAlpha.SetTrigger("fade");
		StartCoroutine("HideMenuDelayed");
	}

	private void ChangeMusicOnStartIfAppropriate()
	{
		if (ChangeMusicOnStart)
			Invoke("PlayNewMusic", FadeAlphaAnimationClip.length);
	}

	public IEnumerator UnpauseGameAfterMenuFaded()
	{
		yield return new WaitUntil(() => !_showPanels.MenuPanel.gameObject.activeSelf);
		PauseScript.UnPause();
	}

	public void PlayNewMusic()
	{
		_playMusic.FadeUp (fastFadeIn);
		_playMusic.PlaySelectedMusic (1);
	}
}
