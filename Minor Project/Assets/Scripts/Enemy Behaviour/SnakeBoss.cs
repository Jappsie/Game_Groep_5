using UnityEngine;
using System.Collections;
using System;

public class SnakeBoss : HealthSystem {


    public GameObject SnakeHead;
    public GameObject SnakeFront;
    public GameObject SnakeTail;

    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    public float chainDistance = 1.2f;

    public float attackDistance = 10f;
    public float FOV = 30f;
    public float SnakeDamage = 1f;

    public GameObject Crystal;
    public int crystalAmount = 4;
    public float StartTime;
    public float damageDelay;
    public float intervalTime;
    public float spawnDepth = -2.5f;
    public float CrystalDamage = 1f;

    private Quaternion rotFix = Quaternion.Euler( 90, 0, 0 );
    private GameObject player;
    private Vector3 headStartPos;
    private Animator animator;
    private bool finishAnimation;

    private Stack crystals;
    private bool crystalSequence;
    private bool Vulnerable;


	void Start () {
        // Get player, animator and default head position
        player = GameObject.FindGameObjectWithTag( "Player" );
        animator = gameObject.GetComponent<Animator>();
        headStartPos = SnakeHead.transform.position;

        // Can we take damage?
        Vulnerable = false;

        // Crystal initialization
        crystals = new Stack();
        crystalSequence = false;
        Invoke( "spawnRocks", StartTime );

	}
	
	// Update is called once per frame
	protected override void Update () {
        // Check health stuffs
        base.Update();

        // Define behaviour paths
        if ( (isClose() || finishAnimation) && !crystalSequence )
        {
            animator.SetBool( "Attack", true );
            NeckAnimation();
        }
        else if (crystalSequence)
        {
            animator.SetBool( "Crystals", true );
        }
        else
        {
            FrontFollow();
            TailFollow();
            SnakeMovement();
        }
    }

    // Check if the player is in range, and in field of vision
    private bool isClose()
    {
        Vector3 playerpos = player.transform.position;
        Vector3 headpos = SnakeHead.transform.position;
        bool fieldOfVision = Mathf.Abs(Vector3.Angle( SnakeHead.transform.forward, playerpos - headpos )) < FOV;
        bool distance =  Vector3.Distance( playerpos, headpos ) < attackDistance;
        return fieldOfVision && distance;
    }

    // Interface for animation events: make sure they finish.
    private void waitForAnimation(int value)
    {
        switch (value)
        {
            case 0:
                finishAnimation = true;
                Vulnerable = true;
                break;
            case 1:
                animator.SetBool( "Attack", false );
                finishAnimation = false;
                Vulnerable = false;
                break;
            case 2:
                finishAnimation = true;
                break;
            case 3:
                finishAnimation = false;
                break;
        }
    }

    // Method used for the tail movement
    private void TailFollow()
    {
        // Get all the slices
        Transform[] TailParts = SnakeTail.GetComponentsInChildren<Transform>(); //also returns parent
        TailParts[0] = SnakeFront.transform.GetChild( SnakeFront.transform.childCount - 1 );

        // Go through all but the front slice
        for (int i = 1; i < TailParts.Length; i++ )
        {
            // Used variables
            Transform Front = TailParts[ i - 1 ];
            Transform Back = TailParts[ i ];

            // Check if the positions are too far apart
            if ( Vector3.Distance( Front.position, Back.position ) > chainDistance )
            {
                Back.position = Vector3.MoveTowards( Back.position, Front.position, followSpeed * Time.deltaTime);
            }
            // Look directly at the next slice
            Quaternion toNext = Quaternion.LookRotation( Front.position - Back.position ) * rotFix;
            // Interpolate between 'looking at' and imitating
            toNext = Quaternion.Slerp( toNext, Front.rotation, 0.3f );
            Back.rotation = Quaternion.RotateTowards( Back.rotation, toNext, rotationSpeed * Time.deltaTime );
        }

    }

