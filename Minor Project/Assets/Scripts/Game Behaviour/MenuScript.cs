using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
    public Canvas loginMenu;
    public Button loginText;
	public Button continueText;
	public Button newgameText;
	public Button avatarText;
	public Button quitText;
	public Button sandboxText;
    public InputField input;

	// Use this for initialization
	void Start () {
        Time.timeScale = 0;
	//Set all the buttons & texts right
		quitMenu = quitMenu.GetComponent<Canvas> ();
		continueText = continueText.GetComponent<Button> ();
		newgameText = newgameText.GetComponent<Button> ();
		avatarText = avatarText.GetComponent<Button> (); 
		quitText = quitText.GetComponent<Button> ();
		quitMenu.enabled = false;
        loginMenu.enabled = false;
		sandboxText = sandboxText.GetComponent<Button> ();
        PauseMenu.PauzeKey = KeyCode.None;
        updateLoginText();
	}
	//Load a new scene when you want to change the avatar
	public void AvatarPress(){
        SceneManagerScript.goToScene( "Character Selection", false );
	}

	// Opens a subscreen, disables all other buttons 
	public void QuitPress(){
		quitMenu.enabled = true;
        loginText.enabled = false;
		continueText.enabled = false;
		newgameText.enabled = false;
		avatarText.enabled = false; 
		quitText.enabled = false; 
		sandboxText.enabled = false;
	}
	//Set everything back in orgininal state 
	public void NoPress(){
		quitMenu.enabled = false;
        loginText.enabled = true;
		continueText.enabled = true;
		newgameText.enabled = true;
		avatarText.enabled = true; 
		quitText.enabled = true; 
		sandboxText.enabled = true;
	}
	//If you've pressed YES when in the quit subscreen
	public void YesPress(){
		Application.Quit (); 
	}

    public void ContinuePress()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Username")))
        {
            GameManager.GetSession( PlayerPrefs.GetString( "Username" ) );
        }
        else
        {
            GameManager.MakeSession();
        }

        string CheckPoint = PlayerPrefs.GetString( "Checkpoint" );
        if (string.IsNullOrEmpty(CheckPoint))
        {
            GameManager.UpdateCheckpoint( "Level1" );
        }
        Debug.Log( "Current Checkpoint: " + PlayerPrefs.GetString("Checkpoint") );
        PauseMenu.PauzeKey = KeyCode.Escape;
        SceneManagerScript.goToScene( PlayerPrefs.GetString( "Checkpoint" ), false );

    }

	//If you press New Game;
	public void NewGamePress() {
        PlayerPrefs.SetFloat( "StartTime", Time.time );
        PauseMenu.PauzeKey = KeyCode.Escape;
        GameManager.MakeSession();
        SceneManagerScript.goToScene( "Level1", false );
	}

    private void updateLoginText()
    {
        if ( !string.IsNullOrEmpty( PlayerPrefs.GetString( "Username" ) ) )
        {
            loginText.GetComponent<Text>().text = "Hi " + PlayerPrefs.GetString( "Username" );
        }
    }

    public void LoginPress()
    {
        loginMenu.enabled = true;
        loginText.enabled = false;
        continueText.enabled = false;
        newgameText.enabled = false;
        avatarText.enabled = false;
        quitText.enabled = false;
		sandboxText.enabled = false;
    }

    public void OKPress()
    {
        loginMenu.enabled = false;
        loginText.enabled = true;
        continueText.enabled = true;
        newgameText.enabled = true;
        avatarText.enabled = true;
        quitText.enabled = true;
		sandboxText.enabled = true;
        PlayerPrefs.SetString( "Username", input.text );
        PlayerPrefs.Save();
        input.text = "";
        updateLoginText();
    }

    public void CancelPress()
    {
        loginMenu.enabled = false;
        loginText.enabled = true;
        continueText.enabled = true;
        newgameText.enabled = true;
        avatarText.enabled = true;
        quitText.enabled = true;
		sandboxText.enabled = true;
        input.text = "";
    }

	public void sandboxPress() {
		PlayerPrefs.SetFloat( "StartTime", Time.time );
		PauseMenu.PauzeKey = KeyCode.Escape;
		GameManager.MakeSession();
		SceneManagerScript.goToScene( "SandboxHub", false );
	}
	
}
	



