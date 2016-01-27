using UnityEngine;
using System.Collections;

public class InputFieldSyncRelay : MonoBehaviour {

	InputFieldSync syncObject;

	// Use this for initialization
	void Start () {

		syncObject = GetComponentInParent< InputFieldSync >();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void relayTextToRPC( string text )
	{
		syncObject.setInputFieldText( text );
	}
}
