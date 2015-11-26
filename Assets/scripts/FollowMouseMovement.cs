using UnityEngine;
using System.Collections;

public class FollowMouseMovement : MonoBehaviour {
	public Vector3 UltimatePosition = new Vector3(0,0,0);
	float X, Y;
	
	
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {

		float dist = transform.position.z - Camera.main.transform.position.z;
		Vector3 pos = Input.mousePosition;
		pos.z = dist;
		pos = Camera.main.ScreenToWorldPoint(pos);
		pos.y = transform.position.y;
		transform.position = pos;
		print (transform.position);
		
	}
}