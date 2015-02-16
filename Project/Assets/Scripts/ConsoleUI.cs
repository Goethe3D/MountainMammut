using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConsoleUI : MonoBehaviour {

	[SerializeField] InputField console;
	public GameObject pluginManagerObject;
	private PluginManager pluginManager;
	

	// Use this for initialization
	void Start () {

		pluginManager = pluginManagerObject.GetComponent< PluginManager > ();
		console.text = "LOL";
	
	}
	
	// Update is called once per frame
	void Update () {

		if ( Input.GetKeyUp(KeyCode.Return) ) 
		{
			pluginManager.processInput( console.text );
			console.text = "";
		}
	
	}
}
