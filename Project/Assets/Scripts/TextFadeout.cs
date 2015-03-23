using UnityEngine;
using System.Collections;

public class TextFadeout : MonoBehaviour {

	public const int numFramesUntilFadeout = 1500;
	private int numFramesSinceStart = 0;
	private float frameDecreaseFactor;
	private int framesSinceLastDecrease = 0;
	private int framesUntilDecrease;

	// Use this for initialization
	void Start () {
		framesUntilDecrease = numFramesUntilFadeout / 100;
		frameDecreaseFactor = ( float ) ( 1.0 / 100.0 );
	
	}
	
	// Update is called once per frame
	void Update () {

		if( ++numFramesSinceStart > numFramesUntilFadeout )
		{
			Destroy( gameObject );
		}

		if( ++framesSinceLastDecrease < framesUntilDecrease )
		{
			return;
		}

		framesSinceLastDecrease = 0;
		TextMesh textMesh = GetComponent< TextMesh >();
		Color textColor = textMesh.color;
		textColor.a -= frameDecreaseFactor;
		textMesh.color = textColor;
	
	}
}
