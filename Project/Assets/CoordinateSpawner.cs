using UnityEngine;
using System.Collections;

public class CoordinateSpawner : MonoBehaviour {



	// Use this for initialization
	void Start () {

		GameObject MutterObject = new GameObject ();

		for (int x = 0; x < 10; x++) 
		{
			for (int y = 0; y < 10; y++) 
			{
				for (int z = 0; z < 10; z++) 
				{
					
					GameObject CoordinateObject = new GameObject ();
					CoordinateObject.transform.parent = MutterObject.transform;
					TextMesh coordinate = CoordinateObject.AddComponent< TextMesh > (); 
					coordinate.fontSize = 500;
					coordinate.text = x.ToString() + "-" + y.ToString() + "-" + z.ToString();

					CoordinateObject.transform.position = new Vector3 (x * 100, y * 100, z * 100);
					CoordinateObject.transform.localScale = new Vector3 (0.05F, 0.05F, 0.05F);
				}
			
			 }
		 }

	}
	
	// Update is called once per frame
	void Update () {

	}
}
