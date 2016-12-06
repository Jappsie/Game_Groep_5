using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{

    public List<Object> DontDestroy;        // List of object to keep between scenes
    public Object StartScene;               // Scene to reset to
    public KeyCode resetKey = KeyCode.R;    // Reset key
	public GUIText Scoretext;               // Used for displaying the score on the screen

	//Variables that track the time played & a list of all the death times
	public float time;
	public static List<float> deathList = new List<float>();

    private static SceneManagerScript SceneManagementInstance;  // Static SceneManager to check for duplication
    private static IEnumerator coroutine;
    private PortalManager portal;

	private int Deathcount = 0; // Counts how many times the main character died


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
			//Initialize the time and amount of deaths on awake
            SceneManagementInstance = this;
        }
		// Changes the amount of deaths on the screen
		Deathcount = deathList.Count;
		Scoretext.text = "Deaths: " + Deathcount; 
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
        SceneManager.LoadScene( StartScene.name );
		this.Awake();
    }

	//Create a resetOnDeath() that increments the death counter if called
	public void resetOnDeath()
	{
		this.reset ();
		deathList.Add (this.time);			//Add another death to the list
		foreach (float dood in deathList) {
			Debug.Log (dood);
		}
		Debug.Log ("Amount of deaths: " + deathList.Count);		//Log the death times and amount of deaths
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
