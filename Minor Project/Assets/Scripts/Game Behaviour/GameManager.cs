using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool analytics;
    public float trackInterval = 2f;
    public List<GameObject> objects;

    private string UserName;
    [SerializeField]
    private int userId = -1;
    [SerializeField]
    private Scene currentScene;
    [SerializeField]
    private GameObject player;

    private string glyph = "abcdefghijklmnopqrstuvwxyz0123456789";
    private int glyphLength = 32;

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
        if ( analytics )
        {
            SceneManager.sceneLoaded += trackScene;
            InvokeRepeating( "trackPlayer", 0, trackInterval );
            if ( UserName == null )
            {
                SetUserName();
            }
        }
    }

    private void trackScene( Scene scene, LoadSceneMode mode )
    {
        Debug.Log( "Scene changed" );
        if ( mode.Equals( LoadSceneMode.Single ) )
        {
            currentScene = scene;
            player = GameObject.FindGameObjectWithTag( "Player" );
        }
    }

    private void trackPlayer()
    {
        if ( player != null && userId != -1 )
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
            trackScene( currentScene, LoadSceneMode.Single );
        }
    }

    public void deadPlayer( Vector3 position )
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
        for ( int i = 0; i < glyphLength; i++ )
        {
            UserName += glyph[ Random.Range( 0, glyph.Length ) ];
        }
        StartCoroutine( newUser() );
    }

    IEnumerator newUser()
    {
        WWWForm form = new WWWForm();
        form.AddField( "name", UserName );

        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/newUser", form );

        yield return www;

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
        {
            Debug.Log( www.error );
        }
    }

}
