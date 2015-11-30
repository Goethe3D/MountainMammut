using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkManagerFly : MonoBehaviour {
	
//	[SerializeField] Text connectionText;
//	[SerializeField] Transform[] spawnPoints;
//	[SerializeField] Camera sceneCamera;
//
//	[SerializeField] GameObject serverWindow;
//	[SerializeField] InputField username;
//	[SerializeField] InputField roomName;
//	[SerializeField] InputField roomList;
//	[SerializeField] InputField messageWindow;

	string roomName = "Flying room";
	string playerName = "Flyer";
	
	GameObject player;
	Queue<string> messages;
	const int messageCount = 6;
	// to keep it local and cache it
	PhotonView photonView;
	
	void Start () {

		photonView = GetComponent<PhotonView> ();
		messages = new Queue<string> (messageCount);
		
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("0.2");
		StartCoroutine ("UpdateConnectionString");

		
	}
	
	IEnumerator UpdateConnectionString () 
	{
		while(true)
		{
			//connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
			yield return null;
		}
	}
	
	void OnJoinedLobby()
	{
		//serverWindow.SetActive (true);
	}

	void OnReceivedRoomListUpdate()
	{
//		roomList.text = "";
//		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
//		foreach(RoomInfo room in rooms)
//			roomList.text += room.name + "\n";
		JoinRoom ();
	}

	public void JoinRoom()
	{
		PhotonNetwork.player.name = playerName;
		RoomOptions roomOptions = new RoomOptions(){ isVisible = true, maxPlayers = 10 };
		PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, TypedLobby.Default);
	}
	
	void OnJoinedRoom()
	{
//		serverWindow.SetActive (false);
		StopCoroutine ("UpdateConnectionString");
//		connectionText.text = "";
		StartSpawnProcess (0f);
	}
	
	void StartSpawnProcess (float respawnTime)
	{
		//sceneCamera.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}
	
	IEnumerator SpawnPlayer(float respawnTime)
	{
		yield return new WaitForSeconds(respawnTime);
		
		//int index = Random.Range (0, spawnPoints.Length);
		player = PhotonNetwork.Instantiate ("flyingH",
		                                    new Vector3( 0 , 0 , 0 ),
		                                    new Quaternion(),
		                                    0);

//		player.GetComponent<PlayerNetworkMover> ().RespawnMe += StartSpawnProcess;
//		sceneCamera.enabled = false;

		AddMessage ("Spawned player: " + PhotonNetwork.player.name);
	}

	public void AddMessage(string message)
	{
		photonView.RPC ("AddMessage_RPC", PhotonTargets.All, message);
	}

	public void AddChatMessage(string message)
	{
		photonView.RPC ("AddMessage_RPC", PhotonTargets.All, PhotonNetwork.player.name + ": " + message);
	}

	public void AddLocalMessage(string message)
	{
		AddMessage_RPC( "[Local]: " + message);
	}

	public Vector3 getPlayerPosition()
	{
		return player.transform.position;
	}

	public Quaternion getPlayerRotation()
	{
		return player.transform.rotation;
	}

	[RPC]
	void AddMessage_RPC(string message)
	{
		messages.Enqueue (message);
		if(messages.Count > messageCount)
			messages.Dequeue();
		
//		messageWindow.text = "";
//		foreach(string m in messages)
//			messageWindow.text += m + "\n";
	}

}
