using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

<<<<<<< HEAD
public class GameManager : MonoBehaviour
{

    public float trackInterval = 2f;
    public static Scene checkpoint;
	public string checkpoint2;
    public List<GameObject> objects;
=======
public class GameManager: MonoBehaviour {

    public float trackInterval = 2f;
>>>>>>> origin/platform1

    private string UserName;
    [SerializeField]
    private int userId = -1;
    [SerializeField]
    private Scene currentScene;
    [SerializeField]
    private GameObject player;

    private string glyph = "abcdefghijklmnopqrstuvwxyz0123456789";
    private int glyphLength = 32;

<<<<<<< HEAD
    private void Awake()
    {

        if ( GameObject.FindGameObjectsWithTag( "GameManager" ).Length > 1 )
        {
            foreach ( GameObject obj in objects )
            {
                Destroy( obj );
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad( gameObject );
        foreach ( GameObject obj in objects )
        {
            DontDestroyOnLoad( obj );
        }
=======
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad( gameObject );
>>>>>>> origin/platform1
        SceneManager.sceneLoaded += trackScene;
        InvokeRepeating( "trackPlayer", 0, trackInterval );
        if ( UserName == null )
        {
            SetUserName();
        }
<<<<<<< HEAD
    }

	void Update() {
		checkpoint2 = checkpoint.name;
	}

    private void trackScene( Scene scene, LoadSceneMode mode )
    {
        Debug.Log( "Scene changed" );
        if ( mode.Equals( LoadSceneMode.Single ) )
=======
	}

    private void trackScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log( "Scene changed" );
        if ( mode.Equals( LoadSceneMode.Single) )
>>>>>>> origin/platform1
        {
            currentScene = scene;
            player = GameObject.FindGameObjectWithTag( "Player" );
        }
    }

    private void trackPlayer()
    {
<<<<<<< HEAD
        if ( player != null && userId != -1 )
=======
        if (player != null && userId != -1)
>>>>>>> origin/platform1
        {
            WWWForm form = new WWWForm();
            form.AddField( "userId", userId );
            form.AddField( "scene", currentScene.name );
            form.AddField( "x", player.transform.position.x.ToString() );
            form.AddField( "y", player.transform.position.y.ToString() );
            form.AddField( "z", player.transform.position.z.ToString() );
            StartCoroutine( sendData( "position", form ) );
        }
        else
        {
<<<<<<< HEAD
            trackScene( currentScene, LoadSceneMode.Single );
        }
    }

    public void deadPlayer( Vector3 position )
=======
            trackScene(currentScene, LoadSceneMode.Single);
        }
    }

    public void deadPlayer(Vector3 position)
>>>>>>> origin/platform1
    {
        if ( userId != -1 )
        {
            WWWForm form = new WWWForm();
            form.AddField( "userId", userId );
            form.AddField( "scene", currentScene.name );
            form.AddField( "x", position.x.ToString() );
            form.AddField( "y", position.y.ToString() );
            form.AddField( "z", position.z.ToString() );
            StartCoroutine( sendData( "death", form ) );
        }
    }

    private void SetUserName()
    {
        UserName = "";
<<<<<<< HEAD
        for ( int i = 0; i < glyphLength; i++ )
        {
            UserName += glyph[ Random.Range( 0, glyph.Length ) ];
=======
        for (int i = 0; i < glyphLength; i++)
        {
            UserName += glyph[Random.Range(0,glyph.Length)];
>>>>>>> origin/platform1
        }
        StartCoroutine( newUser() );
    }

    IEnumerator newUser()
    {
        WWWForm form = new WWWForm();
        form.AddField( "name", UserName );

        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/newUser", form );

        yield return www;

<<<<<<< HEAD
        if ( !string.IsNullOrEmpty( www.error ) )
        {
            Debug.Log( www.error );
            CancelInvoke( "trackPlayer" );
        }
        else
        {
            JsonUtility.FromJsonOverwrite( www.text, this );
        }
    }

    IEnumerator sendData( string url, WWWForm form )
    {
        Debug.Log( "Sending data" );
        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/" + url, form );
        yield return www;

        if ( !string.IsNullOrEmpty( www.error ) )
=======
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log( www.error );
            CancelInvoke("trackPlayer");
        }
        else
        {
            JsonUtility.FromJsonOverwrite(www.text, this);
        }
    }

    IEnumerator sendData(string url, WWWForm form)
    {
        Debug.Log("Sending data");
        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/" + url, form );
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
>>>>>>> origin/platform1
        {
            Debug.Log( www.error );
        }
    }
<<<<<<< HEAD

=======
	
>>>>>>> origin/platform1
}
