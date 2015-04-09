using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bomb : MonoBehaviour {
	
	private PluginManager pluginManager;
	private PhotonView photonView;
	private NetworkManager networkManager;

	public GameObject textPrefab;

	private Vector3 textTranslationVector = new Vector3( 0 , 0 , 5 );

	private Color defaultTextColor = new Color( 255, 255 , 255);
	private int defaultFontSize = 14;
	private FontStyle defaultFontStyle = FontStyle.Normal;
	
	
	// Use this for initialization
	void Start () {
		pluginManager = GetComponent< PluginManagerInterface > ().getPluginManager ();
		photonView = GetComponent< PluginManagerInterface > ().getPhotonView ();
		networkManager = GetComponent< PluginManagerInterface > ().getNetworkManager ();
		
		Plugin me;
		me.keyword = "#BOMB";
		me.function = processInput;
		
		pluginManager.registerPlugin (me);
		
	}

	string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}


	[RPC]
	void writeText( string inputText , Vector3 textPosition , Quaternion textRotation , string textColorHex , int fontSize , FontStyle fontStyle )
	{
		GameObject instantiatedTextObject = (GameObject) Instantiate( textPrefab , textPosition , textRotation  );
		TextMesh instantiatedText = instantiatedTextObject.GetComponentsInChildren< TextMesh >()[0];

		Color textColor = HexToColor( textColorHex );

		instantiatedText.color = textColor;
		instantiatedText.fontStyle = fontStyle;
		instantiatedText.fontSize = fontSize;
		instantiatedText.text = inputText;
	}

	
	void processInput( string input )
	{
		Vector3 myPosition = networkManager.getPlayerPosition();
		Quaternion myRotation = networkManager.getPlayerRotation();

		Vector3 textPosition = myPosition + myRotation * textTranslationVector;

		Color textColor = defaultTextColor;
		int fontSize = defaultFontSize;
		FontStyle fontStyle = defaultFontStyle;

		string[] splitStrings = input.Split( new Char[]{ ' ' } );
		
		int messageStartIndex = 0;
		bool setDefaultValues = false;
		foreach( string word in splitStrings )
		{
			if( !word.ToUpperInvariant().StartsWith( "-" ) )
			{
				break;
			}
			
			if( word.ToUpperInvariant() == "-SET" )
			{
				setDefaultValues = true;
			}
			
			if( word.ToUpperInvariant().Contains( "COLOR=" ) 
			   || word.ToUpperInvariant().Contains( "C=" ) )
			{
				textColor = HexToColor( word.Substring( word.IndexOf( "=" ) + 1 ) );
				if( setDefaultValues )
				{
					defaultTextColor = textColor;
				}
			}
			
			if( word.ToUpperInvariant().Contains( "BOLD" )
			   || word.ToUpperInvariant() == "-B" )
			{
				if( fontStyle == FontStyle.Italic )
				{
					fontStyle = FontStyle.BoldAndItalic;
				}
				else
				{
					fontStyle = FontStyle.Bold;
				}
				
				if( setDefaultValues )
				{
					defaultFontStyle = fontStyle;
				}
			}
			
			if( word.ToUpperInvariant().Contains( "ITALIC" )
			   || word.ToUpperInvariant() == "-I" )
			{
				if( fontStyle == FontStyle.Bold )
				{
					fontStyle = FontStyle.BoldAndItalic;
				}
				else
				{
					fontStyle = FontStyle.Italic;
				}
				
				if( setDefaultValues )
				{
					defaultFontStyle = fontStyle;
				}
			}
			
			if( word.ToUpperInvariant().Contains( "SIZE=" ) 
			   || word.ToUpperInvariant().Contains( "S=" ) )
			{
				fontSize = Convert.ToInt32( word.Substring( word.IndexOf( "=" ) + 1 ) );
				if( fontSize > 80 )
				{
					fontSize = 80;
				}
				if( fontSize < 0 )
				{
					fontSize = 1;
				}
				
				if( setDefaultValues )
				{
					defaultFontSize = fontSize;
				}
			}
			
			messageStartIndex += word.Length + 1;
		}
		
		string chatMessage = input.Substring( messageStartIndex );

		photonView.RPC( "writeText" , PhotonTargets.All , chatMessage , textPosition , myRotation , ColorToHex( textColor ) , fontSize , fontStyle );
	}
}