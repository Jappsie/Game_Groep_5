using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinnerCanvas : snakeScript {


	public MovieTexture movie; 
	private GameObject snake; 
	public Canvas Winner; 
	public Button Credits;

	// Use this for initialization
	void Start () {
		GetComponent<RawImage> ().texture = movie as MovieTexture; 
		movie.loop = true;
		movie.Play (); 

	
		snake = GameObject.FindGameObjectWithTag ("SnakeController");
		Winner = Winner.GetComponent<Canvas> ();
		Credits = Credits.GetComponent<Button> (); 

		Winner.enabled = false; 
		Credits.enabled = false; 

	}
		
	void GoToCredits(){ 

		SceneManagerScript.goToScene( "Credits", false );
	}
	
	// Update is called once per frame
	void Update () {
		if (CurHealth <= 0) {
			Winner.enabled = true; 
			Credits.enabled = true; 
			Time.timeScale = 0; 

		}
	
	}
}
