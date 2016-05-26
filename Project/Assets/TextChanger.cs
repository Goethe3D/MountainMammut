using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextChanger : MonoBehaviour {

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



	// Use this for initialization
	void Start () {
		textMeshes = new TextMesh[ 2 ];
		textMeshComponentTags = new string[ 2 ];
		editButtons = new KeyCode[ 2 ];

		textMeshComponentTags[ 0 ] = "TextBoxOverhead";
		textMeshComponentTags[ 1 ] = "TextBoxFace";

		editButtons[ 0 ] = KeyCode.Return;
		editButtons[ 1 ] = KeyCode.Keypad5;

		TextMesh[] textMeshesInChildren = GetComponentsInChildren< TextMesh >();

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
		textMeshes[ textMeshID ].text = text;
	}

	public void setText( int textMeshId , string text )
	{
		if( !photonView.isMine )
		{
			return;
		}
		photonView.RPC( "changeMyTextRPC" , PhotonTargets.All , textMeshId , text );
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
			GameObject canvasObject = (GameObject)GameObject.Instantiate( Resources.Load( "Canvas" ) , transform.position + 10 * transform.forward , transform.rotation );

			InputFieldSync canvasSync = canvasObject.GetComponent< InputFieldSync >();
			//canvasSync.setTextMesh( textMesh );
			canvasSync.setTextChanger( this );
			canvasSync.setTextMeshId( pressedKey );


			UnityEngine.UI.InputField inputField = canvasObject.GetComponentInChildren< UnityEngine.UI.InputField >();
			inputField.text = textMeshes[ pressedKey ].text;
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
