﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextChanger : Photon.MonoBehaviour {

	string[] textMeshComponentTags;
	KeyCode[] editButtons;
	TextMesh[] textMeshes;
	PhotonView photonView;
	int currentStringIndex;
	bool editing;
	bool changeEdit;
	bool editCache;
	float editTime;
	bool isMe;
	fly1 flyScript;
	private Queue< KeyValuePair< int , string > > eventBuffer = new Queue< KeyValuePair< int , string > >(10);

	const string overheadTextId = "TextBoxOverhead";
	const string faceTextId = "TextBoxFace";

	InputField currentInputField;
	int currentEditId;

	FaceDatabase m_faceDatabase;// = new FaceDatabase();

	// Use this for initialization
	void Start () {
		Debug.Log( "Start" );
		textMeshes = new TextMesh[ 2 ];
		textMeshComponentTags = new string[ 2 ];
		editButtons = new KeyCode[ 2 ];

		textMeshComponentTags[ 0 ] = overheadTextId;
		textMeshComponentTags[ 1 ] = faceTextId;

		editButtons[ 0 ] = KeyCode.Return;
		editButtons[ 1 ] = KeyCode.LeftControl;

		TextMesh[] textMeshesInChildren = GetComponentsInChildren< TextMesh >();

		m_faceDatabase = GetComponent< FaceDatabase >();

		int tagCount = 0;
		foreach( string textMeshComponentTag in textMeshComponentTags )
		{
			foreach( TextMesh tm in textMeshesInChildren )
			{
				if( tm.name == textMeshComponentTag )
				{
					textMeshes[ tagCount++ ] = tm;
					break;
				}
			}
		}

		//textMesh = GetComponentInChildren< TextMesh >();
		editing = false;
		editCache = false;
		changeEdit = false;
		isMe = false;
		photonView = GetComponent< PhotonView >();
		flyScript = GetComponent< fly1 >();

		if( photonView.isMine )
		{
			foreach( TextMesh tm in textMeshes )
			{
				if( tm.name == "TextBoxFace" )
				{
					MeshRenderer renderer = tm.transform.GetComponent< MeshRenderer >();
					renderer.enabled = false;
				}
			}
		}

		processBufferedEvents();
	
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
	public void changeMyTextRPC( int textMeshID , string text )
	{
		//Debug.Log( textMeshes.Length );
		//Debug.Log( textMeshID );
		if( textMeshes == null )
		{
			eventBuffer.Enqueue( new KeyValuePair<int, string>( textMeshID , text ) );
			return;
		}

		SmartTextMesh smartTextMesh = textMeshes[ textMeshID ].transform.GetComponent< SmartTextMesh >();
		if( smartTextMesh == null )
		{
			textMeshes[ textMeshID ].text = text;
		}
		else
		{
			smartTextMesh.UnwrappedText = text;
			smartTextMesh.NeedsLayout = true;
		}
	}

	public void setText( int textMeshId , string text )
	{
		if( !photonView.isMine )
		{
			return;
		}
		photonView.RPC( "changeMyTextRPC" , PhotonTargets.AllBuffered , textMeshId , text );
	}
	
	// Update is called once per frame
	void Update () {

		int pressedKey = -1;

		int keyArrayId = 0;
		foreach( KeyCode kc in editButtons )
		{
			if( Input.GetKeyUp( kc ) )
			{
				pressedKey = keyArrayId;
				break;
			}
			keyArrayId++;
		}
		
		if ( pressedKey >= 0 && !editing && photonView.isMine )
		{
			editing = true;
			Camera camera = GetComponentInChildren< Camera >();
			//GameObject canvasObject = PhotonNetwork.Instantiate( "Canvas" , transform.position + 10 * transform.forward , transform.rotation , 0 );
			GameObject canvasObject = (GameObject)GameObject.Instantiate( Resources.Load( "Canvas" ) , camera.transform.position + 10 * camera.transform.forward , camera.transform.rotation );

			InputFieldSync canvasSync = canvasObject.GetComponent< InputFieldSync >();
			//canvasSync.setTextMesh( textMesh );
			canvasSync.setTextChanger( this );
			canvasSync.setTextMeshId( pressedKey );


			UnityEngine.UI.InputField inputField = canvasObject.GetComponentInChildren< UnityEngine.UI.InputField >();
			inputField.text = textMeshes[ pressedKey ].text;
			if( textMeshComponentTags[ pressedKey ] == overheadTextId )
			{
				inputField.characterLimit = 140;
			}
			else if( textMeshComponentTags[ pressedKey ] == faceTextId )
			{
				inputField.characterLimit = 6;
			}
			inputField.lineType = InputField.LineType.MultiLineSubmit;
//			RectTransform inputFieldTransform = (RectTransform) inputField.transform;
//			inputFieldTransform.sizeDelta = new Vector2( 40 , 40 );
			inputField.Select();

			flyScript.setTranslationEnabled( false );

			currentInputField = inputField;
			currentEditId = pressedKey;

		}

		int faceId = FaceDatabase.numberButtonPressed();
		if( editing && textMeshComponentTags[ currentEditId ] == faceTextId && faceId >= 0 )
		{
			if( Input.GetKey( KeyCode.LeftAlt ) )
			{
				if( Input.GetKey( KeyCode.LeftShift ) )
				{
					m_faceDatabase.setFace( faceId , currentInputField.text );
				}
				else
				{
					currentInputField.text = m_faceDatabase.getFace( faceId );
				}
			}
		}

		if( changeEdit && Time.time - editTime > 1.0f )
		{
			changeEdit = false;
			editing = editCache;
		}
	
	}


	public void processBufferedEvents()
	{
		while( eventBuffer.Count > 0 )
		{
			KeyValuePair< int , string > bufferedEvent = eventBuffer.Dequeue();
			changeMyTextRPC( bufferedEvent.Key , bufferedEvent.Value );
		}
	}
}
