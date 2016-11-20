using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    static SceneManagerScript SceneManagementInstance;
    public Object MainCamera;
	public KeyCode resetKey;
	public Object loadScene;

    
    void Awake()
    {
		//Default reset knop is "R"
		resetKey = KeyCode.R;

        // Avoid duplcates in DontDestroyOnLoad scene
        if (SceneManagementInstance != null)
        {
            Destroy(gameObject);
            Destroy(MainCamera);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(MainCamera);
            SceneManagementInstance = this;
        }
    }

	void Update() {
		if (Input.GetKeyDown (resetKey)) {
			this.Awake();
			SceneManager.LoadScene (loadScene.name);
		}
	}

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
