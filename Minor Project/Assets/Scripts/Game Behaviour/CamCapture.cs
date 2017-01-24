using UnityEngine;
using System.Collections;

public class CamCapture : MonoBehaviour {

    public string filename;

	// Use this for initialization
	void Start () {
        Application.CaptureScreenshot("Avatars/" + filename);
        Debug.Log( "Click" );
	}

}
