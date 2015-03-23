using UnityEngine;
using System.Collections;
using System.Collections.Generic;


struct Song
{
	public GameObject spawnedSong;
	public string query;
}

public class YoutubeMusic : MonoBehaviour {

	private PluginManager pluginManager;
	private PhotonView photonView;
	private NetworkManager networkManager;

	private List< Song > spawnedSongs = new List< Song >();
	private Queue< string > queryStrings = new Queue< string >();

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


		queryStrings.Clear();
		
	}

	void OnGUI()
	{
//		Debug.Log( spawnedSongs.Count );
		for( int i = 0 ; i < spawnedSongs.Count ; ++i )
		{
			GUI.Box( new Rect( 0.8f * ( float ) Screen.width , 0.1f * ( float ) Screen.height + ( float ) i * 0.06f * Screen.height , 0.15f * ( float ) Screen.width , 0.05f * ( float ) Screen.height ) , spawnedSongs[ i ].query );   
		}
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
		Song newSong;
		newSong.spawnedSong = (GameObject) Instantiate( emptyObject , browserPosition , Quaternion.identity );
		UWKWebView.AddToGameObject( newSong.spawnedSong , url);

		if( queryStrings.Count >= 1 )
		{
			newSong.query = queryStrings.Dequeue();
		}
		else
		{
			newSong.query = "Song";
		}

		spawnedSongs.Add( newSong );
		
		
	}

	[RPC]
	void killFirstSong()
	{
		if( spawnedSongs.Count == 0 )
		{
			return;
		}

		Song song = spawnedSongs[ 0 ];
		spawnedSongs.Remove( song );
		GameObject.DestroyImmediate( song.spawnedSong );
	}

	[RPC]
	void killAllSongs()
	{
		if( spawnedSongs.Count == 0 )
		{
			return;
		}

		while( spawnedSongs.Count > 0 )
		{
			Song song = spawnedSongs[ 0 ];
			spawnedSongs.Remove( song );
			GameObject.DestroyImmediate( song.spawnedSong );
		}
	}

	[RPC]
	void addToQueryQueue( string query )
	{
		queryStrings.Enqueue( query );
	}


	void processInput( string input )
	{
		if( input.ToUpperInvariant().StartsWith( "-KILLALL" ) )
		{
			photonView.RPC( "killAllSongs" , PhotonTargets.All );
			return;
		}
		else if( input.ToUpperInvariant().StartsWith( "-KILL" ) )
		{
			photonView.RPC( "killFirstSong" , PhotonTargets.All );
			return;
		}

		photonView.RPC( "addToQueryQueue" , PhotonTargets.All , input );
		view.SendJSMessage( "YoutubeMusic" , input );
		
		StartCoroutine( initializeQuery( 5 ) );
	}
}
