using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour
{

    public Canvas quitMenu;
    public Canvas loginMenu;
    public Button loginText;
    public Button continueText;
    public Button newgameText;
    public Button avatarText;
    public Button quitText;
    public Button sandboxText;
    public InputField input;

    private GameObject gameManager;
    private GameManager manager;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 0;
        gameManager = GameObject.FindGameObjectWithTag( "GameManager" );
        if ( gameManager )
        {
            manager = gameManager.GetComponent<GameManager>();
        }
        //Set all the buttons & texts right
        quitMenu = quitMenu.GetComponent<Canvas>();
        continueText = continueText.GetComponent<Button>();
        newgameText = newgameText.GetComponent<Button>();
        avatarText = avatarText.GetComponent<Button>();
        quitText = quitText.GetComponent<Button>();
        quitMenu.enabled = false;
        loginMenu.enabled = false;
        sandboxText = sandboxText.GetComponent<Button>();
        PauseMenu.PauzeKey = KeyCode.None;
        updateLoginText();
    }
    //Load a new scene when you want to change the avatar
    public void AvatarPress()
    {
        SceneManagerScript.goToScene( "Character Selection", false );
    }

    // Opens a subscreen, disables all other buttons 
    public void QuitPress()
    {
        quitMenu.enabled = true;
        loginText.enabled = false;
        continueText.enabled = false;
        newgameText.enabled = false;
        avatarText.enabled = false;
        quitText.enabled = false;
        sandboxText.enabled = false;
    }

    //Set everything back in orgininal state 
    public void NoPress()
    {
        quitMenu.enabled = false;
        loginText.enabled = true;
        continueText.enabled = true;
        newgameText.enabled = true;
        avatarText.enabled = true;
        quitText.enabled = true;
        sandboxText.enabled = true;
    }

    //If you've pressed YES when in the quit subscreen
    public void YesPress()
    {
        Application.Quit();
    }

    // Get/make session, check checkpoint, set pauzekey, start scene
    public void ContinuePress()
    {
        if ( !string.IsNullOrEmpty( PlayerPrefs.GetString( "Username" ) ) )
        {
            if ( manager )
            {
                manager.GetSession( PlayerPrefs.GetString( "Username" ) );
            }

        }
        else
        {
            if ( manager )
            {
                manager.MakeSession();
            }
        }

        // If checkpoint is empty, set it to Level1.
        string CheckPoint = PlayerPrefs.GetString( "Checkpoint" );
        if ( string.IsNullOrEmpty( CheckPoint ) )
        {
            GameObject manager = GameObject.FindGameObjectWithTag( "GameManager" );
            if ( manager != null )
            {
                manager.GetComponent<GameManager>().UpdateCheckpoint( "Level1" );
            }
        }
        Debug.Log( "Current Checkpoint: " + PlayerPrefs.GetString( "Checkpoint" ) );
        PauseMenu.PauzeKey = KeyCode.Escape;
        SceneManagerScript.goToScene( PlayerPrefs.GetString( "Checkpoint" ), false );

    }

    // Log startTime, set pauzekey, make a new session
    public void NewGamePress()
    {
        PlayerPrefs.SetFloat( "StartTime", Time.time );
        PlayerPrefs.SetFloat( "PlayTime", 0 );
        PauseMenu.PauzeKey = KeyCode.Escape;
        if ( manager )
        {
            manager.MakeSession();
        }
        SceneManagerScript.goToScene( "Level1", false );
    }

    // Change logintext
    private void updateLoginText()
    {
        if ( !string.IsNullOrEmpty( PlayerPrefs.GetString( "Username" ) ) )
        {
            loginText.GetComponent<Text>().text = "Hi " + PlayerPrefs.GetString( "Username" );
        }
    }

    // Disable unused menus, enable login menu
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

    // Undo login menu, set data
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

    // Undo login menu, reset textbox
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

    // Go to the sandbox scene
    public void sandboxPress()
    {
        PlayerPrefs.SetFloat( "StartTime", Time.time );
        PauseMenu.PauzeKey = KeyCode.Escape;
        if ( manager )
        {
            manager.MakeSession();
        }
        SceneManagerScript.goToScene( "SandboxHub", false );
    }

}




