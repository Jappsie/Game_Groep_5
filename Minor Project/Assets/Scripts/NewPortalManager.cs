using UnityEngine;
using System.Collections;

public class NewPortalManager : MonoBehaviour {

    public Object Scene;
    public Vector3 Position;
    public string PortalName;
    public bool Additive;

    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            SceneManagerScript.goToScene( Scene.name, Additive );
            GameObject portalObject = GameObject.Find( PortalName );
            NewPortalManager portalScript = portalObject.GetComponent<NewPortalManager>();
            other.transform.position = portalObject.transform.position + portalScript.Position + new Vector3( 0, other.transform.position.y, 0 );
        }
    }

}
