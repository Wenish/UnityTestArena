using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFollow : MonoBehaviour, IClickable {

	public Follower myPlayerFollower;
	public NetworkEntity networkEntity;

	void Start() {
		networkEntity = GetComponent<NetworkEntity> ();
	}

	public void OnClick (RaycastHit hit)
	{
		Debug.Log ("following " + hit.collider.gameObject.name);

		Network.Follow (networkEntity.id);

		myPlayerFollower.target = transform;
	}
}
