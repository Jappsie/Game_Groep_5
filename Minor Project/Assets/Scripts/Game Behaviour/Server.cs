using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Server : MonoBehaviour {

    [HideInInspector]
    public int message;

	// Use this for initialization
	void Start () {
        StartCoroutine( newUser() );
	}

    IEnumerator newUser()
    {
        WWWForm form = new WWWForm();
        form.AddField( "name", "Martijn" );

        WWW www = new WWW( "http://insyprojects.ewi.tudelft.nl:8085/newUser", form );

        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log( www.error );
        }
        else
        {
            JsonUtility.FromJsonOverwrite(www.text, this);
            Debug.Log( "Form upload complete! " + message);
        }
    }
	
}
