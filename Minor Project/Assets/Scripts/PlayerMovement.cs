using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float speed;     // Snelheid variabele

    private Rigidbody rb;   // Physics engine

    // Vind het physics engine object
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
    // Update de forces iedere update dat er forces gebruikt worden
	void FixedUpdate () {
        float horizontalMovement = Input.GetAxis("Horizontal");        
        float verticalMovement = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalMovement, 0.0f, verticalMovement);

        rb.AddForce(movement * speed);
    }    

}