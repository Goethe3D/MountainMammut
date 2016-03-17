using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldSync : MonoBehaviour {

	InputField inputField;
	PhotonView photonView;
	TextMesh textMesh;
	TextChanger textChanger;

	// Use this for initialization
	void Start () {
	
		photonView = GetComponent< PhotonView >();
		inputField = GetComponentInChildren< InputField >();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setTextMesh( TextMesh mesh )
	{
		textMesh = mesh;
	}

	public void setTextChanger( TextChanger changer )
	{
		textChanger = changer;
	}

	[RPC]
	void setInputFieldTextRPC( string text )
	{
		textMesh.text = text;
		textChanger.setEditing( false );
		Destroy( gameObject );
	}

	public void setInputFieldText( string text )
	{
		Debug.Log ( text );
		photonView.RPC( "setInputFieldTextRPC" , PhotonTargets.All , text );
	}
}
