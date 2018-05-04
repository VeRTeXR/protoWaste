using UnityEngine;

public class LevelController : MonoBehaviour
{
	public GameObject Exit;

	private GameObject _exit;
	
	// Use this for initialization
	void Start ()
	{
		Exit = (GameObject) Resources.Load("Prefabs/Exit");
		SpawnExit();
		
	}

	private void SpawnExit()
	{
			_exit = Instantiate(Exit);
		
		var origin = transform.position;
		var range = transform.localScale / 2.0f;
		var randomRange = new Vector3(Random.Range(-range.x, range.x),
			Random.Range(-range.y, range.y),
			Random.Range(-range.z, range.z));
		var randomCoordinate = origin + randomRange;
		_exit.transform.localPosition = randomCoordinate;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position, transform.localScale);
		
	}
}
