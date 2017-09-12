﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour {

	public Transform target;

	public float scanFrequency = 0.5f;
	public float stopFollowDistance = 2;

	NavMeshAgent agent;

	float lastScanTime = 0;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isReadyToScan () && !isInRange()) {
			Debug.Log ("scanning nav path");
			agent.SetDestination (target.position);
		}
	}

	bool isReadyToScan ()
	{
		return Time.time - lastScanTime > scanFrequency && target;
	}

	bool isInRange ()
	{
		var distance = Vector3.Distance (target.position, transform.position);
		return distance < stopFollowDistance;
	}
}
