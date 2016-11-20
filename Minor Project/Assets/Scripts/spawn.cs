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
    // point in 3D space where the enemy will be spawned
    private Vector3 spawnPoint;

	
	// Update is called once per frame
	void Update () {
        // identify all enemy objects in the world
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        // spawn until limit has been reached
        if (enemies.Length <= amount)
        {
            // call spawn function
            InvokeRepeating("spawnEnemy", spawnTime, interval);
        }

	}

    // spawn function
    void spawnEnemy ()
    {
        // randomly assign a coordinate within world space to the enemy
        spawnPoint.x = Random.Range(-4, 4);
        spawnPoint.y = 0.5f;
        spawnPoint.z = Random.Range(-4, 4);

        // make an instance of a random enemy at the spawnPoint and align it with world coordinates
        Instantiate (enemies[Random.Range(0, enemies.Length - 1)], spawnPoint, Quaternion.identity);
        // cancel the invoke and return
        CancelInvoke ();
    }
}
