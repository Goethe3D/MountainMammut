using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Browser : MonoBehaviour {
	
	private PluginManager pluginManager;
	private PhotonView photonView;
	private NetworkManager networkManager;
	private List< GameObject > spawnedBrowsers = new List< GameObject >();
	
	public GameObject BrowserScreen;
	
	
	// Use this for initialization
	void Start () {
		pluginManager = GetComponent< PluginManagerInterface > ().getPluginManager ();
		photonView = GetComponent< PluginManagerInterface > ().getPhotonView ();
		networkManager = GetComponent< PluginManagerInterface > ().getNetworkManager ();
		
		Plugin me;
		me.keyword = "#BROWSER";
		me.function = processInput;
		
		pluginManager.registerPlugin (me);
		
	}
	
	[RPC]
	void spawnBrowserWindow( string url , Vector3 browserPosition )
	{
		Quaternion browserRotation = Quaternion.Euler( 90 , 180 , 0 );
		
		string fullUrl = "http://www." + url;

		GameObject BrowserObject = (GameObject) Instantiate( BrowserScreen, browserPosition , browserRotation );
		UWKWebView.AddToGameObject( BrowserObject , fullUrl);
		
		spawnedBrowsers.Add( BrowserObject );
		
	}
	
	[RPC]
	void killFirstBrowser()
	{
		if( spawnedBrowsers.Count == 0 )
		{
			return;
		}
		
		GameObject Browser = spawnedBrowsers[ 0 ];
		spawnedBrowsers.Remove( Browser );
		GameObject.DestroyImmediate( Browser );
	}

	[RPC]
	void killAllBrowsers()
	{
		if( spawnedBrowsers.Count == 0 )
		{
			return;
		}

		while( spawnedBrowsers.Count > 0 )
		{
			GameObject Browser = spawnedBrowsers[ 0 ];
			spawnedBrowsers.Remove( Browser );
			GameObject.DestroyImmediate( Browser );
		}
	}
	
	void processInput( string input )
	{
		if( input.ToUpperInvariant().StartsWith( "-KILLALL" ) )
		{
			photonView.RPC( "killAllBrowsers" , PhotonTargets.All );
			return;
		} else if( input.ToUpperInvariant().StartsWith( "-KILL" ) )
		{
			photonView.RPC( "killFirstBrowser" , PhotonTargets.All );
			return;
		}

		Vector3 browserPosition = networkManager.getPlayerPosition();
		photonView.RPC( "spawnBrowserWindow" , PhotonTargets.All , input , browserPosition );
	}
}
