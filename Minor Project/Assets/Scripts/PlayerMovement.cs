using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 10f;       // Speed variable
    public bool isDead;             // isDead tag
    public float gravity = 14.0f;    // Gravity effect
    public float jumpForce = 10.0f;  // Force of jump

    private CharacterController controller;     // Controller of the movement
    private float verticalVelocity;            // Velocity regarding jump/gravity

    // Get reference to the CharacterController
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Make the object move
    void Update()
    {
        // Jump if not floating
        if ( controller.isGrounded )
        {
            verticalVelocity = 0.0f;
            if ( Input.GetKey( KeyCode.Space ) )
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Move about in the 2D area
        float horizontalMovement = Input.GetAxis( "Horizontal" );
        float verticalMovement = Input.GetAxis( "Vertical" );
        Vector3 movement = new Vector3( speed * horizontalMovement, verticalVelocity, speed * verticalMovement );
        controller.Move( movement * Time.deltaTime );
    }

}