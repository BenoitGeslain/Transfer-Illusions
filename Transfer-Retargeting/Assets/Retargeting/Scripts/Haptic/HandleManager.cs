using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleManager : MonoBehaviour {

	Collider collision;
	int colStatus;

	// Use this for initialization
	void Start () {
		/*this.collision = GetComponent<Collider>();
		this.collision.enabled = false;*/
		this.colStatus = 0;
	}

	void OnTriggerEnter (Collider col) {
        if(col.gameObject.name != "Proxy") {
			this.colStatus++;
        }
    }

	void OnTriggerExit(Collider col) {
        if(col.gameObject.name != "Proxy") {
			this.colStatus--;
        }
    }

	public bool GetStatus() {
		return (this.colStatus>0);
	}

	// Update is called once per frame
	void Update() { }
}
