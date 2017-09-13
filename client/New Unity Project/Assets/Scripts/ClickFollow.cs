using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFollow : MonoBehaviour, IClickable {

	public GameObject myPlayer;
	public NetworkEntity networkEntity;

	Targeter myPlayerTargeter;

	void Start() {
		networkEntity = GetComponent<NetworkEntity> ();
		myPlayerTargeter = myPlayer.GetComponent<Targeter> ();
	}

	public void OnClick (RaycastHit hit)
	{
		Debug.Log ("following " + hit.collider.gameObject.name);

		Network.Follow (networkEntity.id);

		myPlayerTargeter.target = transform;
	}
}
