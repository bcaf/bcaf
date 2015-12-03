using UnityEngine;
using System.Collections;

public class ChangeTexture : MonoBehaviour {

	public Texture[] textures;
	public int currentTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("p")) {
			currentTexture++;
			currentTexture %= textures.Length;
			GetComponent<Renderer>().material.mainTexture = textures[currentTexture];
		}
	
	}
}
