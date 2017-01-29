using UnityEngine;
using System.Collections;

public class CamCapture : MonoBehaviour {

    public string filename;

	// Use this for initialization
	void Start () {
        Application.CaptureScreenshot("Assets/Resources/Avatars/" + filename);
        Debug.Log( "Click" );
	}

}
