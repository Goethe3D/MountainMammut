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
	fly1 flyScript;



	// Use this for initialization
	void Start () {

		textMesh = GetComponentInChildren< TextMesh >();
		editing = false;
		editCache = false;
		changeEdit = false;
		isMe = false;
		photonView = GetComponent< PhotonView >();
		flyScript = GetComponent< fly1 >();
	
	}

	public void setEditing( bool edit )
	{
		changeEdit = true;
		editCache = edit;
		editTime = Time.time;
		if( !edit )
		{
			flyScript.setTranslationEnabled( true );
		}
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
		if( !photonView.isMine )
		{
			return;
		}
		photonView.RPC( "changeMyTextRPC" , PhotonTargets.All , text );
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( Input.GetKeyUp(KeyCode.Return) && !editing && photonView.isMine )
		{
			editing = true;
			Camera camera = GetComponentInChildren< Camera >();
			//GameObject canvasObject = PhotonNetwork.Instantiate( "Canvas" , transform.position + 10 * transform.forward , transform.rotation , 0 );
			GameObject canvasObject = (GameObject)GameObject.Instantiate( Resources.Load( "Canvas" ) , transform.position + 10 * transform.forward , transform.rotation );

			InputFieldSync canvasSync = canvasObject.GetComponent< InputFieldSync >();
			//canvasSync.setTextMesh( textMesh );
			canvasSync.setTextChanger( this );


			UnityEngine.UI.InputField inputField = canvasObject.GetComponentInChildren< UnityEngine.UI.InputField >();
			inputField.Select();

			flyScript.setTranslationEnabled( false );

		}

		if( changeEdit && Time.time - editTime > 1.0f )
		{
			changeEdit = false;
			editing = editCache;
		}
	
	}
}
