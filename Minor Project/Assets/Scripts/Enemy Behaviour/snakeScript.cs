using UnityEngine;
using System.Collections;

public class snakeScript : HealthSystem {

    private GameObject player;
    private Vector3 playerLocation;
    private Stack crystals;
	private bool crystalSequence;

    public GameObject crystal;
    public float startTime; // Tijd tussen het starten van de scene, en het beginnen van de eerste spawn sequence
    public float intervalTime; // tussen maken van 2 stenen
    public float repeatTime; // Tijd tussen twee opeenvolgende sequences

 
	void Start () {
        if (repeatTime < 5f* intervalTime)       //Als de repeatTime > 4*intervalTime, dan gaat het fout
        {
            repeatTime = 5f * intervalTime + 1;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        crystals = new Stack ();
		crystalSequence = false;
        Invoke("spawnRocks", startTime);
	}

	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
		if (!crystalSequence) {
			gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, Quaternion.LookRotation (player.transform.position - gameObject.transform.position), 4f * Time.deltaTime);
			gameObject.transform.position += gameObject.transform.forward * 10f * Time.deltaTime;
		}
	}

    void spawnRocks ()          //Method die de sequence start
    {
        Debug.Log(crystals.Count);
        foreach (GameObject curCryst in crystals)       //Verwijder alle bestaande stenen
        {
           Destroy(curCryst);
        }
        crystals.Clear();
		crystalSequence = true;
        StartCoroutine("spawnRock");        //start het spawnen van de stenen
    }

    IEnumerator spawnRock ()    //Method die het spawnen van de stenen regelt
    {
        for (int i = 0; i < 4; i++)
        {
            playerLocation = player.transform.position;
            playerLocation.y = -2.5f;
            GameObject curCrystal = Instantiate(crystal, playerLocation, Quaternion.identity) as GameObject;
            crystals.Push(curCrystal);
            yield return new WaitForSeconds(intervalTime);
            curCrystal.GetComponentInChildren<BoxCollider>().enabled = true;
        }
		crystalSequence = false;
        yield return new WaitForSeconds(repeatTime);
        spawnRocks();
    }

	private void OnCollisionEnter(Collision col) {
		Debug.Log ("Collision met: " + col.gameObject.tag);
		if (col.gameObject.CompareTag ("Crystal")) {
			Debug.Log ("Damage werkt");
			TakeDamage (1f);
		}
	}

	protected override void Death ()
	{
		Destroy (gameObject);
	}
}
