using UnityEngine;
using System.Collections;

public class YoutubeVideo : MonoBehaviour {

	private PluginManager pluginManager;


	// Use this for initialization
	void Start () {
		pluginManager = GetComponent< PluginManagerInterface > ().getPluginManager ();

		Plugin me;
		me.keyword = "#VIDEO";
		me.function = processInput;

		pluginManager.registerPlugin (me);
	
	}

	void processInput( string input )
	{
	}
}
