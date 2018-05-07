using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    // the vertical position of the ground
    public int groundLevel;

    // objects on these layers will not get spawned on top of
    public LayerMask doNotOverlapOnSpawn;

    // called to spawn a new object
    public GameObject Spawn(GameObject spawnableObject)
    {
        GameObject prefabToSpawn = spawnableObject;

        // choose a position that doesnt overlap anything on the layermask
        Vector3 randomPosition = GetValidSpawnLocation(prefabToSpawn);

        // create the object at the new position
        var spawnedObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
        return spawnedObject;
    }

    private Vector3 GetValidSpawnLocation(GameObject go)
    {
        // get the spawning object's collider to use for size
        Collider2D objectCollider = go.GetComponent<Collider2D>();

        Vector3 newPosition = Vector3.zero;
        bool validPosition = false;
        do
        {
            // get a random position horizontally on screen
            newPosition.x = Camera.main.ViewportToWorldPoint(Vector3.right * Random.value).x;

            // set it to ground level height
            newPosition.y = Camera.main.ViewportToWorldPoint(Vector3.up * Random.value).y;

            // match this spawner's z value
            newPosition.z = transform.position.z;

            // get the corners of the object at the new position
            Vector3 min = newPosition - objectCollider.bounds.extents;
            Vector3 max = newPosition + objectCollider.bounds.extents;

            // check that area for overlaps with the layermask
            Collider2D[] overlapObjects = Physics2D.OverlapAreaAll(min, max, doNotOverlapOnSpawn);

            // if it does not overlap any objects in the layermask
            if (overlapObjects.Length == 0)
            {
                // break out of the do while loop
                validPosition = true;
            }
        } while (!validPosition);

        // return the valid position
        return newPosition;
    }
}
