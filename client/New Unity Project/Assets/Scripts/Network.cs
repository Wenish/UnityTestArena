using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
public class Network : MonoBehaviour {

	static SocketIOComponent socket;

	public GameObject myPlayer;

	public Spawner spawner;


	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent> ();
		socket.On ("open", OnConnected);
		socket.On ("register", OnRegister);
		socket.On ("spawn", OnSpawned);
		socket.On ("move", OnMove);
		socket.On ("follow", OnFollow);
		socket.On ("attack", OnAttack);
		socket.On ("disconnected", OnDisconnected);
		socket.On ("requestPosition", OnRequestPosition);
		socket.On ("updatePosition", OnUpdatePosition);

	}

	void OnConnected(SocketIOEvent e) {
		Debug.Log("connected");
	}

	void OnRegister(SocketIOEvent e) {
		Debug.Log ("Successfully registered, with id: " + e.data);
		spawner.AddPlayer (e.data ["id"].str, myPlayer);
	}

	void OnSpawned(SocketIOEvent e) {
		Debug.Log ("spawned" + e.data["id"].str);
		var player = spawner.SpawnPlayer (e.data["id"].str);

		if (e.data ["x"]) {
			
			var movePosition = GetVectorFromJSON (e);

			var navigationPos = player.GetComponent<Navigator> ();

			navigationPos.NavigateTo (movePosition);
		}
	}

	void OnMove(SocketIOEvent e) {
		Debug.Log ("player is moving" + e.data);

		var position = GetVectorFromJSON (e);
			
		var player = spawner.FindPlayer(e.data ["id"].str);

		var navigationPos = player.GetComponent<Navigator> ();

		navigationPos.NavigateTo (position);

	}

	void OnFollow(SocketIOEvent e) {
		Debug.Log ("follow request " + e.data);
		var player = spawner.FindPlayer(e.data ["id"].str);

		var targetTransform = spawner.FindPlayer(e.data ["targetId"].str).transform;

		var target = player.GetComponent<Targeter> ();

		target.target = targetTransform;
	}

	void OnAttack(SocketIOEvent e) {
		Debug.Log ("recieved attack " + e.data);

		var targetPlayer = spawner.FindPlayer(e.data ["targetId"].str);

		targetPlayer.GetComponent<Hittable> ().OnHit ();

		var attackingPlayer = spawner.FindPlayer(e.data ["id"].str);

		attackingPlayer.GetComponent<Animator> ().SetTrigger ("Attack");
	}

	void OnRequestPosition(SocketIOEvent e) {
		Debug.Log ("server is requesting position");
		socket.Emit("updatePosition", VectorToJson(myPlayer.transform.position));
	}

	void OnDisconnected(SocketIOEvent e) {
		Debug.Log ("player disconnected: " + e.data);

		var id = e.data ["id"].str;

		spawner.Remove (id);
	}

	void OnUpdatePosition(SocketIOEvent e) {
		Debug.Log ("update position " + e.data);

		var position = GetVectorFromJSON (e);

		var player = spawner.FindPlayer(e.data ["id"].str);

		player.transform.position = position;
	}

	static public void Move (Vector3 position) {
		// send pos to node
		Debug.Log("sending position to node" + Network.VectorToJson(position));
		socket.Emit("move", Network.VectorToJson(position));
	}

	static public void Follow (string id) {
		// send pos to node
		Debug.Log("sending follow player id" + Network.PlayerIdToJson(id));
		socket.Emit("follow", Network.PlayerIdToJson(id));
	}

	static public void Attack(string targetId){
		Debug.Log ("attacking player: " + Network.PlayerIdToJson (targetId));
		socket.Emit("attack", Network.PlayerIdToJson(targetId));
	}

	static Vector3 GetVectorFromJSON(SocketIOEvent e){
		return new Vector3 (e.data ["x"].n, 0, e.data ["y"].n);
	}

	public static JSONObject VectorToJson (Vector3 vector) {
		JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
		j.AddField ("x", vector.x);
		j.AddField ("y", vector.z);
		return j;
	}

	public static JSONObject PlayerIdToJson (string id) {
		JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
		j.AddField ("targetId", id);
		return j;
	}

}
