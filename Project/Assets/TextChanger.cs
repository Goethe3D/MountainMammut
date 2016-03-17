using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextChanger : MonoBehaviour {

	TextMesh textMesh;
	List< string > predefineStringList = new List< string >();
	int currentStringIndex;
	bool editing;
	bool changeEdit;
	bool editCache;
	float editTime;



	// Use this for initialization
	void Start () {

		textMesh = GetComponent< TextMesh >();
		editing = false;
		editCache = false;
		changeEdit = false;
	
	}

	public void setEditing( bool edit )
	{
		changeEdit = true;
		editCache = edit;
		editTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( Input.GetKeyUp(KeyCode.Return) && !editing )
		{
			editing = true;
			Camera camera = transform.parent.gameObject.GetComponentInChildren< Camera >();
			GameObject canvasObject = PhotonNetwork.Instantiate( "Canvas" , transform.parent.position + 10 * transform.parent.forward , Quaternion.identity , 0 );

			InputFieldSync canvasSync = canvasObject.GetComponent< InputFieldSync >();
			canvasSync.setTextMesh( textMesh );
			canvasSync.setTextChanger( this );

		}

		if( changeEdit && Time.time - editTime > 1.0f )
		{
			changeEdit = false;
			editing = editCache;
		}
	
	}
}
