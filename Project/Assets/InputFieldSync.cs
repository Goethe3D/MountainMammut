using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldSync : MonoBehaviour {

	InputField inputField;
	PhotonView photonView;

	// Use this for initialization
	void Start () {
	
		photonView = GetComponent< PhotonView >();
		inputField = GetComponent< InputField >();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void setInputFieldTextRPC( string text )
	{
		inputField.text = text;
	}

	public void setInputFieldText( string text )
	{
		photonView.RPC( "setInputFieldTextRPC" , PhotonTargets.All , text );
	}
}
