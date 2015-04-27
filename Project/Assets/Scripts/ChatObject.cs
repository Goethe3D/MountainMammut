using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatObject : MonoBehaviour {

	public GameObject messageBox;

	private List< GameObject > chatroomMessages = new List< GameObject >();

	private PhotonView photonView;

	// Use this for initialization
	void Awake () {
		photonView = GetComponent< PhotonView > ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void newMessage( string message )
	{
		photonView.RPC( "newMessage_RPC" , PhotonTargets.All , message );
	}

	[RPC]
	public void newMessage_RPC( string message )
	{
		foreach( GameObject previousMessage in chatroomMessages )
		{
			Vector3 previousMessagePosition = previousMessage.transform.position;
			previousMessagePosition.y = previousMessagePosition.y + 2;
			previousMessage.transform.position = previousMessagePosition;
		}

		GameObject chatBoxObject = ( GameObject ) Instantiate( messageBox , transform.position , transform.rotation );
		chatroomMessages.Add( chatBoxObject );

		DiegeticChatManager manager = chatBoxObject.GetComponentInChildren< DiegeticChatManager >();
		
		manager.AddMessage_RPC( message );

	}
}
