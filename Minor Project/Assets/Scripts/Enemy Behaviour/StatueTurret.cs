using UnityEngine;
using System.Collections;
using System;

public class StatueTurret : HealthSystem
{
    public GameObject bullet;
    public Vector3 spawnOffset = new Vector3(0,2,1.3f);
    public float rotationSpeed = 1f;
    public float LineOfSight = 10f;
    public float FieldOfView = 45f;
    public float activationTime = 1f;
    public float cooldownTime = 1f;
    public bool adaptive = false;
    public bool vulnerable = true;

    protected GameObject player;
    protected Vector3 startPos;
    protected Quaternion startRot;
    protected bool canFire = true;
    protected Vector3 playerPos;
    protected Vector3 objectPos;
    protected Quaternion objectRot;
    protected Vector3 bulletPos;

    private int deaths;
    private double repeatrate;
    private EnableScript toggler;
    private Renderer rendererStatue;
    private Color color;
    private Color flicker;

    // Get the player object
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag( "Player" );
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
        toggler = GetComponent<EnableScript>();
        if ( adaptive )
        {
            deaths = SceneManagerScript.deathList.Count;        
            //When deaths == 0, repeatrate is equal to itself
            activationTime = 2f * activationTime * (1 / (1 + Mathf.Exp( -0.3f * deaths )));
            cooldownTime = 2f * cooldownTime * (1 / (1 + Mathf.Exp( -0.3f * deaths )));
        }
        rendererStatue = gameObject.GetComponent<Renderer>();
        color = rendererStatue.material.color;
        flicker = color;
        flicker.a = 0.2f;
    }

    // Always look at the player
    protected override void Update()
    {
        if ( player != null )
        {
            base.Update();
            playerPos = player.transform.position + new Vector3( 0, 1f, 0 ); // Vector as compensation for mass-position
            objectPos = gameObject.transform.position;
            objectRot = gameObject.transform.rotation;
            if ( Vector3.Distance( playerPos, objectPos ) < LineOfSight && Vector3.Angle( playerPos - objectPos, objectRot * Vector3.forward ) < FieldOfView )
            {
                if ( canFire )
                {
                    bulletPos = objectPos + objectRot * spawnOffset;
                    RaycastHit hit;
                    Ray playerRay = new Ray( bulletPos, playerPos - bulletPos );
                    if ( Physics.Raycast( playerRay, out hit ) && hit.collider.gameObject.Equals( player ) )
                    {
                        canFire = false;
                        StartCoroutine( fire( playerPos ) );
                    }
                }
                playerPos.y = objectPos.y;
                transform.rotation = Quaternion.Slerp( objectRot, Quaternion.LookRotation( playerPos - objectPos ), rotationSpeed * Time.deltaTime );
            }
            else
            {
                transform.rotation = Quaternion.Slerp( objectRot, startRot, rotationSpeed * Time.deltaTime );
            }
        }
        else
        {
            Start();
        }

    }

    protected virtual IEnumerator fire( Vector3 PlayerPos )
    {
        yield return new WaitForSecondsRealtime( activationTime );
        Quaternion angleY = Quaternion.LookRotation( PlayerPos - bulletPos);
        Instantiate( bullet,  bulletPos, Quaternion.Euler(angleY.eulerAngles.x, objectRot.eulerAngles.y, 0) );
        yield return new WaitForSecondsRealtime( cooldownTime );
        canFire = true;
    }

    private void OnCollisionEnter( Collision collision )
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy( collision.gameObject );
            if ( vulnerable )
            {
                TakeDamage( 1f );
                StartCoroutine( "Flicker" );
            }
        }
    }

    // What happens when the object dies
    protected override void Death()
    {
        Debug.Log( "Im dead!" );
        if ( toggler != null )
        {
            toggler.toggle( true );
        }
        Destroy( gameObject );
    }

    IEnumerator Flicker()
    {
        vulnerable = false;
        Debug.Log( "Invulnerable: " + Time.time );
        for ( int i = 0; i < 8; i++ )
        {
            rendererStatue.material.color = flicker;
            yield return new WaitForSeconds( 0.05f );
            rendererStatue.material.color = color;
            yield return new WaitForSeconds( 0.05f );
        }
        vulnerable = true;
        Debug.Log( "Vulnerable: " + Time.time );
    }
}
