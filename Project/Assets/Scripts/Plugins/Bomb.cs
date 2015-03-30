﻿using UnityEngine;
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

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}


	[RPC]
	void writeText( string inputText , Vector3 textPosition , Quaternion textRotation )
	{
		GameObject instantiatedTextObject = (GameObject) Instantiate( textPrefab , textPosition , textRotation  );
		TextMesh instantiatedText = instantiatedTextObject.GetComponentsInChildren< TextMesh >()[0];

		instantiatedText.color = defaultTextColor;
		instantiatedText.fontStyle = defaultFontStyle;
		instantiatedText.fontSize = defaultFontSize;

		string[] splitStrings = inputText.Split( new Char[]{ ' ' } );

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
				instantiatedText.color = HexToColor( word.Substring( word.IndexOf( "=" ) + 1 ) );
				if( setDefaultValues )
				{
					defaultTextColor = instantiatedText.color;
				}
			}

			if( word.ToUpperInvariant().Contains( "BOLD" )
			   || word.ToUpperInvariant() == "-B" )
			{
				if( instantiatedText.fontStyle == FontStyle.Italic )
				{
					instantiatedText.fontStyle = FontStyle.BoldAndItalic;
				}
				else
				{
					instantiatedText.fontStyle = FontStyle.Bold;
				}

				if( setDefaultValues )
				{
					defaultFontStyle = instantiatedText.fontStyle;
				}
			}

			if( word.ToUpperInvariant().Contains( "ITALIC" )
			   || word.ToUpperInvariant() == "-I" )
			{
				if( instantiatedText.fontStyle == FontStyle.Bold )
				{
					instantiatedText.fontStyle = FontStyle.BoldAndItalic;
				}
				else
				{
					instantiatedText.fontStyle = FontStyle.Italic;
				}

				if( setDefaultValues )
				{
					defaultFontStyle = instantiatedText.fontStyle;
				}
			}

			if( word.ToUpperInvariant().Contains( "SIZE=" ) 
			   || word.ToUpperInvariant().Contains( "S=" ) )
			{
				int fontSize = Convert.ToInt32( word.Substring( word.IndexOf( "=" ) + 1 ) );
				if( fontSize > 80 )
				{
					fontSize = 80;
				}
				if( fontSize < 0 )
				{
					fontSize = 1;
				}
				instantiatedText.fontSize = fontSize;

				if( setDefaultValues )
				{
					defaultFontSize = fontSize;
				}
			}

			messageStartIndex += word.Length + 1;
		}

		string chatMessage = inputText.Substring( messageStartIndex );


		instantiatedText.text = chatMessage;
	}

	
	void processInput( string input )
	{
		Vector3 myPosition = networkManager.getPlayerPosition();
		Quaternion myRotation = networkManager.getPlayerRotation();

		Vector3 textPosition = myPosition + myRotation * textTranslationVector;

		photonView.RPC( "writeText" , PhotonTargets.All , input , textPosition , myRotation );
	}
}