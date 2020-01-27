using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorID : MonoBehaviour {

	public int id;
	SphereSize sSScript;

	void Start() {
		sSScript = GameObject.Find("World").GetComponent<SphereSize>();
	}

	void OnTriggerEnter(Collider c) {
		sSScript.Collision(id);
	}
}
