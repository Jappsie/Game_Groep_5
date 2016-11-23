using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewPortalManager : MonoBehaviour {

    public Object Scene;
    public Vector3 Position;
    public string PortalName;
    public bool Additive;

    private IEnumerator coroutine;

    void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            SceneManagerScript.goToScene( Scene.name, Additive );
            coroutine = teleport( other );
            StartCoroutine(  coroutine );
        }
    }

    private IEnumerator teleport( Collider other)
    {
        // Detect loading the scene
        yield return null;
        SceneManager.sceneLoaded += teleport;
        
    }

    private void teleport( Scene scene, LoadSceneMode sceneMode )
    {
        // Find portal
        GameObject portalObject = GameObject.Find( PortalName );
        NewPortalManager portalScript = portalObject.GetComponent<NewPortalManager>();

        // Find player
        GameObject other = GameObject.FindGameObjectWithTag( "Player" );
        other.transform.position = portalObject.transform.position + portalScript.Position + new Vector3( 0, other.transform.position.y, 0 );
    }

}
