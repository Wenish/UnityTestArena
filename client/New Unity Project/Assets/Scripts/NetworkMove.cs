﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkMove : MonoBehaviour {

	public SocketIOComponent socket;

	public void OnMove (Vector3 position) {
		// send pos to node
		Debug.Log("sending position to node" + position);
		socket.Emit("move");
	}
}