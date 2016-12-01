using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour {

    // list of enemy objects that can be spawned
    public GameObject[] enemies;
    // amount of enemies that need to be spawned
    public int amount;
    // keep spawning until destroyed
    public bool spawnInfinite;
    // initial time before spawning begins in [s]
    public float spawnTime;
    // time interval inbetween spawns in [s]
    public float interval;
    // spawn radius
	public float radius;
    // spawn counter;
    private int count;
    // computed spawn location
    private Vector3 spawnPoint;

    // start spawning when object is activated
	void Start() {
        // initialize counter
        count = 0;
		InvokeRepeating ("spawnEnemy", spawnTime, interval);
    }
	
	// Update is called once per frame
	void Update () {                
        // spawn until limit has been reached
        if (count >= amount || spawnInfinite )
        {
			CancelInvoke ();
            Destroy( gameObject );
        }
	}

    // spawn function
    void spawnEnemy ()
    {
        count++;
        // randomly assign a coordinate within world space to the enemy
		spawnPoint.x = Random.Range(-radius, radius);
		spawnPoint.z = Random.Range(-radius, radius);

        // make an instance of a random enemy at the spawnPoint and align it with world coordinates
		GameObject enemy = enemies[Random.Range(0, enemies.Length)];

		//Check where the center of the collider of the enemy is
		Vector3 enemyCenter = enemy.GetComponent<BoxCollider> ().size * 0.5f + spawnPoint;
		//Fill an array with all the collision with a sphere around the enemy's BoxCollider with a ray of 0.5 * the width of the BoxCollider
		Collider[] colliderChecker = Physics.OverlapSphere (enemyCenter, enemy.GetComponent<BoxCollider>().size.x * 0.5f);
		//When the enemy only collides with the plane, place it
		if (colliderChecker.Length == 1) {
			Instantiate (enemy, gameObject.transform.position + spawnPoint + enemy.transform.position, Quaternion.identity);
		}
    }
}
