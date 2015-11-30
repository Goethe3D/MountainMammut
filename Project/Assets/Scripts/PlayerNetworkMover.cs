using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {
	
//	public delegate void Respawn(float time);
//	public event Respawn RespawnMe;
//	
	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;
//	float health = 100f;
	bool Crouch = false;
//	bool sprint = false;
	bool nowWalking = false;
	bool initialLoad = true;

	Animator anim;

	
	
	void Start () {

//		anim = GetComponentInChildren<Animator> ();
		if(photonView.isMine)
		{
//			GetComponent<Rigidbody>().useGravity = true;
			//GetComponent<UnitySampleAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
//			GetComponent<FirstPersonCharacter>().enabled = true;
//			GetComponent<FirstPersonHeadBob>().enabled = true;
//			GetComponent<FirstPersonController>().enabled = true;
//			foreach(SimpleMouseRotator rot in GetComponentsInChildren<SimpleMouseRotator>())
//				rot.enabled = true;
			foreach(Camera cam in GetComponentsInChildren<Camera>())
				cam.enabled = true;

//			transform.Find ("Head Joint/First Person Camera/GunCamera/Candy-Cane").gameObject.layer = 11;
//			transform.Find ("Head Joint/First Person Camera/GunCamera/Candy-Cane/Sights").gameObject.layer = 11;
		}
		// if its another player coroutine
		else{
			foreach(Camera cam in GetComponentsInChildren<Camera>())
				cam.enabled = false;
			foreach(fly1 flying in GetComponentsInChildren<fly1>())
				flying.enabled = false;
			StartCoroutine("UpdateData");
		}
	}

	//cooroutine to smooth movement of other players
	IEnumerator UpdateData()
	{

		if(initialLoad)
		{
			initialLoad = false;
			transform.position = position;
			transform.rotation = rotation;
		}

		while(true)
		{
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
//			anim.SetBool("Crouch", Crouch);
//			anim.SetBool("Walking", nowWalking);
//			anim.SetBool ("Sprint", sprint);
			yield return null;
		}
	}

	//reads and updates everything that is not mine or sends to the network
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
//			stream.SendNext(health);
//			stream.SendNext(anim.GetBool ("Crouch"));
//			stream.SendNext(anim.GetBool ("Walking"));
//			stream.SendNext(anim.GetBool ("Sprint"));
		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();
//			health = (float)stream.ReceiveNext();
//			Crouch = (bool)stream.ReceiveNext();
//			nowWalking = (bool)stream.ReceiveNext();
//			sprint = (bool)stream.ReceiveNext();

		}
	}


//	RPCs call all the machines at the same time and can call function
//	[RPC]
//	public void GetShot(float damage)
//	{
//		health -= damage;
//		if(health <= 0 && photonView.isMine)
//		{
//			if(RespawnMe != null)
//				RespawnMe(3f);
//			
//			PhotonNetwork.Destroy (gameObject);
//		}
//	}
	
}