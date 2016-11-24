﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{

    public List<Object> DontDestroy;        // List of object to keep between scenes
    public Object StartScene;               // Scene to reset to
    public KeyCode resetKey = KeyCode.R;    // Reset key

    private static SceneManagerScript SceneManagementInstance;  // Static SceneManager to check for duplication
    private static IEnumerator coroutine;
    private PortalManager portal;

    // If SceneManagementInstance exists, destroy the existing objects first to avoid duplication
    void Awake()
    {
        // Make sure this object is included
        DontDestroy.Add( gameObject );

        if ( SceneManagementInstance != null )
        {
            foreach ( Object obj in DontDestroy )
            {
                Destroy( obj );
            }
        }
        else
        {
            foreach ( Object obj in DontDestroy )
            {
                DontDestroyOnLoad( obj );
            }
            SceneManagementInstance = this;
        }
    }

    // Check the reset key
    void Update()
    {
        if ( Input.GetKeyDown( resetKey ) )
        {
            reset();
        }
    }

    public void reset()
    {
        this.Awake();
        SceneManager.LoadScene( StartScene.name );
    }

    // Main method to switch scenes
    public static void goToScene( string scene, bool Additive, PortalManager portal )
    {
        SceneManagementInstance.portal = portal;
        LoadSceneMode mode = LoadSceneMode.Single;
        if ( Additive )
        {
            mode = LoadSceneMode.Additive;
        }
        SceneManager.sceneLoaded += SceneManagementInstance.teleport;
        // Only reload scene if actually changing scenes
        if ( !SceneManager.GetActiveScene().Equals( SceneManager.GetSceneByName( scene ) ))
        {
            //SceneManager.LoadScene( scene, mode );
            IEnumerator coroutine = SceneManagementInstance.loadScene( scene, mode );
            SceneManagementInstance.StartCoroutine( coroutine );
        }
        else
        {
            SceneManagementInstance.teleport(SceneManager.GetActiveScene(), mode);
        }
    }

    private IEnumerator loadScene( string scene, LoadSceneMode mode)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync( scene, mode );
        yield return null;
    }

    private void teleport (Scene scene, LoadSceneMode mode)
    {
        portal.teleport();
        SceneManager.sceneLoaded -= teleport;
    }

}
