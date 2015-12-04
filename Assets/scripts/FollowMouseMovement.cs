using UnityEngine;
using System.Collections;

public class FollowMouseMovement : MonoBehaviour {
	
	void Start () {
	
	}

	void Update () {

		// Move with mouse
		float dist = transform.position.z - Camera.main.transform.position.z;
		Vector3 pos = Input.mousePosition;
		pos.z = dist;
		pos = Camera.main.ScreenToWorldPoint(pos);
		pos.y = transform.position.y;
		transform.position = pos;
	
	}
}
