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

		Vector3 pos = Input.mousePosition;
		pos.z = transform.position.z - Camera.main.transform.position.z;
		transform.position = Camera.main.ScreenToWorldPoint(pos);
		print (transform.position);

		//Vector3 mouselocation = Camera.main.camera.ScreenToWorldPoint(Input.mousePosition);
		//transform.position = mouselocation;
		//print("MouseX: " + X + " MouseY: " + Y);
		
	}
}