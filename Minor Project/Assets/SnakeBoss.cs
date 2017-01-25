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
        Transform FrontPart = SnakeFront.transform.GetChild( SnakeFront.transform.childCount - 1 );
        Transform[] TailParts = SnakeTail.GetComponentsInChildren<Transform>(); //also returns parent

        for (int i = 1; i < TailParts.Length; i++ )
        {
            Transform Front;
            if (i == 1)
            {
                Front = FrontPart;
            }
            else
            {
                Front = TailParts[ i - 1 ];
            }
            Transform Back = TailParts[ i ];

            if ( Vector3.Distance( Front.position, Back.position ) > chainDistance )
            {
                float LinSpeed = followSpeed * Time.deltaTime;
                if (LinSpeed > 0.9) { LinSpeed = 0.9f; };
                Back.position = Vector3.Slerp( Back.position, Front.position, LinSpeed);
            }
            float RotSpeed = followSpeed * Time.deltaTime;
            if ( RotSpeed > 0.9 ) { RotSpeed = 0.9f; };
            Back.rotation = Quaternion.Slerp( Back.rotation, Quaternion.LookRotation( Front.position - Back.position ) * rotFix, RotSpeed);
        }

    }

    private void FrontFollow()
    {
        Transform HeadPart = SnakeHead.transform.GetChild( SnakeHead.transform.childCount - 1 );
        Transform[] FrontParts = SnakeFront.GetComponentsInChildren<Transform>(); //also returns parent

        for (int i = 1; i < FrontParts.Length; i++ )
        {
            Transform Front;
            if ( i == 1 )
            {
                Front = HeadPart;
            }
            else
            {
                Front = FrontParts[ i - 1 ];
            }
            Transform Back = FrontParts[ i ];

            if ( Vector3.Distance( Front.position, Back.position ) > 1 )
            {
                float LinSpeed = followSpeed * Time.deltaTime;
                if ( LinSpeed > 0.9 ) { LinSpeed = 0.9f; };
                Back.position = Vector3.Slerp( Back.position, Front.position, followSpeed * Time.deltaTime );
            }
            float RotSpeed = followSpeed * Time.deltaTime;
            if ( RotSpeed > 0.9 ) { RotSpeed = 0.9f; };
            Back.rotation = Quaternion.Slerp( Back.rotation, Quaternion.LookRotation( Front.position - Back.position ) * rotFix, rotationSpeed * Time.deltaTime );
        }
    }
}
