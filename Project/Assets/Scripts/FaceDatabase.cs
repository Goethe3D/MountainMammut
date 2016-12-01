using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaceDatabase : MonoBehaviour {

	string[] m_faceDatabase = new string[10];

	// Use this for initialization
	void Start () {

		//m_faceDatabase = new List< string >( 10 );


		m_faceDatabase[ 0 ] = "~ + ~";
		m_faceDatabase[ 1 ] = "O___O";
		m_faceDatabase[ 2 ] = "?___?";
		m_faceDatabase[ 3 ] = "X___X";
		m_faceDatabase[ 4 ] = "L:::R";
		m_faceDatabase[ 5 ] = ">>|<<";
		m_faceDatabase[ 6 ] = "# | #";
		m_faceDatabase[ 7 ] = "$___$";
		m_faceDatabase[ 8 ] = "@___@";
		m_faceDatabase[ 9 ] = "^___^";
	
	}

	public string getFace( int id )
	{
		Debug.Log( "Getting Face " + id + " : " + m_faceDatabase[ id ] );
		return m_faceDatabase[ id ];
	}

	public void setFace( int id , string newFace )
	{
		Debug.Log( "Setting Face " + id + " : " + newFace );
		m_faceDatabase[ id ] = newFace;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static int idForKeycode( KeyCode kc )
	{
		switch( kc )
		{
		case KeyCode.Alpha1:
			{
				return 0;
			}
		case KeyCode.Alpha2:
			{
				return 1;
			}
		case KeyCode.Alpha3:
			{
				return 2;
			}
		case KeyCode.Alpha4:
			{
				return 3;
			}
		case KeyCode.Alpha5:
			{
				return 4;
			}
		case KeyCode.Alpha6:
			{
				return 5;
			}
		case KeyCode.Alpha7:
			{
				return 6;
			}
		case KeyCode.Alpha8:
			{
				return 7;
			}
		case KeyCode.Alpha9:
			{
				return 8;
			}
		case KeyCode.Alpha0:
			{
				return 9;
			}
		default:
			{
				return 0;
			}
		}
	}

	public static bool isValidFace( KeyCode kc )
	{
		return kc == KeyCode.Alpha0 ||
			kc == KeyCode.Alpha1 ||
			kc == KeyCode.Alpha2 ||
			kc == KeyCode.Alpha3 ||
			kc == KeyCode.Alpha4 ||
			kc == KeyCode.Alpha5 ||
			kc == KeyCode.Alpha6 ||
			kc == KeyCode.Alpha7 ||
			kc == KeyCode.Alpha8 ||
			kc == KeyCode.Alpha9;
	}

	public static int numberButtonPressed()
	{
		if( Input.GetKeyUp( KeyCode.Alpha1 ) )
		{
			return 0;
		}

		if( Input.GetKeyUp( KeyCode.Alpha2 ) )
		{
			return 1;
		}

		if( Input.GetKeyUp( KeyCode.Alpha3 ) )
		{
			return 2;
		}

		if( Input.GetKeyUp( KeyCode.Alpha4 ) )
		{
			return 3;
		}

		if( Input.GetKeyUp( KeyCode.Alpha5 ) )
		{
			return 4;
		}

		if( Input.GetKeyUp( KeyCode.Alpha6 ) )
		{
			return 5;
		}

		if( Input.GetKeyUp( KeyCode.Alpha7 ) )
		{
			return 6;
		}

		if( Input.GetKeyUp( KeyCode.Alpha8 ) )
		{
			return 7;
		}

		if( Input.GetKeyUp( KeyCode.Alpha9 ) )
		{
			return 8;
		}

		if( Input.GetKeyUp( KeyCode.Alpha0 ) )
		{
			return 9;
		}

		return -1;
	}
}
