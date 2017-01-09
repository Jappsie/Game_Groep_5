using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class LoadData : MonoBehaviour {

    public GameObject sphere;

	// Use this for initialization
	void Start () {
        StreamReader reader = new StreamReader( "Data.json" );
        string dataText = reader.ReadToEnd();
        Data[] data = JSONHelper.getJsonArray<Data>( dataText );
        foreach (Data value in data)
        {
            if ( value.Scene.Equals( "Boss" ) )
            {
                Vector3 position = new Vector3( value.XPos, value.YPos, value.ZPos );
                GameObject.Instantiate( sphere, position, Quaternion.identity );
            }
        }
	}

    
	
}

public class JSONHelper
{
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>( newJson );
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

[System.Serializable]
public class Data
{
    public int Id;
    public int Death;
    public string Scene;
    public string Date;
    public float XPos;
    public float YPos;
    public float ZPos;
}
