using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    static SceneManagerScript SceneManagementInstance;
    public Object MainCamera;

    
    void Awake()
    {
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
