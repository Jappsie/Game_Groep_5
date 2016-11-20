using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
	public bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		isDead = false;
    }
	
	void FixedUpdate () {
        float horizontalMovement = Input.GetAxis("Horizontal");        
        float verticalMovement = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalMovement, 0.0f, verticalMovement);

		//Speler mag niet bewegen, als deze dood is
		if (!isDead) {
			rb.AddForce (movement * speed);
		}

    }

    
}