using UnityEngine;
using System.Collections;

public class Rovelation : MonoBehaviour {

    public GameObject SnakeHead;
	public GameObject Camera;
    public float speed = 0.3f;
    public int steps = 5;
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
        gameObject.transform.position = Vector3.Slerp( gameObject.transform.position, startPos + new Vector3(0,targetPos,0), speed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Slerp( gameObject.transform.rotation, targetRot, speed * Time.deltaTime );

        if ( Vector3.Distance(gameObject.transform.position, startPos + new Vector3(0,steps*stepSize,0)) < 1 && gameObject.transform.childCount == 1 && Quaternion.Angle(gameObject.transform.GetChild(0).rotation, Quaternion.Euler(0,0,0)) < 5)
        {
            SnakeHead.GetComponent<Animator>().SetTrigger( "SolvedPuzzle" );
        }

	}

    public void Rovelate(bool reset, int step, int rot)
    {
        if (reset)
        {
            targetPos = 0f;
            targetRot = Quaternion.identity;
            return;
        }
        Debug.Log( "Vertical: " + step + " Angle: " + rot );
        if (targetPos + step*stepSize >= 0 && targetPos + step*stepSize <= steps*stepSize)
        {
            targetPos += step * stepSize;
            targetRot *= Quaternion.Euler( new Vector3( 0, rot * 45, 0 ) );
			Camera.GetComponent<CameraController> ().Shake ();
			gameObject.GetComponent<AudioSource> ().Play ();
        }

    }

    private void OnTriggerEnter( Collider other )
    {
        Debug.Log( "Triggered" );
        if (other.CompareTag("Pushable"))
        {            
            other.gameObject.transform.parent = gameObject.transform;
            other.tag = "Untagged";
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.localPosition = new Vector3( 0, 0.01165f, 0 );
        }


    }


}
