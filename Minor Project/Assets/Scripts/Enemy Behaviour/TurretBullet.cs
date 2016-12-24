using UnityEngine;
using System.Collections;

public class TurretBullet : MonoBehaviour
{
    public float firespeed = 7.0f;
    public float damage = 1f;

    private Vector3 direction;

    // Destroy after 3 seconds
    private void Start()
    {
        Destroy( this.gameObject, 3.0f );
    }

    //Bullet speed
    void Update()
    {
        transform.position += transform.forward * (firespeed * Time.deltaTime);
    }

    private void OnTriggerEnter( Collider collision )
    {
        if ( collision.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log( collision.gameObject.name + " Got Damaged" );
            collision.gameObject.SendMessage( "TakeDamage", damage );
        }

        if ( !collision.gameObject.CompareTag( "Turret" ) )
        {
            Destroy( gameObject );
        }
    }
}
