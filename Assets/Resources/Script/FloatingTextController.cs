using UnityEngine;

public class FloatingTextController : MonoBehaviour {
	private static FloatingText popupText;
	private static GameObject canvas;


	public static void CreateFloatingText(string text, Transform location)
	{
		Debug.LogError(text + " ::: "+ location.position);
		popupText = Resources.Load<FloatingText>("Prefab/PopupTextParent");
		FloatingText instance = Instantiate(popupText);
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-.2f, .2f), location.position.y + Random.Range(-.2f, .2f)));
		canvas = GameObject.Find("Canvas");
		instance.transform.SetParent(canvas.transform, false);
		instance.transform.position = screenPosition;
		instance.SetText(text);
	}
}