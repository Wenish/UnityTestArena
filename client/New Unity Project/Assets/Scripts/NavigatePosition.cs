using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigatePosition : MonoBehaviour {

	NavMeshAgent agent;

	// Use this for initialization
	void Awake () {
		agent = GetComponent<NavMeshAgent> ();
	}

	public void NavigateTo (Vector3 position) {
		agent.SetDestination (position);
	}

	void Update() {
		GetComponent<Animator> ().SetFloat ("Distance", agent.remainingDistance);
	}
}
