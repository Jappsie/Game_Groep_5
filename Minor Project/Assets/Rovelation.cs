using UnityEngine;
using System.Collections;

public class Rovelation : MonoBehaviour {

    public float speed = 0.3f;
    public int steps = 4;
    public float stepSize = 1f;
    private Vector3 startPos;

    private Quaternion targetRot = Quaternion.identity;
    private float targetPos = 0;

    private void Start()
    {
        startPos = gameObject.transform.position;
    }


    // Update is called once per frame
    void Update () {
	    if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Elevate( 1 );
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Elevate( -1 );
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rotate( 1 );
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Rotate( -1 );
        }
        gameObject.transform.position = Vector3.Slerp( gameObject.transform.position, startPos + new Vector3(0,targetPos,0), speed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Slerp( gameObject.transform.rotation, targetRot, speed * Time.deltaTime );

	}

    void Elevate (int step)
    {
            targetPos += step*stepSize;
            if ( targetPos > steps*stepSize )
            {
                targetPos = steps*stepSize;
            }

            if ( targetPos < 0 )
            {
                targetPos = 0;
            }
    }

    void Rotate(int step )
    {
            targetRot *= Quaternion.Euler( new Vector3( 0, step*45, 0 ) );
    }


}
