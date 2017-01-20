﻿using UnityEngine;
using System.Collections;


public class PauseMenu : MonoBehaviour
{
    public static KeyCode PauzeKey = KeyCode.None;
    public KeyCode pauzeKey2;
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
        pauzeKey2 = PauzeKey;
        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown( PauzeKey ) )
        {
            if ( Time.timeScale == 1 )
            {
                showPaused();
            }
            else if ( Time.timeScale == 0 )
            {
                Debug.Log( "high" );
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

    public void SaveGame()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Username")))
        {
            Debug.Log( "Not logged in" );
        }
        else
        {
            GameManager.SaveUser(PlayerPrefs.GetString("Username"));
            Debug.Log( "Saved" );
        }
        Continue();
    }


    //Reloads the Level
    public void Reload()
    {
        hidePaused();
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
		Time.timeScale = 0;
        foreach ( GameObject g in pauseObjects )
        {
            g.SetActive( true );
        }
        PlayerMovement.AbleShoot = false;
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
		Time.timeScale = 1;
        foreach ( GameObject g in pauseObjects )
        {
            g.SetActive( false );
        }
        PlayerMovement.AbleShoot = true;
    }

    //Reloads main menu
    public void LoadMainMenu()
    {
        //UnityEngine.EventSystems.EventSystem.current.enabled = false;
        hidePaused();
        SceneManagerScript.goToScene( "main menu", false );
    }

}