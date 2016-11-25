using UnityEngine;
using System.Collections;

public class PortalManager : MonoBehaviour {

    public Object Scene;
    public Vector3 Position;
    public string PortalName;
    public bool Additive;

    private IEnumerator coroutine;

    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log( gameObject.name + " : " + Scene.name );
            SceneManagerScript.goToScene( Scene.name, Additive , this);
        }
    }

    public void teleport()
    {
        // Find portal
        GameObject portalObject = GameObject.Find( PortalName );
        PortalManager portalScript = portalObject.GetComponent<PortalManager>();

        // Find player
        GameObject other = GameObject.FindGameObjectWithTag( "Player" );
        other.transform.position = portalObject.transform.position + portalScript.Position + new Vector3( 0, other.transform.position.y, 0 );
    }

}
