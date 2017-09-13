using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

	public float attackDistance;
	public float attackRate;

	float lastAttackTime = 0;

	Targeter targeter;

	// Use this for initialization
	void Start () {
		targeter = GetComponent<Targeter> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (targeter.target != null) {
			if (targeter.target.GetComponent<Hittable> ().IsDead) {
				targeter.target = null;
				return;
			}
		}

		if (isReadyToAttack() && targeter.IsInRange (attackDistance) && !targeter.target.GetComponent<Hittable>().IsDead) {
			Debug.Log ("attacking" + targeter.target.name);

			var targetId = targeter.target.GetComponent<NetworkEntity> ().id;

			Network.Attack (targetId);
			lastAttackTime = Time.time;

		}
	}

	bool isReadyToAttack() {
		return Time.time - lastAttackTime > attackRate && targeter.target;
	}

}
