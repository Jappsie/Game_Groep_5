using UnityEngine;
using System.Collections;

public class SnakeBoss : MonoBehaviour {

    public GameObject SnakeHead;
    public GameObject SnakeFront;
    public GameObject SnakeTail;

    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    public float chainDistance = 1.2f;

    private Quaternion rotFix = Quaternion.Euler( 90, 0, 0 );


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        FrontFollow();
        TailFollow();
	}

    private void TailFollow()
    {
        Transform[] TailParts = SnakeTail.GetComponentsInChildren<Transform>(); //also returns parent
        TailParts[0] = SnakeFront.transform.GetChild( SnakeFront.transform.childCount - 1 );

        for (int i = 1; i < TailParts.Length; i++ )
        {
            Transform Front = TailParts[ i - 1 ];

            Transform Back = TailParts[ i ];

            if ( Vector3.Distance( Front.position, Back.position ) > chainDistance )
            {
                Back.position = Vector3.MoveTowards( Back.position, Front.position, followSpeed * Time.deltaTime);
            }
            Back.rotation = Quaternion.RotateTowards( Back.rotation, Quaternion.LookRotation( Front.position - Back.position ) * rotFix, rotationSpeed * Time.deltaTime);
        }

    }

    private void FrontFollow()
    {
        Transform[] FrontParts = SnakeFront.GetComponentsInChildren<Transform>(); //also returns parent
        FrontParts[0] = SnakeHead.transform.GetChild( SnakeHead.transform.childCount - 1 );

        for (int i = 1; i < FrontParts.Length; i++ )
        {
            Transform Front = FrontParts[ i - 1 ];

            Transform Back = FrontParts[ i ];

            if ( Vector3.Distance( Front.position, Back.position ) > 1 )
            {
                Back.position = Vector3.MoveTowards( Back.position, Front.position, followSpeed * Time.deltaTime );
            }
            Back.rotation = Quaternion.RotateTowards( Back.rotation, Quaternion.LookRotation( Front.position - Back.position ) * rotFix, rotationSpeed * Time.deltaTime );
        }
    }
}
