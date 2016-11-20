using UnityEngine;
using System.Collections;

public class PortalManager : MonoBehaviour {

    public Object Scene;
    public bool Additive;
    public Vector3 Position;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            string portalName = gameObject.name;
            int Portal = int.Parse(portalName.Replace("Portal ", ""));
            SceneManagerScript.goToScene(Scene.name, Additive);
            other.transform.position = Position + new Vector3(0, other.transform.position.y, 0);
        }
    }

}
