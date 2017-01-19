using UnityEngine;
using System.Collections;

public class ObjectPush : MonoBehaviour {

	public float Speed; 
	public Vector3 gridSize;

	private bool push = false;
	private int direction;
	private Vector3 target;
	private Rigidbody thing;

	void Start () {
		
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
        if ( !hit.gameObject.CompareTag( "Pushable" ) || push)
        {
            return;
        }

		thing = hit.collider.GetComponent<Rigidbody>();

		direction = 0;
		float temp = Mathf.Abs (hit.moveDirection.x);
		if (temp < Mathf.Abs (hit.moveDirection.z)) {
			direction = 1;
		}

		switch (direction){
		case 0:
			target = thing.transform.position + new Vector3 (Mathf.Sign(hit.moveDirection.x) * gridSize.x, 0, 0);
			break;
		case 1:
			target = thing.transform.position + new Vector3 (0, 0, Mathf.Sign(hit.moveDirection.z) * gridSize.z);
			break;
		}
		push = true;
	}

	void FixedUpdate () {
		if (!push) {
			return;
		}
        if ( thing.gameObject.CompareTag( "Pushable" ) )
        {
            Debug.Log( "fixedUpdate" );
            thing.transform.position = Vector3.MoveTowards( thing.transform.position, target, Speed * Time.deltaTime );

            if ( target.Equals( thing.transform.position ) )
            {
                push = false;
            }
        }
	}
}
