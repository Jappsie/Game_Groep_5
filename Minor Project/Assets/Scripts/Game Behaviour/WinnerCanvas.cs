	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;

	public class WinnerCanvas : snakeScript {

	public Canvas Winner; 
	public Button Credits;


		public MovieTexture movie; 

		// Use this for initialization
		void Start () {
		
			GetComponent<RawImage> ().texture = movie as MovieTexture; 
			movie.loop = true;
			movie.Play (); 

		Winner = Winner.GetComponent<Canvas> (); 
		Credits = Credits.GetComponent<Button> (); 

		Winner.enabled = false; 
		Credits.enabled = false; 
		}

		public void GoToCredits(){ 

			SceneManagerScript.goToScene("Credits", false );
		}

		// Update is called once per frame
		void Update () {

		if (CurHealth <= 0) {
			Time.timeScale = 0; 
			Winner.enabled = true; 
			Credits.enabled = true; 
	
		}

		}

}
