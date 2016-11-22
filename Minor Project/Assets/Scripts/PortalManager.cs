using UnityEngine;
using System.Collections;

public class PortalManager : MonoBehaviour
{

    public Object Scene;        // Scene to teleport to
    public Vector3 Position;    // Absolute position to teleport to
    public bool Additive;       // Choose single or additive loading of scenes

    // On Collide with a 'player' go to the scene and relocate
    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            SceneManagerScript.goToScene( Scene.name, Additive );
            Debug.Log( other.gameObject.transform.position );
            other.transform.position = Position + new Vector3( 0, other.transform.position.y, 0 );
        }
    }

}
