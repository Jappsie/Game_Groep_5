using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TimeCounter : MonoBehaviour
{

    public Text counterText;
    public float seconds, minutes;

    private float startTime;
    private float playTime;

    // Use this for initialization
    void Start()
    {
        startTime = PlayerPrefs.GetFloat( "StartTime" );
        playTime = PlayerPrefs.GetFloat( "PlayTime" );
        Debug.Log( startTime );
        Debug.Log( playTime );

        counterText = GetComponent<Text>() as Text;

    }

    // Update is called once per frame
    void Update()
    {
        minutes = (int) ((Time.time - startTime + playTime) / 60f);
        seconds = (int) ((Time.time - startTime + playTime) % 60f);
        counterText.text = minutes.ToString( "00" ) + ":" + seconds.ToString( "00" );
    }
}
