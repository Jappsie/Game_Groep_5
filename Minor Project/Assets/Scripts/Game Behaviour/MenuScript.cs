using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button continueText;
	public Button newgameText;
	public Button avatarText;
	public Button quitText;

	// Use this for initialization
	void Start () {

	//Set all the buttons & texts right
		quitMenu = quitMenu.GetComponent<Canvas> ();
		continueText = continueText.GetComponent<Button> ();
		newgameText = newgameText.GetComponent<Button> ();
		avatarText = avatarText.GetComponent<Button> (); 
		quitText = quitText.GetComponent<Button> ();
		quitMenu.enabled = false;
        PauseMenu.PauzeKey = KeyCode.None;
	
	}
	//Load a new scene when you want to change the avatar
	public void AvatarPress(){
        SceneManagerScript.goToScene( "Character Selection", false );
	}

	// Opens a subscreen, disables all other buttons 
	public void QuitPress(){
		quitMenu.enabled = true;
		continueText.enabled = false;
		newgameText.enabled = false;
		avatarText.enabled = false; 
		quitText.enabled = false; 
	}
	//Set everything back in orgininal state 
	public void NoPress(){
		quitMenu.enabled = false;
		continueText.enabled = true;
		newgameText.enabled = true;
		avatarText.enabled = true; 
		quitText.enabled = true; 
	}
	//If you've pressed YES when in the quit subscreen
	public void YesPress(){
		Application.Quit (); 
	}

    public void ContinuePress()
    {
        string CheckPoint = GameManager.checkpoint.name;
		Debug.Log ("Current Checkpoint: " + CheckPoint);
        if (!string.IsNullOrEmpty(CheckPoint))
        {
            PauseMenu.PauzeKey = KeyCode.Escape;
            SceneManagerScript.goToScene( CheckPoint, false );
        }
        
    }

	//If you press New Game;
	public void NewGamePress(){
        PauseMenu.PauzeKey = KeyCode.Escape;
        SceneManagerScript.goToScene( "Level1", false );
	}
	
	}
	



