using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour {

    // enemy objects
    public GameObject[] enemies;
    // amount of enemies that need to be spawned
    public int amount;
    // initial time before spawning begins in [s]
    public float spawnTime;
    // time interval inbetween spawns in [s]
    public float interval;
    // radius
	public float radius;
	private Vector3 spawnPoint;


	void Start() {
		InvokeRepeating ("spawnEnemy", spawnTime, interval);
	}
	
	// Update is called once per frame
	void Update () {
        // identify all enemy objects in the world
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        // spawn until limit has been reached
        if (enemies.Length >= amount)
        {
            // call spawn function
			CancelInvoke ();
        }

	}

    // spawn function
    void spawnEnemy ()
    {
        // randomly assign a coordinate within world space to the enemy
		spawnPoint.x = Random.Range(-radius, radius);
		spawnPoint.y = gameObject.transform.position.y;
		spawnPoint.z = Random.Range(-radius, radius);

        // make an instance of a random enemy at the spawnPoint and align it with world coordinates
		GameObject enemy = enemies[Random.Range(0, enemies.Length - 1)];

		Instantiate (enemy, gameObject.transform.position + spawnPoint + enemy.transform.position, Quaternion.identity);
        // cancel the invoke and return
    }
}
