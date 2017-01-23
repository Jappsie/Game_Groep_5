using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AvatarChanger : MonoBehaviour
{

    public string fileLocation;
    public Image image;
    private Sprite[] sprites;


    private void Start()
    {
        sprites = Resources.LoadAll<Sprite>( fileLocation );
        SceneManager.sceneLoaded += Reload;
        ChangeAvatar();
    }

    private void ChangeAvatar()
    {
        image.sprite = sprites[ PlayerPrefs.GetInt( "CharacterSelected" ) ];
    }

    private void Reload( Scene scene, LoadSceneMode mode )
    {
        ChangeAvatar();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= Reload;
    }


}
