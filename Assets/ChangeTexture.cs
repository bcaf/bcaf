using UnityEngine;
using System.Collections;

public class ChangeTexture : MonoBehaviour {

	public Texture[] textures;
	public int currentTexture;

	// Use this for initialization
	void Start () {

		StartCoroutine(BlinkTimer());
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("p")) {
			currentTexture++;
			currentTexture %= textures.Length;
			GetComponent<Renderer>().material.mainTexture = textures[currentTexture];
		}

		if (Input.GetMouseButton (0)) {
			GetComponent<Renderer>().material.mainTexture = textures[2];
		}

		if (Input.GetMouseButtonUp (0)) {
			GetComponent<Renderer>().material.mainTexture = textures[0];
		}

	
	}

	public IEnumerator BlinkTimer() {
		yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
		GetComponent<Renderer>().material.mainTexture = textures[3];
		yield return new WaitForSeconds(0.1f);
		GetComponent<Renderer>().material.mainTexture = textures[0];
		StartCoroutine(BlinkTimer());
	}
}
