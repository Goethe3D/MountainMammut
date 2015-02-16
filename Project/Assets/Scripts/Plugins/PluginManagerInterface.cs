using UnityEngine;
using System.Collections;

public class PluginManagerInterface : MonoBehaviour {

	public GameObject pluginManagerObject;
	public GameObject photonViewObject;

	public PluginManager getPluginManager()
	{
		return pluginManagerObject.GetComponent< PluginManager > ();
	}

	public PhotonView getPhotonView()
	{
		return GetComponent< PhotonView > ();
	}

	public NetworkManager getNetworkManager()
	{
		return photonViewObject.GetComponent< NetworkManager > ();
	}
}
