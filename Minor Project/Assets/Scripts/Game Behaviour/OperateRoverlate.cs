using UnityEngine;
using System.Collections;

public class OperateRoverlate : MonoBehaviour {

    public bool reset;
    public int vertical;
    public int angular;
    public GameObject cylinder;

    private void OnCollisionEnter( Collision collision )
    {
        cylinder.GetComponent<Rovelation>().Rovelate( reset, vertical, angular );
    }
}
