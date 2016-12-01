using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {

    private GameObject target;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag( "Player" );
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log( "Draw line" );
        Debug.DrawLine( transform.position, target.transform.position, Color.black);
	}
}
