using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public List<Object> DontDestroy;        // List of object to keep between scenes
    public KeyCode resetKey = KeyCode.R;    // Reset key
    public GUIText Scoretext;               // Used for displaying the score on the screen


    //Variables that track the time played & a list of all the death times
    private float time;
    private static List<float> deathList = new List<float>();
    private static SceneManagerScript SceneManagementInstance;  // Static SceneManager to check for duplication
    private PortalManager portal;


    private void Awake()
    {
<<<<<<< HEAD
        Debug.Log( "Awake! " + gameObject.name );
        if ( GameObject.FindGameObjectsWithTag("GameController").Length > 1 )
=======
        if ( GameObject.FindGameObjectsWithTag( "GameController" ).Length > 1 )
>>>>>>> origin/platform1
        {
            foreach ( Object obj in SceneManagementInstance.DontDestroy )
            {
                Destroy( obj );
            }
            SceneManagementInstance = null;
        }
    }

    // If SceneManagementInstance exists, destroy the existing objects first to avoid duplication
    void Start()
    {
        // Make sure this object is included
        this.DontDestroy.Add( gameObject );
        foreach ( Object obj in this.DontDestroy )
        {
            DontDestroyOnLoad( obj );
        }
        SceneManagementInstance = this;

        // Changes the amount of deaths on the screen
        if ( Scoretext != null )
        {
            Scoretext.text = "Deaths: " + deathList.Count;
        }
		if (!SceneManager.GetActiveScene().name.Equals("main menu")) {
        	GameManager.checkpoint = SceneManager.GetActiveScene();
		}
    }

    // Check the reset key
    void Update()
    {
        if ( Input.GetKeyDown( resetKey ) )
        {
            reset();
        }
        time = Time.time;
    }

    public void reset()
    {
        SceneManager.LoadScene( GameManager.checkpoint.name );
        this.Awake();
    }

    //Create a resetOnDeath() that increments the death counter if called
    public void resetOnDeath()
    {
        this.reset();
        deathList.Add( this.time );
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
        if ( !SceneManager.GetActiveScene().Equals( SceneManager.GetSceneByName( scene ) ) )
        {
            IEnumerator coroutine = SceneManagementInstance.loadScene( scene, mode );
            SceneManagementInstance.StartCoroutine( coroutine );
        }
        else
        {
            SceneManagementInstance.teleport( SceneManager.GetActiveScene(), mode );
        }
    }

    public static void goToScene( string scene, bool Additive )
    {
        LoadSceneMode mode = LoadSceneMode.Single;
        if ( Additive )
        {
            mode = LoadSceneMode.Additive;
        }
        IEnumerator coroutine = SceneManagementInstance.loadScene( scene, mode );
        SceneManagementInstance.StartCoroutine( coroutine );
    }

    private IEnumerator loadScene( string scene, LoadSceneMode mode )
    {
        //AsyncOperation loading = 
        SceneManager.LoadSceneAsync( scene, mode );
        yield return null;
    }

    private void teleport( Scene scene, LoadSceneMode mode )
    {
        portal.teleport();
        SceneManager.sceneLoaded -= teleport;
    }
}
