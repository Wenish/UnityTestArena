using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour {

	public float health = 100;

	public bool IsDead {
		get{
			return health <= 0;	
		}
	}

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public void OnHit () {
		health -= 10;

		if (IsDead) {
			animator.SetTrigger ("Dead");
		}
	}
}
