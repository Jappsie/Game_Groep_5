using UnityEngine;
using System.Collections;

public class SnakeBite : MonoBehaviour {

    public float damageFromSnake;
    public float damageToSnake;
    public float damageFromCrystal;

    // If normalcollision, this was the snake, 
    private void OnCollisionEnter( Collision collision )
    {
        if (collision.gameObject.CompareTag("Crystal"))
        {
            gameObject.GetComponent<HealthSystem>().TakeDamage( damageToSnake );
        }
    }

    private void OnControllerColliderHit( ControllerColliderHit hit )
    {
        if (hit.gameObject.CompareTag("Crystal"))
        {
            gameObject.GetComponent<HealthSystem>().TakeDamage( damageFromCrystal );
        }

        if (hit.gameObject.CompareTag("Snake"))
        {
            gameObject.GetComponent<HealthSystem>().TakeDamage( damageFromSnake );
        }
    }
}
