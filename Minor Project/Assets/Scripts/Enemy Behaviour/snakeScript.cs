using UnityEngine;
using System.Collections;

public class snakeScript : MonoBehaviour {

    private GameObject player;
    private Vector3 playerLocation;
    private Stack crystals;

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
        Invoke("spawnRocks", startTime);
	}
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    void spawnRocks ()          //Method die de sequence start
    {
        Debug.Log(crystals.Count);
        foreach (GameObject curCryst in crystals)       //Verwijder alle bestaande stenen
        {
           Destroy(curCryst);
        }
        crystals.Clear();
        StartCoroutine("spawnRock");        //start het spawnen van de stenen
    }

    IEnumerator spawnRock ()    //Method die het spawnen van de stenen regelt
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(player.Equals(null));
            playerLocation = player.transform.position;
            playerLocation.y = -2.5f;
            Debug.Log(playerLocation);
            GameObject curCrystal = Instantiate(crystal, playerLocation, Quaternion.identity) as GameObject;
            crystals.Push(curCrystal);
            yield return new WaitForSeconds(intervalTime);
            curCrystal.GetComponentInChildren<BoxCollider>().enabled = true;
            //curCrystal.transform.position += Vector3.up * 5.65f;
        }
        yield return new WaitForSeconds(repeatTime);
        spawnRocks();
    }
}
