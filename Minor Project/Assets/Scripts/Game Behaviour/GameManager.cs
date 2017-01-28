using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public bool analytics;
    public float trackInterval = 2f;
    public List<GameObject> objects;

    [SerializeField]
    private int deathCount = 0;
    [SerializeField] // Is nodig voor de jsonUtility
    private string Checkpoint = "";
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
            Destroy( gameObject );
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

    public void MakeSession()
    {
        string username = "";
        for ( int i = 0; i < glyphLength; i++ )
        {
            username += glyph[ UnityEngine.Random.Range( 0, glyph.Length ) ];
        }
        StartCoroutine( newSession(username) );
    }

    public void GetSession(string username)
    {
        StartCoroutine( getSession(username) );
    }

    public void SaveUser(string username)
    {
        PlayerPrefs.SetString( "Username", username );
        WWWForm form = new WWWForm();
        form.AddField( "Username", username );
        form.AddField( "Checkpoint", PlayerPrefs.GetString( "Checkpoint" ) );
        form.AddField( "Id", userId );
        StartCoroutine( sendData( "saveUser", form ) );
    }

    public void UpdateCheckpoint(string Checkpoint)
    {
            if ( !string.IsNullOrEmpty( PlayerPrefs.GetString( "Username" ) ) )
            {
                WWWForm form = new WWWForm();
                form.AddField( "Username", PlayerPrefs.GetString( "Username" ) );
                form.AddField( "Checkpoint", Checkpoint );
                StartCoroutine( sendData( "checkpoint", form ) );
            }
        
    }

    public void UpdateAvatar( int Avatar )
    {
        if ( !string.IsNullOrEmpty( PlayerPrefs.GetString( "Username" ) ) )
        {
            WWWForm form = new WWWForm();
            form.AddField( "Username", PlayerPrefs.GetString( "Username" ) );
            form.AddField( "Avatar", Avatar );
            StartCoroutine( sendData( "avatar", form ) );
        }
    }

    public void getDeaths()
    {
        
        if (userId >= 0)
        {
            StartCoroutine( getDeaths( userId ) );
        }
    }

    IEnumerator newSession(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField( "name", username );

        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/newSession", form );

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

    IEnumerator getSession(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField( "name", username );

        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/getSession", form );

        yield return www;

        if (!string.IsNullOrEmpty( www.error))
        {
            Debug.Log( www.error );
            CancelInvoke( "trackPlayer" );
        }
        else
        {
            Debug.Log( www.text );
            JsonUtility.FromJsonOverwrite( www.text, this );
            Debug.Log( Checkpoint );
            PlayerPrefs.SetString( "Checkpoint", Checkpoint );
            getDeaths();
        }
    }

    IEnumerator getDeaths( int userId)
    {
        WWWForm form = new WWWForm();
        form.AddField( "UserId", userId );

        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/getDeaths", form );

        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log( www.error );
        }
        else
        {
            Debug.Log( www.text );
            JsonUtility.FromJsonOverwrite( www.text, this );
            PlayerPrefs.SetInt( "Deaths", deathCount );
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
        } else
        {
            Debug.Log( www.text );
        }
    }

}
