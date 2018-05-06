using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPartyPosition : MonoBehaviour
{
	public Vector2 FollowPos;
	public int MinDistanceBetweenMember = 10;
	
	void Update () {
		if (Vector2.Distance(FollowPos, transform.position) > MinDistanceBetweenMember)
		{
			Vector2 Orientation = (FollowPos - (Vector2)transform.position).normalized;
			transform.position = Orientation * 5f;
		}
	}

	public void SetFollowPosition(Vector2 pos)
	{
		FollowPos = pos;
	}
}
