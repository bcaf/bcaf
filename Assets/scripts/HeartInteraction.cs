﻿using UnityEngine;
using System.Collections;

public class HeartInteraction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseUp(){
		print ("hej");
		if(Input.GetMouseButtonDown(0)){
			print ("hej");
		}
	}
}