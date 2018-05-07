using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPartyPosition : MonoBehaviour
{
	public Vector2 FollowPos;
	public int MinDistanceBetweenMember = 10;
	
//	void Update () {
//		if (Vector2.Distance(FollowPos, transform.position) > MinDistanceBetweenMember)
//		{
//			Vector2 Orientation = (FollowPos - (Vector2)transform.position).normalized;
//			transform.position = Orientation * 5f;
//		}
//	}
//
//	public void SetFollowPosition(Vector2 pos)
//	{
//		FollowPos = pos;
//	}
	
	 public List<GameObject> spawnableObjects;
 
    // the vertical position of the ground
    public int groundLevel;
 
    // objects on these layers will not get spawned on top of
    public LayerMask doNotOverlapOnSpawn;
 
    // called to spawn a new object
    public void Spawn()
    {
        // pick next prefab randomly
        int randomIndex = (int)(Random.value * (spawnableObjects.Count - 1));
        GameObject prefabToSpawn = spawnableObjects[randomIndex];
 
        // choose a position that doesnt overlap anything on the layermask
        Vector3 randomPositionX = getValidSpawnLocation(prefabToSpawn);
 
        // create the object at the new position
        Instantiate(prefabToSpawn, randomPositionX, Quaternion.identity);
    }
 
    private Vector3 getValidSpawnLocation(GameObject go)
    {
        // get the spawning object's collider to use for size
        Collider2D objectCollider = go.GetComponent<Collider2D>();
 
        Vector3 newPosition = Vector3.zero;
        bool validPosition = false;
        do
        {
            // get a random position horizontally on screen
            newPosition = Camera.main.ViewportToWorldPoint(Vector3.right * Random.value);
 
            // set it to ground level height
            newPosition.y = groundLevel;
 
            // match this spawner's z value;
 
            // get the corners of the object at the new position
            Vector3 min = newPosition - objectCollider.bounds.extents;
            Vector3 max = newPosition + objectCollider.bounds.extents;
 
            // check that area for overlaps with the layermask
            Collider2D[] overlapObjects = Physics2D.OverlapAreaAll(min, max, doNotOverlapOnSpawn);
 
            // if it does not overlap any objects in the layermask
            if(overlapObjects.Length == 0)
            {
                // break out of the do while loop
                validPosition = true;
            }
        } while(!validPosition);
 
        // return the valid position\
        return newPosition;
    }
}
