using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//using StreamObject = System.Collections.Generic.KeyValuePair< System.DateTime , float >;
//using StreamObject = StreamObjectStruct< double , float >;

using StreamObject = StreamObjectStruct;

public class HeadColorSync : MonoBehaviour {

	private float m_fHueSeed = -1;

	private const float goldenAngle = 137.5077640500378546463487f / 360;

	public float getHueSeed()
	{
		return m_fHueSeed;
	}

	void OnJoinedRoom()
	{
		Debug.Log ( "Hier bin ich" );
		if( PhotonNetwork.room.playerCount == 1 )
		{
			m_fHueSeed = UnityEngine.Random.Range( 0f , 1f );
			Debug.Log( "Ich bin alleine" );
			Debug.Log ( m_fHueSeed );

			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
			customProperties.Add( "hs" , m_fHueSeed );
			PhotonNetwork.room.SetCustomProperties( customProperties );
		}
		else
		{
			Debug.Log ( "Wir sind mehrere" );

			ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.room.customProperties;

			float hueSeed = (float)roomProperties[ "hs" ];
			m_fHueSeed = ( hueSeed + goldenAngle ) % 1;

			roomProperties[ "hs" ] = m_fHueSeed;

			PhotonNetwork.room.SetCustomProperties( roomProperties );

		}
	}

	public bool seedInitialized()
	{
		return m_fHueSeed >= 0;
	}
}
