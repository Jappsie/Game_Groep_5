using UnityEngine;
using System.Collections;

public class BallAnimation : MonoBehaviour {

    Animator animator;
	// Use this for initialization

	void Start () {
        animator = GetComponent<Animator>();
	}

    private void OnCollisionEnter( Collision other )
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            animator.Play( "RollingBall" );
        }
    }

}
