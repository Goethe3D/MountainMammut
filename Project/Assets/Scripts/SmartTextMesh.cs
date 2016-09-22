using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class SmartTextMesh : MonoBehaviour
{
	TextMesh TheMesh;
	public string UnwrappedText;
	public float MaxWidth;
	public bool NeedsLayout = true;
	public bool ConvertNewLines = false;

	Vector3 defaultPosition;

	void Start()
	{
		TheMesh = GetComponent<TextMesh>();
		if (ConvertNewLines)
			UnwrappedText = UnwrappedText.Replace("\\n", System.Environment.NewLine);

		defaultPosition = transform.localPosition;
	}

	void moveUp()
	{
		Vector3 newPosition = transform.localPosition;
		newPosition.y += 1f;
		transform.localPosition = newPosition;
	}

	string BreakPartIfNeeded(string part)
	{
		string saveText = TheMesh.text;
		TheMesh.text = part;

		if (TheMesh.GetComponent<Renderer>().bounds.extents.x > MaxWidth)
		{
			string remaining = part;
			part = "";
			while (true)
			{
				int len;
				for (len = 2; len <= remaining.Length; len++)
				{
					TheMesh.text = remaining.Substring(0, len);
					if (TheMesh.GetComponent<Renderer>().bounds.extents.x > MaxWidth)
					{
						len--;
						break;
					}
				}
				if (len >= remaining.Length)
				{
					part += remaining;
					break;
				}
				part += remaining.Substring(0, len) + System.Environment.NewLine;
				moveUp();
				remaining = remaining.Substring(len);
			}

			part = part.TrimEnd();
		}

		TheMesh.text = saveText;

		return part;
	}

	void Update()
	{
		if (!NeedsLayout)
			return;
		NeedsLayout = false;
		transform.localPosition = defaultPosition;
		if (MaxWidth == 0)
		{
			TheMesh.text = UnwrappedText;
			return;
		}
		string builder = "";
		string text = UnwrappedText;
		TheMesh.text = "";
		string[] parts = text.Split(' ');
		for (int i = 0; i < parts.Length; i++)
		{
			string part = BreakPartIfNeeded(parts[i]);
			TheMesh.text += part + " ";
			if (TheMesh.GetComponent<Renderer>().bounds.extents.x > MaxWidth)
			{
				Debug.Log( "Inserting New Line" );
				TheMesh.text = builder.TrimEnd() + System.Environment.NewLine + part + " ";
				moveUp();
			}
			builder = TheMesh.text;
		}
	}
}