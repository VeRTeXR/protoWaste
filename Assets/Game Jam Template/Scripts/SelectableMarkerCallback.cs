using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableMarkerCallback : MonoBehaviour, IPointerEnterHandler
{

	private GameObject _buttonParentGameObject;

	private void Start()
	{
		_buttonParentGameObject = gameObject.transform.parent.gameObject;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_buttonParentGameObject.GetComponent<StartMenuSelection>().DisableMarker();
		EnableMarker(true);
	}

	private void EnableMarker(bool isEnabled)
	{
		transform.GetChild(1).gameObject.SetActive(isEnabled);
	} 
}
