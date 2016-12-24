using UnityEngine;
using System.Collections;

public class EnableScript : MonoBehaviour {

	public GameObject[] toEnable;
	public bool Status;

    private void Start()
    {
        toggle( Status );
    }

    public void toggle ()
    {
        toggle( !Status );
    }

    public void toggle(bool curStatus)
    {
        for (int i = 0; i < toEnable.Length; i++ )
        {
            toEnable[ i ].gameObject.SetActive( curStatus );
        }
    }
}