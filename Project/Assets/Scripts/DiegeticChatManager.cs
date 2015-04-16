using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DiegeticChatManager : MonoBehaviour {
	
//	[SerializeField] Text connectionText;
//	[SerializeField] Transform[] spawnPoints;
//	[SerializeField] Camera sceneCamera;
//
//	[SerializeField] GameObject serverWindow;
//	[SerializeField] InputField username;
//	[SerializeField] InputField roomName;
//	[SerializeField] InputField roomList;
	[SerializeField] InputField ChatInput;
	
//	GameObject player;
	Queue<string> messages;
	const int messageCount = 6;
	// to keep it local and cache it
	PhotonView photonView;
	
	void Start () {

		photonView = GetComponent<PhotonView> ();
		messages = new Queue<string> (messageCount);
		
//		PhotonNetwork.logLevel = PhotonLogLevel.Full;
//		PhotonNetwork.ConnectUsingSettings ("0.2");
//		StartCoroutine ("UpdateConnectionString");


		AddMessage_RPC ("Bla");
		AddMessage_RPC ("Bla");
		AddMessage_RPC ("Bla");
		AddMessage_RPC ("Bla");
		AddMessage_RPC ("Bla");
	}
//	
//	IEnumerator UpdateConnectionString () 
//	{
//		while(true)
//		{
//			connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
//			yield return null;
//		}
//	}
//	
//	void OnJoinedLobby()
//	{
//		serverWindow.SetActive (true);
//	}
//
//	void OnReceivedRoomListUpdate()
//	{
//		roomList.text = "";
//		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
//		foreach(RoomInfo room in rooms)
//			roomList.text += room.name + "\n";
//	}
//
//	public void JoinRoom()
//	{
//		PhotonNetwork.player.name = username.text;
//		RoomOptions roomOptions = new RoomOptions(){ isVisible = true, maxPlayers = 10 };
//		PhotonNetwork.JoinOrCreateRoom (roomName.text, roomOptions, TypedLobby.Default);
//	}
//	
//	void OnJoinedRoom()
//	{
//		serverWindow.SetActive (false);
//		StopCoroutine ("UpdateConnectionString");
//		connectionText.text = "";
//		StartSpawnProcess (0f);
//	}
//	
//	void StartSpawnProcess (float respawnTime)
//	{
//		sceneCamera.enabled = true;
//		StartCoroutine ("SpawnPlayer", respawnTime);
//	}
//	
//	IEnumerator SpawnPlayer(float respawnTime)
//	{
//		yield return new WaitForSeconds(respawnTime);
//		
//		int index = Random.Range (0, spawnPoints.Length);
//		player = PhotonNetwork.Instantiate ("FPSPlayer", 
//		                                    spawnPoints [index].position,
//		                                    spawnPoints [index].rotation,
//		                                    0);
//
////		player.GetComponent<PlayerNetworkMover> ().RespawnMe += StartSpawnProcess;
//		sceneCamera.enabled = false;
//
//		AddMessage ("Spawned player: " + PhotonNetwork.player.name);
//	}
//
//	public void AddMessage(string message)
//	{
//		photonView.RPC ("AddMessage_RPC", PhotonTargets.All, message);
//	}
//
	public void AddChatMessage(string message)
	{
		photonView.RPC ("AddMessage_RPC", PhotonTargets.All, PhotonNetwork.player.name + ": " + message);
	}
//
//	public void AddLocalMessage(string message)
//	{
//		AddMessage_RPC( "[Local]: " + message);
//	}
//
//	public Vector3 getPlayerPosition()
//	{
//		return player.transform.position;
//	}
//
//	public Quaternion getPlayerRotation()
//	{
//		return player.transform.rotation;
//	}

	[RPC]
	void AddMessage_RPC(string message)
	{
		messages.Enqueue (message);
		if(messages.Count > messageCount)
			messages.Dequeue();
		
		ChatInput.text = "";
		foreach(string m in messages)
			ChatInput.text += m + "\n";
	}

}
