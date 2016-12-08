using UnityEngine;
using System.Collections;

public class ApplicationShortcuts : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetKeyUp(KeyCode.Q) && Input.GetKey(KeyCode.LeftAlt) )
		{
			Application.Quit();
		}
	
	}
}
