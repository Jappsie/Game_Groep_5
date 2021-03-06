﻿using UnityEngine;
using System.Collections;

public class PortalManager : MonoBehaviour {

    public string Scene;
    public Vector3 Position;
    public bool toPortal;
    public string PortalName;
    public bool Additive;

    private IEnumerator coroutine;

    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log( gameObject.name + " : " + Scene );
            if ( toPortal )
            {
                SceneManagerScript.goToScene( Scene, Additive, this );
            }
            else
            {
                SceneManagerScript.goToScene( Scene, Additive );
            }
        }
    }

    public void teleport()
    {
        // Find portal
        GameObject portalObject = GameObject.Find( PortalName );
        PortalManager portalScript = portalObject.GetComponent<PortalManager>();

        // Find player
        GameObject other = GameObject.FindGameObjectWithTag( "Player" );
        other.transform.position = portalObject.transform.position + portalScript.Position;
    }

}
