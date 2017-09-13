using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour, IClickable {

	public GameObject player;

	public void OnClick (RaycastHit hit) {
		var navigator = player.GetComponent<Navigator> ();

		navigator.NavigateTo (hit.point);
		Network.Move (player.transform.position, hit.point);
	}
}
