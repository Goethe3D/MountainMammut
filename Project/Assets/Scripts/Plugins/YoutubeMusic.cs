using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YoutubeMusic : MonoBehaviour {

	private PluginManager pluginManager;
	private PhotonView photonView;
	private NetworkManager networkManager;

	private List< GameObject > spawnedSongs = new List< GameObject >();

	UWKWebView view;
	GameObject viewObject;
	GameObject emptyObject;

	// Use this for initialization
	void Start () {
		pluginManager = GetComponent< PluginManagerInterface > ().getPluginManager ();
		photonView = GetComponent< PluginManagerInterface > ().getPhotonView ();
		networkManager = GetComponent< PluginManagerInterface > ().getNetworkManager ();

		Plugin me;
		me.keyword = "#MUSIC";
		me.function = processInput;
		
		pluginManager.registerPlugin (me);

		StartCoroutine (initializeQuery (0));

		emptyObject = new GameObject ();
	}


	IEnumerator initializeQuery( float secondsTillExecution )
	{
		yield return new WaitForSeconds( secondsTillExecution );
		
		GameObject.DestroyImmediate( viewObject );
		
		viewObject = new GameObject();
		
		UWKWebView.AddToGameObject( viewObject , "http://mstellmacher.de/unityprojectapi/JavascriptExample.html" );
		
		view = viewObject.GetComponent<UWKWebView>();
		view.JSMessageReceived += onJSMessage;
		
		
	}

	void onJSMessage(UWKWebView view, string message, string json, Dictionary<string, object> values)
	{

		//currentChatMessage = json;
		if (message == "YoutubeMusic")
		{
			//führt funktion aus, die dann für alle user ausgeführt werden kann
			photonView.RPC( "spawnBrowser" , PhotonTargets.All , json );
			view.Reload();
		}
		
		
	}

	[RPC]
	void spawnBrowser( string url )
	{

		url = url.Substring(1);
		url = url.Remove( url.Length - 1 );

		
		Vector3 browserPosition = transform.position;
		GameObject musicObject = (GameObject) Instantiate( emptyObject , browserPosition , Quaternion.identity );
		UWKWebView.AddToGameObject( musicObject , url);

		spawnedSongs.Add( musicObject );
		
		
	}

	[RPC]
	void killFirstSong()
	{
		if( spawnedSongs.Count == 0 )
		{
			return;
		}

		GameObject song = spawnedSongs[ 0 ];
		spawnedSongs.Remove( song );
		GameObject.DestroyImmediate( song );
	}


	void processInput( string input )
	{
		if( input.ToUpperInvariant().StartsWith( "-KILL" ) )
		{
			photonView.RPC( "killFirstSong" , PhotonTargets.All );
			return;
		}

		view.SendJSMessage( "YoutubeMusic" , input );
		
		StartCoroutine( initializeQuery( 5 ) );
	}
}
