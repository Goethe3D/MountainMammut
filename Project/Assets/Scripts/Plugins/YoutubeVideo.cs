using UnityEngine;
using System.Collections;

public class YoutubeVideo : MonoBehaviour {

	private PluginManager pluginManager;
	private PhotonView photonView;
	private NetworkManager networkManager;

	public GameObject videoScreen;


	// Use this for initialization
	void Start () {
		pluginManager = GetComponent< PluginManagerInterface > ().getPluginManager ();
		photonView = GetComponent< PluginManagerInterface > ().getPhotonView ();
		networkManager = GetComponent< PluginManagerInterface > ().getNetworkManager ();

		Plugin me;
		me.keyword = "#VIDEO";
		me.function = processInput;

		pluginManager.registerPlugin (me);
	
	}

	[RPC]
	void spawnVideoBrowser( string url )
	{
		Quaternion browserRotation = Quaternion.Euler( 90 , 180 , 0 );
		
		string fullUrl = "http://mstellmacher.de/unityprojectapi/vrvideo.php?url=" + url;
		
		Vector3 browserPosition = networkManager.getPlayerPosition();
		GameObject videoObject = (GameObject) Instantiate( videoScreen , browserPosition , browserRotation );
		UWKWebView.AddToGameObject( videoObject , fullUrl);
		
	}

	void processInput( string input )
	{
		photonView.RPC( "spawnVideoBrowser" , PhotonTargets.AllViaServer , input );
	}
}
