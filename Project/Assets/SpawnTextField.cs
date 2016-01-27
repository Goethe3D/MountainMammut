using UnityEngine;
using System.Collections;

public class SpawnTextField : MonoBehaviour {

	public GameObject canvas;
	public GameObject networkManagerFlyObject;

	private NetworkManagerFly networkManagerFly;

	private PhotonView photonView;

	private int chatroomPhotonId = 5;

	// Use this for initialization
	void Start () {

		photonView = GetComponent< PhotonView >();
		networkManagerFly = networkManagerFlyObject.GetComponent< NetworkManagerFly >();
	
	}
	
	// Update is called once per frame
	void Update () {

		if ( Input.GetKeyUp(KeyCode.Return) ) 
		{
			int photonId = PhotonNetwork.AllocateSceneViewID();
			photonView.RPC( "spawnTextRPC" , PhotonTargets.All , networkManagerFly.getPlayerPosition() , networkManagerFly.getPlayerRotation() , photonId );
		}
	
	}


	[RPC]
	void spawnTextRPC( Vector3 position , Quaternion rotation , int photonId ) {

//		Vector3 myPosition = transform.position;
//		Quaternion myRotation = transform.rotation;

		Debug.Log ( "Spawning text inside RPC" );
		
		Vector3 textPosition = position + rotation * new Vector3( 0 , 0 , 10 );

		GameObject chatBoxObject = ( GameObject ) Instantiate( canvas , textPosition , rotation );

		PhotonView[] nViews = chatBoxObject.GetComponentsInChildren<PhotonView>();
		nViews[0].viewID = photonId;

	}
}
