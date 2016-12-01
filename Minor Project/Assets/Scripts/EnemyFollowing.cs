using UnityEngine;
using System.Collections;
using System;

public class EnemyFollowing : HealthSystem
{
    public float followDistance = 10f;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 3.0f;
    public float Damage = 1;
	public int Enemylife = 4;

    private GameObject Player;
    private Vector3 startPos;

    // Use this for initialization
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Follow a 'player' if within followDistance
	protected override void Update()			//Now overrides the Update of HealthSystem to check y pos
	{
		base.Update ();							//Call to Update of Parent

		Player = GameObject.FindGameObjectsWithTag ("Player") [0];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;

		if (Vector3.Distance (playerPos, objectPos) < followDistance) {
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (playerPos - objectPos), rotationSpeed * Time.deltaTime);
		} else {
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (startPos - objectPos), rotationSpeed * Time.deltaTime);
		}
		gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
	}

	private void OnCollisionStay( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log( collision.gameObject.name + " Got Damaged");
            collision.gameObject.SendMessage( "TakeDamage", Damage );
        }
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Enemylife -= 1;
            Destroy( collision.gameObject );
            if (Enemylife <= 0)
            {
                Destroy( this.gameObject );
            }
        }
    }

	private void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag("PlayerBullet")) {
			Enemylife -= 1;
			Destroy (col.gameObject);
			if (Enemylife == 0) {
				Destroy (this.gameObject);
			}
		}
	}

    protected override void Death()
    {
        Destroy(gameObject);
    }

}