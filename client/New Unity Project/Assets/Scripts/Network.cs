using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
public class Network : MonoBehaviour {

	static SocketIOComponent socket;

	public GameObject playerPrefab;

	public GameObject myPlayer;

	Dictionary<string, GameObject> players;

	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent> ();
		socket.On ("open", OnConnected);
		socket.On ("spawn", OnSpawned);
		socket.On ("move", OnMove);
		socket.On ("disconnected", OnDisconnected);
		socket.On ("requestPosition", OnRequestPosition);
		socket.On ("updatePosition", OnUpdatePosition);

		players = new Dictionary<string, GameObject> ();
	}

	void OnConnected(SocketIOEvent e) {
		Debug.Log("connected");
	}

	void OnSpawned(SocketIOEvent e) {
		Debug.Log ("spawned" + e.data["id"].ToString());
		var player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		if (e.data ["x"]) {
			
			var movePosition = new Vector3 (GetFloatFromJson (e.data, "x"), 0, GetFloatFromJson (e.data, "y"));

			var navigationPos = player.GetComponent<NavigatePosition> ();

			navigationPos.NavigateTo (movePosition);
		}

		players.Add (e.data["id"].ToString(), player);
		Debug.Log ("count: " + players.Count);
	}

	void OnMove(SocketIOEvent e) {
		Debug.Log ("player is moving" + e.data);

		var position = new Vector3 (GetFloatFromJson (e.data, "x"), 0, GetFloatFromJson (e.data, "y"));
			
		var player = players [e.data ["id"].ToString()];

		var navigationPos = player.GetComponent<NavigatePosition> ();

		navigationPos.NavigateTo (position);

	}

	void OnRequestPosition(SocketIOEvent e) {
		Debug.Log ("server is requesting position");
		socket.Emit("updatePosition", new JSONObject(VectorToJson(myPlayer.transform.position)));
	}

	void OnDisconnected(SocketIOEvent e) {
		Debug.Log ("player disconnected: " + e.data);

		var id = e.data ["id"].ToString ();

		var player = players [id];
		Destroy (player);
		players.Remove (id);
	}

	void OnUpdatePosition(SocketIOEvent e) {
		Debug.Log ("update position " + e.data);

		var position = new Vector3 (GetFloatFromJson (e.data, "x"), 0, GetFloatFromJson (e.data, "y"));

		var player = players [e.data ["id"].ToString()];

		player.transform.position = position;
	}

	float GetFloatFromJson(JSONObject data, string key){
		return float.Parse(data [key].str);
	}

	public static string VectorToJson (Vector3 vector) {
		return string.Format (@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
	}

}
