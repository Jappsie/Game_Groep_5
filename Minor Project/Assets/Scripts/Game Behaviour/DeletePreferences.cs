using UnityEngine;
using System.Collections;

public class DeletePreferences : MonoBehaviour {

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
}
