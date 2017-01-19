using UnityEngine;
using System.Collections;

public class StickyTrigger : MonoBehaviour {

    private void OnTriggerEnter( Collider other )
    {
        if (other.gameObject.CompareTag("Sticky"))
        {
            gameObject.transform.parent = other.gameObject.transform;
            gameObject.tag = "Player";
            foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
            {
                child.tag = "Player";
            }
        }

    }

    private void OnTriggerExit( Collider other )
    {
        if ( other.gameObject.CompareTag( "Sticky" ) )
        {
            gameObject.transform.parent = null;
            gameObject.tag = "Untagged";
            foreach ( Transform child in gameObject.GetComponentInChildren<Transform>() )
            {
                child.tag = "Untagged";
            }
        }

    }
}
