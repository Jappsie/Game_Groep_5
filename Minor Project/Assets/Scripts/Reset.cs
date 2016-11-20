/*Script to reset the game to the first scene by pressing "R"
 * Author: Jasper */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

	//Save the relevant key as public such that it can easily be changed
	private KeyCode resetKey;

	// Use this for initialization
	void Start () {
		resetKey = KeyCode.R;
	}
	
	// Update is called once per frame
	void Update () {
		//If the key is pressed, load the first scene
		if (Input.GetKeyDown (resetKey)) {
			//SceneManager.LoadScene ("scene1");  <-- this uses the name of the scene
			SceneManager.LoadScene(0);			//<-- this uses the index of the scene
		}
	}
}
