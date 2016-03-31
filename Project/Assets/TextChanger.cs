using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextChanger : MonoBehaviour {

	TextMesh textMesh;
	List< string > predefineStringList = new List< string >();
	PhotonView photonView;
	int currentStringIndex;
	bool editing;
	bool changeEdit;
	bool editCache;
	float editTime;
	bool isMe;



	// Use this for initialization
	void Start () {

		textMesh = GetComponentInChildren< TextMesh >();
		editing = false;
		editCache = false;
		changeEdit = false;
		isMe = false;
		photonView = GetComponent< PhotonView >();
	
	}

	public void setEditing( bool edit )
	{
		changeEdit = true;
		editCache = edit;
		editTime = Time.time;
	}

	public void setIsMe( bool me )
	{
		isMe = me;
	}

	[RPC]
	public void changeMyTextRPC( string text )
	{
		textMesh.text = text;
	}

	public void setText( string text )
	{
		photonView.RPC( "changeMyTextRPC" , PhotonTargets.All , text );
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( Input.GetKeyUp(KeyCode.Return) && !editing && GetComponent< PhotonView >().isMine )
		{
			editing = true;
			Camera camera = GetComponentInChildren< Camera >();
			//GameObject canvasObject = PhotonNetwork.Instantiate( "Canvas" , transform.position + 10 * transform.forward , transform.rotation , 0 );
			GameObject canvasObject = (GameObject)GameObject.Instantiate( Resources.Load( "Canvas" ) , transform.position + 10 * transform.forward , transform.rotation );

			InputFieldSync canvasSync = canvasObject.GetComponent< InputFieldSync >();
			//canvasSync.setTextMesh( textMesh );
			canvasSync.setTextChanger( this );

		}

		if( changeEdit && Time.time - editTime > 1.0f )
		{
			changeEdit = false;
			editing = editCache;
		}
	
	}
}
