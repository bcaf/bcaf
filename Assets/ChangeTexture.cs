using UnityEngine;
using System.Collections;

public class ChangeTexture : MonoBehaviour {

	public Texture[] textures;
	public int currentTexture;
	bool complain;
	
	void Start () {

		complain = false;
		StartCoroutine(BlinkTimer());
	
	}

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
		var randomInt = Random.Range (0, 3);
		if (randomInt == 0) {
			complain = true;
		}
		yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
		GetComponent<Renderer>().material.mainTexture = textures[3];
		yield return new WaitForSeconds(0.1f);
		GetComponent<Renderer>().material.mainTexture = textures[0];
		if (complain) {
			yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
			GetComponent<Renderer>().material.mainTexture = textures[1];
			yield return new WaitForSeconds(0.5f);
			GetComponent<Renderer>().material.mainTexture = textures[0];
			complain = false;
		}
		StartCoroutine(BlinkTimer());
	}
}
