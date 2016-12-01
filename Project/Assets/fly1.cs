using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class fly1 : MonoBehaviour
{
	public float keySpeed = 10;
	public float mouseSpeed = 1.25f;
	public float turnSpeed = 50;
	public GameObject eye;
	
	private Quaternion originalRotation;
	private Vector2 angle = new Vector2(0f, 0f);
	private Vector2 minAngle = new Vector2(-360f, -90f);
	private Vector2 maxAngle = new Vector2(360f, 90f);
	private float limit = 360f;

	private bool translationEnabled = true;
	private bool mouseTranslationEnabled = true;

	bool HMD = false;
	
	// Use this for initialization
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		originalRotation = transform.localRotation;
//		if (VRDevice.isPresent && GetComponent< PhotonView >().isMine ) 
//		{
//			mouseTranslationEnabled = false;
//		}
		if (VRDevice.isPresent && GetComponent< PhotonView >().isMine ) {
			HMD = true;
		}
	}
	
	void Update()
	{
		if (Input.GetKey(KeyCode.A) && translationEnabled)
		{
			Strafe(-keySpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D) && translationEnabled)
		{
			Strafe(keySpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.W) && translationEnabled)
		{   
			Fly(keySpeed * Time.deltaTime);
		}   
		if (Input.GetKey(KeyCode.S) && translationEnabled)
		{
			Fly(-keySpeed * Time.deltaTime);
		}

		if (!mouseTranslationEnabled)
		{
			return;
		}

		float dx = Input.GetAxis("Mouse X");
		float dy = 0;
		if( !HMD )
		{
			dy = Input.GetAxis("Mouse Y");
		}

		if( Input.GetKey(KeyCode.LeftArrow ) && translationEnabled )
		{
			dx -= turnSpeed * Time.deltaTime;
		}

		if( Input.GetKey(KeyCode.RightArrow) && translationEnabled )
		{
			dx += turnSpeed * Time.deltaTime;
		}

		if( Input.GetKey(KeyCode.DownArrow ) && translationEnabled )
		{
			dy -= turnSpeed * Time.deltaTime;
		}

		if( Input.GetKey(KeyCode.UpArrow) && translationEnabled )
		{
			dy += turnSpeed * Time.deltaTime;
		}
			
		Look(new Vector2(dx, dy) * mouseSpeed);
	}
	
	void Strafe (float dist)
	{
		transform.position += eye.transform.right * dist;
	}
	
	void Fly (float dist)
	{
		transform.position += eye.transform.forward * dist;
	}
	
	void Look(Vector2 dist)
	{
		angle += dist;
		
		angle.x = ClampAngle(angle.x, minAngle.x, maxAngle.x);
		angle.y = ClampAngle(angle.y, minAngle.y, maxAngle.y);
		
		Quaternion quatX = Quaternion.AngleAxis(angle.x, Vector3.up);
		Quaternion quatY = Quaternion.AngleAxis(angle.y, -Vector3.right);

		transform.localRotation = originalRotation * quatX * quatY;
		//transform.localRotation = originalRotation * quatX * quatY * eye.transform.localRotation;
		//eye.transform.localRotation = Quaternion.identity;
	}
	
	float ClampAngle(float angle, float min, float max)
	{
		if (angle < -limit)
		{
			angle += limit;
		}
		else if (angle > limit)
		{
			angle -= limit;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public void setTranslationEnabled( bool enabled )
	{
		translationEnabled = enabled;
	}

	public void setMouseTranslationEnabled( bool enabled )
	{
		mouseTranslationEnabled = enabled;
	}
}