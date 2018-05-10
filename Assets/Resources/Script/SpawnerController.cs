using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public int groundLevel;
    public LayerMask doNotOverlapOnSpawn;

    public GameObject Spawn(GameObject spawnableObject)
    {
        GameObject prefabToSpawn = spawnableObject;

        Vector3 randomPosition = GetValidSpawnLocation(prefabToSpawn);

        var spawnedObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
        return spawnedObject;
    }

    private Vector3 GetValidSpawnLocation(GameObject go)
    {
        Collider2D objectCollider = go.GetComponent<Collider2D>();

        Vector3 newPosition = Vector3.zero;
        bool validPosition = false;
        do
        {
            newPosition.x = Camera.main.ViewportToWorldPoint(Vector3.right * Random.value).x;

            newPosition.y = Camera.main.ViewportToWorldPoint(Vector3.up * Random.value).y;

            newPosition.z = transform.position.z;

            Vector3 min = newPosition - objectCollider.bounds.extents;
            Vector3 max = newPosition + objectCollider.bounds.extents;

            Collider2D[] overlapObjects = Physics2D.OverlapAreaAll(min, max, doNotOverlapOnSpawn);

            if (overlapObjects.Length == 0)
            {
                validPosition = true;
            }
        } while (!validPosition);

        return newPosition;
    }
}
