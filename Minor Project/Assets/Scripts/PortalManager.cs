using UnityEngine;
using System.Collections;

public class PortalManager : MonoBehaviour {

    public Object Scene;        // Scene om naar te teleporteren
    public Vector3 Position;    // Positie om naar te teleporteren
    public bool Additive;       // Boolean om te kiezen voor additive teleporteren

    // Ga naar de goede scene + positie wanneer een player object collide
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManagerScript.goToScene(Scene.name, Additive);
            other.transform.position = Position + new Vector3(0, other.transform.position.y, 0);
        }
    }

}
