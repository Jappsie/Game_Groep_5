using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour {

    // list of enemy objects that can be spawned
    public GameObject[] enemies;
    // amount of enemies that need to be spawned
    public int amount;
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
        if (count >= amount)
        {
			CancelInvoke ();
        }
	}

    // spawn function
    void spawnEnemy ()
    {
        count++;
        // randomly assign a coordinate within world space to the enemy
		spawnPoint.x = Random.Range(-radius, radius);
		spawnPoint.y = gameObject.transform.position.y;
		spawnPoint.z = Random.Range(-radius, radius);

        // make an instance of a random enemy at the spawnPoint and align it with world coordinates
		GameObject enemy = enemies[Random.Range(0, enemies.Length)];

		Instantiate (enemy, gameObject.transform.position + spawnPoint + enemy.transform.position, Quaternion.identity);
    }
}
