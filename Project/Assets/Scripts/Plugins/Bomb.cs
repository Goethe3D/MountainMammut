using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour {
	
	private PluginManager pluginManager;
	private PhotonView photonView;
	private NetworkManager networkManager;

	public GameObject textPrefab;

	private Vector3 textTranslationVector = new Vector3( 0 , 0 , 5 );
	
	
	// Use this for initialization
	void Start () {
		pluginManager = GetComponent< PluginManagerInterface > ().getPluginManager ();
		photonView = GetComponent< PluginManagerInterface > ().getPhotonView ();
		networkManager = GetComponent< PluginManagerInterface > ().getNetworkManager ();
		
		Plugin me;
		me.keyword = "#BOMB";
		me.function = processInput;
		
		pluginManager.registerPlugin (me);
		
	}


	[RPC]
	void writeText( string chatMessage , Vector3 textPosition , Quaternion textRotation )
	{
		GameObject instantiatedTextObject = (GameObject) Instantiate( textPrefab , textPosition , textRotation  );
		TextMesh instantiatedText = instantiatedTextObject.GetComponentsInChildren< TextMesh >()[0];
		instantiatedText.text = chatMessage;
	}

	
	void processInput( string input )
	{
		Vector3 myPosition = networkManager.getPlayerPosition();
		Quaternion myRotation = networkManager.getPlayerRotation();

		Vector3 textPosition = myPosition + myRotation * textTranslationVector;

		photonView.RPC( "writeText" , PhotonTargets.All , input , textPosition , myRotation );
	}
}