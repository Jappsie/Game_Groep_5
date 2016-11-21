using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneManagerScript : MonoBehaviour {

    public List<Object> DontDestroy;
    public Object StartScene;
    public KeyCode resetKey = KeyCode.R;    // Reset key

    static SceneManagerScript SceneManagementInstance;  // Static SceneManager voor eenmalig maken van DontDestroyOnLoad

    // Vermijd dubbele objecten
    void Awake()
    {
        DontDestroy.Add(gameObject);
        if (SceneManagementInstance != null)
        {
            foreach (Object obj in DontDestroy)
            {
                Destroy(obj);
            }
        }
        else
        {
            foreach (Object obj in DontDestroy)
            {
                DontDestroyOnLoad(obj);
            }
            SceneManagementInstance = this;
        }
    }

    // Check voor een reset
	void Update() {
		if (Input.GetKeyDown (resetKey)) {
			this.Awake();
			SceneManager.LoadScene (StartScene.name);
		}
	}

    // Methode om scene te wisselen, dit zodat er later uitgebreid kan worden
    public static void goToScene(string scene, bool Additive)
    {
        LoadSceneMode mode = LoadSceneMode.Single;
        if (Additive)
        {
            mode = LoadSceneMode.Additive;
        }
        SceneManager.LoadScene(scene, mode);
    }
}
