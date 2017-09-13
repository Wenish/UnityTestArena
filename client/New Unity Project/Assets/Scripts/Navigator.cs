using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour {

	NavMeshAgent agent;
	Targeter targeter;

	// Use this for initialization
	void Awake () {
		agent = GetComponent<NavMeshAgent> ();
		targeter = GetComponent<Targeter> ();
	}

	public void NavigateTo (Vector3 position) {
		agent.SetDestination (position);
		targeter.target = null;
	}

	void Update() {
		GetComponent<Animator> ().SetFloat ("Distance", agent.remainingDistance);
	}
}
