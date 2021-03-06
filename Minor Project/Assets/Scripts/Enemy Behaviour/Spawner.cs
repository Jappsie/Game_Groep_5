﻿using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

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

	private Renderer[] renders;

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
		// Create collider array to check collision with environment
		Collider[] colliderChecker;
		//Pick a random enemy type
		GameObject enemy = enemies [Random.Range (0, enemies.Length)];
		int counter = 0;

		do {
			// randomly assign a coordinate within world space to the enemy
			spawnPoint.x = Random.Range(-radius, radius);
			spawnPoint.z = Random.Range(-radius, radius);

			Vector3 finalSpawn = gameObject.transform.position + spawnPoint + enemy.transform.position;

			// Fill an array with all the collision with a sphere around the enemy's BoxCollider with a ray of 0.5 * the width of the BoxCollider
			colliderChecker = Physics.OverlapSphere (finalSpawn, 1.7f);

			for (int i = 0; i < colliderChecker.Length; i++) {
				Debug.Log(colliderChecker[i]);
			}
			counter++;
		//Pick spawnpoints until it doesn't collide
		} while (colliderChecker.Length != 0 && counter < 10);
		GameObject spawnedOne = (GameObject)Instantiate(enemy, gameObject.transform.position + spawnPoint
			+ enemy.transform.position, Quaternion.identity);
		count++;
		// apply procedural generation on the spawned GameObject
		proceduralEnemyGeneration(spawnedOne);
    }

    // procedurally generates enemies that differ in strength
    void proceduralGeneration(GameObject creature)
    {
        // random roulette selector [0.0, 1.0]
        float target = Random.value;
        // probability for instantiating the overpowered enemy
        float limit = 0.15f;
        // get the parameters of the enemy
        EnemyFollowing enemyParams = creature.GetComponent<EnemyFollowing>();

        if (target < limit)
        {
            // black (overpowered enemy)
            creature.transform.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            enemyParams.Enemylife = 100;
            enemyParams.moveSpeed = 20;
            enemyParams.Damage = 9001;
        }
        else
        {
            // red (weak enemy)
            creature.transform.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
            enemyParams.Enemylife = 1;
            enemyParams.moveSpeed = 1;
            enemyParams.Damage = 0.1f;
        } 
    }

    void proceduralEnemyGeneration(GameObject creature)
    {
        float color = Random.value;

        EnemyFollowing enemyParams = creature.GetComponent<EnemyFollowing>();
		renders = creature.GetComponentsInChildren<Renderer> ();
		foreach (Renderer rend in renders) {
			rend.material.color = new Color(1.0f - color, color, 1.0f / color);
		}
        enemyParams.Enemylife = (int)(9 * color + 1); // [1, 10]
        enemyParams.moveSpeed = (int)(12 * color + 2); // [2, 10]
        enemyParams.Damage = 19.9f * color + 0.1f; // [0, 10]
    }
}