    // Method used for the front movement
    private void FrontFollow()
    {
        // Get all the slices
        Transform[] FrontParts = SnakeFront.GetComponentsInChildren<Transform>(); //also returns parent
        FrontParts[ 0 ] = SnakeHead.transform.GetChild( SnakeHead.transform.childCount - 1 );

        // Go through all but the front slice
        for ( int i = 1; i < FrontParts.Length; i++ )
        {
            // Used variables
            Transform Front = FrontParts[ i - 1 ];
            Transform Back = FrontParts[ i ];

            // Check if the positions are too far apart
            if ( Vector3.Distance( Front.position, Back.position ) > chainDistance )
            {
                // Let the neck keep its structure by fixing the y position
                Vector3 target = new Vector3( Front.position.x, Back.position.y, Front.position.z );
                Back.position = Vector3.MoveTowards( Back.position, target, followSpeed * Time.deltaTime );
            }
            // Look directly at the next slice
            Quaternion toNext = Quaternion.LookRotation( Front.position - Back.position ) * rotFix;
            // Interpolate between 'looking at' and imitating
            toNext = Quaternion.Slerp( toNext, Front.rotation, 0.3f );
            Back.rotation = Quaternion.RotateTowards( Back.rotation, toNext , rotationSpeed * Time.deltaTime );
        }
    }

    // Method used for moving the whole snake
    private void SnakeMovement()
    {
        // Used variables
        Vector3 playerPos = player.transform.position;
        Vector3 snakePos = SnakeHead.transform.position;
        Quaternion snakeRot = SnakeHead.transform.rotation;
        Vector3 moveDirection = snakePos + SnakeHead.transform.forward;

        // Make sure the head stays upright
        moveDirection.y = headStartPos.y;
        playerPos.y = snakePos.y;

        SnakeHead.transform.position = Vector3.MoveTowards( snakePos, moveDirection, followSpeed/2 * Time.deltaTime );
        SnakeHead.transform.rotation = Quaternion.RotateTowards( snakeRot, Quaternion.LookRotation( playerPos - snakePos ), rotationSpeed * Time.deltaTime );
    }

    // Method used to make the head animation feel smoother
    private void NeckAnimation()
    {
        // Get all the slices
        Transform[] NeckParts = SnakeFront.GetComponentsInChildren<Transform>(); //also returns parent
        NeckParts[ 0 ] = SnakeHead.transform.GetChild( SnakeHead.transform.childCount - 1 );

        for (int i = 1; i < NeckParts.Length-1; i++ )
        {
            Transform Front = NeckParts[ i - 1 ];
            Transform NeckPart = NeckParts[ i ];
            Transform Back = NeckParts[ i + 1 ];

            Vector3 back = new Vector3( Back.position.x, NeckPart.position.y, Back.position.z );
            Vector3 front = new Vector3( Front.position.x, NeckPart.position.y, Front.position.z );
            NeckPart.position = Vector3.Slerp( Back.position, Front.position, 0.5f );
          
            // Look directly at the next slice
            Quaternion toNext = Quaternion.LookRotation( Front.position - NeckPart.position ) * rotFix;
            //Interpolate between 'looking at' and imitating
            toNext = Quaternion.Slerp( toNext, Front.rotation, 0.5f );
            NeckPart.rotation = Quaternion.RotateTowards( NeckPart.rotation, toNext, rotationSpeed * Time.deltaTime );
        }
    }

    // Method which handles the crystals
    private void spawnRocks()
    {
        foreach (GameObject curCryst in crystals)
        {
            Destroy( curCryst );
        }
        crystals.Clear();
        crystalSequence = true;
        StartCoroutine( spawnRock() );
    }

    // Coroutine which handles crystal spawning, and linking the animation
    IEnumerator spawnRock()
    {
        for (int i = 0; i < crystalAmount; i++ )
        {
            Vector3 playerLocation = player.transform.position;
            playerLocation.y = spawnDepth;
            animator.SetBool( "Crystal", true );
            GameObject curCrystal = Instantiate( Crystal, playerLocation, Quaternion.identity ) as GameObject;
            BoxCollider box = curCrystal.GetComponentInChildren<BoxCollider>();
            box.enabled = false;
            crystals.Push( curCrystal );
            yield return new WaitForSeconds( damageDelay );
            animator.SetBool( "Crystal", false );
			if (box) {
				box.enabled = true;
			}
            yield return new WaitForSeconds( intervalTime );
        }
        animator.SetBool( "Crystals", false );
        crystalSequence = false;
    }

    // Method to check for damage
    private void OnTriggerEnter( Collider other )
    {
        if (Vulnerable && other.CompareTag("Crystal"))
        {
            Vulnerable = false;
            spawnRocks();
            TakeDamage( CrystalDamage );
        }
    }

    // Method to activate something when defeated
    protected override void Death()
    {
        Destroy( gameObject );
    }
}
