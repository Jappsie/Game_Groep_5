using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

public class LoadData : MonoBehaviour {

    public GameObject sphere;
	public GameObject skull;
	public string level;
    public bool death;

	private Collider[] curColliders;
    private List<Object> objects = new List<Object>();

	// Use this for initialization
	void Start () {
        StreamReader reader = new StreamReader( "Data.json" );
        string dataText = reader.ReadToEnd();
        Data[] data = JSONHelper.getJsonArray<Data>( dataText );
		float radius = sphere.transform.localScale.x;
        foreach (Data value in data)
        {
			if (value.Scene.Equals (level) && value.Death == 0 && !death) {
				Vector3 position = new Vector3 (value.XPos, value.YPos, value.ZPos);
				curColliders = Physics.OverlapSphere (position, radius, 1 << 9);
				foreach (Collider col in curColliders) {
                        col.gameObject.GetComponent<Renderer>().material.color -= new Color( -3f / 255f, 3f / 255f, 0, -1f / 255f );
				}
				objects.Add(GameObject.Instantiate (sphere, position, Quaternion.identity));
			} else if (value.Scene.Equals (level) && value.Death == 1 && death) {
				Vector3 position = new Vector3 (value.XPos, value.YPos, value.ZPos);
				objects.Add(GameObject.Instantiate (skull, position, Quaternion.identity));

			}
        }
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            death = !death;
            Debug.Log( objects.Count );
            foreach (Object value in objects)
            {
                Destroy( value );
            }
            objects.Clear();
            Start();
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
