using UnityEngine;
using System.Collections;

public class FallingTree : MonoBehaviour {

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void playAnimation()
    {

        animator.Play( "FallingTree" );
    }
}
