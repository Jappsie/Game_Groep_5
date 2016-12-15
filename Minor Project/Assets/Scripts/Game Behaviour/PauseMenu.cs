using UnityEngine;
using System.Collections;


public class PauseMenu : MonoBehaviour
{

    private GameObject[] pauseObjects;
    private SceneManagerScript SceneManager;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag( "ShowOnPause" );
        SceneManager = GameObject.Find( "SceneController" ).GetComponent<SceneManagerScript>();
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        //uses the p button to pause and unpause the game
        if ( Input.GetKeyDown( KeyCode.P ) )
        {
            if ( Time.timeScale == 1 )
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if ( Time.timeScale == 0 )
            {
                Debug.Log( "high" );
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    //When continue button is pressed
    public void Continue()
    {
        Time.timeScale = 1;
        hidePaused();
    }


    //Reloads the Level
    public void Reload()
    {
        SceneManager.resetOnDeath();
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if ( Time.timeScale == 1 )
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if ( Time.timeScale == 0 )
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach ( GameObject g in pauseObjects )
        {
            g.SetActive( true );
        }
        PlayerMovement.AbleShoot = false;
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach ( GameObject g in pauseObjects )
        {
            g.SetActive( false );
        }
        PlayerMovement.AbleShoot = true;
    }

    //Reloads main menu
    public void LoadMainMenu( string level )
    {
        SceneManagerScript.goToScene( "main menu", false );
    }

}
