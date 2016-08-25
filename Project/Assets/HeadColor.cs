using UnityEngine;
using System.Collections;

public class HeadColor : MonoBehaviour {


	PhotonView photonView;
	HeadColorSync headColorSync;

	MeshRenderer meshRenderer;
	TextMesh overheadTextMesh;

	// Use this for initialization
	void Awake () {

		Transform[] childrenGameObjects = GetComponentsInChildren< Transform >();

		Transform headObject = null;
		Transform textBoxOverheadObject = null;
		foreach( Transform childObject in childrenGameObjects )
		{
			if( childObject.name == "head" )
			{
				headObject = childObject;
			}

			if( childObject.name == "TextBoxOverhead" )
			{
				textBoxOverheadObject = childObject;
			}
		}

		if( headObject == null )
		{
//			Debug.Log( "Couldn't find my head" );
			return;
		}

		if( textBoxOverheadObject == null )
		{
//			Debug.Log( "Couldn't find my overhead text" );
			return;
		}

//		Debug.Log( "Found my head" );


		meshRenderer = headObject.GetComponentInChildren< MeshRenderer >();
		overheadTextMesh = textBoxOverheadObject.GetComponent< TextMesh >();

		if( meshRenderer == null )
		{
//			Debug.Log( "Didn't find my renderer" );
			return;
		}

		if( overheadTextMesh == null )
		{
//			Debug.Log( "Couldn't find my overhead text mesh" );
			return;
		}

//		Debug.Log( "Found my head renerer" );


		GameObject headColorSynchronizer = GameObject.Find( "HeadColorSynchronizer" );
		headColorSync = headColorSynchronizer.GetComponent< HeadColorSync >();
	
	}

	void Start()
	{
		photonView = GetComponent< PhotonView >();


		if( !photonView.isMine )
		{
			return;
		}

		StartCoroutine( retrieveAndSetHeadColor( 2 ) );
	}

	IEnumerator retrieveAndSetHeadColor( float secondsTillExecution )
	{
		yield return new WaitForSeconds( secondsTillExecution );

		if( headColorSync.seedInitialized() )
		{
			//Color myColor = Color.HSVToRGB( headColorSync.getHueSeed() , 1 , 1 );
			photonView.RPC( "setHeadColor" , PhotonTargets.AllBuffered , headColorSync.getHueSeed() );
			return true;
		}

		StartCoroutine( retrieveAndSetHeadColor( 2 ) );
	}

	[RPC]
	void setHeadColor( float colorSeed )
	{
		Debug.Log( "Set Head Color" );
		meshRenderer.material.color = Color.HSVToRGB( colorSeed , 1 , 1 );
		overheadTextMesh.color = Color.HSVToRGB( colorSeed , 1 , 1 );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
