using UnityEngine;
using System.Collections;

public class OrganInteraction : MonoBehaviour {

	void OnMouseDown(){
		if (gameObject.GetComponent<Rigidbody> () == null) {
			Rigidbody gameObjectsRigidBody = gameObject.AddComponent<Rigidbody> ();
		}
	}

	void OnMouseDrag(){
		float dist = transform.position.z - Camera.main.transform.position.z;
		Vector3 pos = Input.mousePosition;
		pos.z = dist;
		pos = Camera.main.ScreenToWorldPoint(pos);
		pos.y = transform.position.y;
		transform.position = pos;
	}
}
