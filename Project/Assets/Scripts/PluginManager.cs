using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ProcessFunction( string input );

public struct Plugin
{
	public string keyword;
	public ProcessFunction function;
}

public class PluginManager : MonoBehaviour {

	private List< Plugin > plugins = new List< Plugin >();
	private NetworkManager networkManager;
	public GameObject networkManagerObject;

	// Use this for initialization
	void Start () {
		networkManager = networkManagerObject.GetComponent< NetworkManager > ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void processInput( string input )
	{
		foreach (Plugin plugin in plugins) 
		{
			if( input.ToUpperInvariant().StartsWith( plugin.keyword ) )
			{
				plugin.function( input.Substring( plugin.keyword.Length + 1 ) );
				return;
			}
		}
		networkManager.AddChatMessage (input);
	}

	public void registerPlugin( Plugin plugin )
	{
		plugins.Add( plugin );
	}

}
