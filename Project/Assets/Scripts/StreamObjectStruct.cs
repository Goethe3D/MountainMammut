using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


//[Serializable]
//public class StreamObjectStruct< T , P >
public class StreamObjectStruct
{
	//public StreamObjectStruct( T first , P second )
	public StreamObjectStruct( int first , float second )
	{
		this.first = first;
		this.second = second;
	}

	public int first {get;set;}
	public float second {get;set;}

	public static byte[] SerializeStreamObject(object customobject)
	{
		Debug.Log( "Custom Serialize" );
		StreamObjectStruct so = (StreamObjectStruct)customobject;

		byte[] doubleBytes = BitConverter.GetBytes( so.first );
		byte[] floatBytes = BitConverter.GetBytes( so.second );

		byte[] bytes = new byte[ doubleBytes.Length + floatBytes.Length ];

		System.Buffer.BlockCopy( doubleBytes , 0 , bytes , 0 , doubleBytes.Length );
		System.Buffer.BlockCopy( floatBytes , 0 , bytes , doubleBytes.Length , floatBytes.Length );

		return bytes;
	}


	public static object DeserializeStreamObject(byte[] bytes)
	{
		Debug.Log( "Custom deserialize" );
		StreamObjectStruct so = new StreamObjectStruct(0 , 0);

		so.first = BitConverter.ToInt32( bytes , 0 );
		so.second = BitConverter.ToSingle( bytes , 8 );

		return so;
	}
}

//public class StreamObjectStruct : MonoBehaviour {
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//}
