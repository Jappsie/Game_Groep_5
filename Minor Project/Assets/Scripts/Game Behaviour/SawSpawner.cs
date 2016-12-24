using UnityEngine;
using System.Collections;

public class SawSpawner : EnableScript {

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "PlayerBullet" ) )
        {
            toggle( true );
        }
    }
}
