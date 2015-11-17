﻿using UnityEngine;
using System.Collections;

public class ToolUser : MonoBehaviour {

	public Material capMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;

			if(Physics.Raycast(transform.position, transform.forward, out hit)){
				GameObject[] pieces = MeshCut.Cut (hit.collider.gameObject, transform.position, transform.right, capMaterial);
			
				if(!pieces[1].GetComponent<Rigidbody>())
					pieces[1].AddComponent<Rigidbody>();
				
				Destroy(pieces[1], 1);
			}
		}

	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;

		Gizmos.DrawLine (transform.position, transform.position + transform.forward * 50.0f);
		Gizmos.DrawLine (transform.position + transform.up * 10f, transform.position + transform.up * 10f + transform.forward * 50.0f);
		Gizmos.DrawLine (transform.position + -transform.up * 10f, transform.position + -transform.up * 10f + transform.forward * 50.0f);
	
		Gizmos.DrawLine (transform.position, transform.position + transform.up * 10f);
		Gizmos.DrawLine (transform.position, transform.position + -transform.up * 10f);
	}
}
