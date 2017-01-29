using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{

    public List<Object> DontDestroy;        // List of object to keep between scenes
    public bool makeCheckpoint;
    public KeyCode resetKey = KeyCode.R;    // Reset key
    public GUIText Scoretext;               // Used for displaying the score on the screen

    //Variables that track the time played
    //public float time;

    private static SceneManagerScript SceneManagementInstance;  // Static SceneManager to check for duplication
    private static IEnumerator coroutine;
    private string Checkpoint;
    private PortalManager portal;

    private int Deathcount = 0; // Counts how many times the main character died

    // If another GameController exists, destroy the existing objects first to avoid duplication
    private void Awake()
    {
        if ( GameObject.FindGameObjectsWithTag("GameController").Length > 1 )
        {
            foreach ( Object obj in SceneManagementInstance.DontDestroy )
            {
                Destroy( obj );
            }
            SceneManagementInstance = null;
        }
    }

    
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
            Deathcount = PlayerPrefs.GetInt( "Deaths" );
            Scoretext.text = "Deaths: " + Deathcount;
        }

        Checkpoint = SceneManager.GetActiveScene().name;
		if ( makeCheckpoint ) {
            PlayerPrefs.SetString( "Checkpoint", SceneManager.GetActiveScene().name );
            GameObject manager = GameObject.FindGameObjectWithTag( "GameManager" );
            if ( manager != null )
            {
                manager.GetComponent<GameManager>().UpdateCheckpoint( SceneManager.GetActiveScene().name );
            }
		}
    }

    // Check the reset key
    void Update()
    {
        if ( Input.GetKeyDown( resetKey ) )
        {
            reset();
        }
        //time = Time.time;
    }

    public void reset()
    {
        SceneManager.LoadScene( Checkpoint );
        this.Awake();
    }

    //Create a resetOnDeath() that increments the death counter if called
    public void resetOnDeath()
    {
        reset();
        PlayerPrefs.SetInt( "Deaths", PlayerPrefs.GetInt( "Deaths" ) + 1 );
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
        SceneManager.LoadSceneAsync( scene, mode );
        yield return null;
        Time.timeScale = 1;
    }

    private void teleport( Scene scene, LoadSceneMode mode )
    {
        portal.teleport();
        SceneManager.sceneLoaded -= teleport;
    }
}
