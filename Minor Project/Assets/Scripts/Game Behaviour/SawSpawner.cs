using UnityEngine;
using System.Collections;

public class SawSpawner : EnableScript {

    Animator anim;

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "PlayerBullet" ) )
        {
            toggle( true );
            anim = this.GetComponent<Animator>();
            // stop animating the target when hit
            anim.SetTime(0);
            Destroy(anim);

        }
    }
}
