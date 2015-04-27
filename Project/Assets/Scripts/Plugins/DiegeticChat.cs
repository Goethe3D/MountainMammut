using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DiegeticChat : MonoBehaviour {

	public GameObject chatbox;
	private List< GameObject > chatrooms = new List< GameObject >();

	private PluginManager pluginManager;
	private PhotonView photonView;
	private NetworkManager networkManager;
	
	private Vector3 chatboxTranslationVector = new Vector3( 0 , 0 , 5 );

	private int defaultChatroomId = 0;
	private int chatroomPhotonId = 5;
	
	// Use this for initialization
	void Start () {
		pluginManager = GetComponent< PluginManagerInterface > ().getPluginManager ();
		photonView = GetComponent< PluginManagerInterface > ().getPhotonView ();
		networkManager = GetComponent< PluginManagerInterface > ().getNetworkManager ();
		
		Plugin me;
		me.keyword = "#CHAT";
		me.function = processInput;
		
		pluginManager.registerPlugin (me);
		
	}


	[RPC]
	void createChatBox( Vector3 position , Quaternion rotation )
	{
		position.y -= 7;
		GameObject chatBoxObject = ( GameObject ) Instantiate( chatbox , position , rotation );
		chatrooms.Add( chatBoxObject );

		PhotonView[] nViews = chatBoxObject.GetComponents<PhotonView>();
		nViews[0].viewID = chatroomPhotonId++;
	}


	void processInput( string input )
	{
		if( input.ToUpperInvariant().StartsWith( "-CREATE" ) )
		{
			Vector3 myPosition = networkManager.getPlayerPosition();
			Quaternion myRotation = networkManager.getPlayerRotation();
			
			Vector3 chatBoxPosition = myPosition + myRotation * chatboxTranslationVector;

			photonView.RPC( "createChatBox" , PhotonTargets.All , chatBoxPosition , myRotation );

			return;
		}

		if( chatrooms.Count == 0 )
		{
			return;
		}

		if( defaultChatroomId >= chatrooms.Count )
		{
			defaultChatroomId = 0;
		}

		int chatroomId = defaultChatroomId;

		string[] splitStrings = input.Split( new Char[]{ ' ' } );
		
		int messageStartIndex = 0;
		bool setDefaultValues = false;
		foreach( string word in splitStrings )
		{
			if( !word.ToUpperInvariant().StartsWith( "-" ) )
			{
				break;
			}
			
			if( word.ToUpperInvariant() == "-SET" )
			{
				setDefaultValues = true;
			}
			
			if( word.ToUpperInvariant().Contains( "ROOM=" ) 
			   || word.ToUpperInvariant().Contains( "R=" ) )
			{
				int insertedId = Convert.ToInt32( word.Substring( word.IndexOf( "=" ) + 1 ) );
				if( insertedId < chatrooms.Count )
				{
					chatroomId = insertedId;
					if( setDefaultValues )
					{
						defaultChatroomId = chatroomId;
					}
				}
			}
			
			messageStartIndex += word.Length + 1;
		}
		
		string chatMessage = input.Substring( messageStartIndex );

		ChatObject selectedChatObject = chatrooms[ chatroomId ].GetComponent< ChatObject >();

		selectedChatObject.newMessage( PhotonNetwork.player.name + ": " + chatMessage );


//		DiegeticChatManager manager = chatrooms[ chatroomId ].GetComponentInChildren< DiegeticChatManager >();
//
//		manager.AddChatMessage( chatMessage );

		//chatrooms[ chatroomId ].AddMessage( chatMessage );

	}
}
