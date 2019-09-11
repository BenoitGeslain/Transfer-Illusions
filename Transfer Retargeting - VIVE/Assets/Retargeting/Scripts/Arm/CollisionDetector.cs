using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
	BodyWarping bodyWarping;
	TrialManager trialManager;

	Color noContact, contact;

    void Start() {
        bodyWarping = GameObject.Find("World").GetComponent<BodyWarping>();
        noContact = new Color(0.5660378f, 0.5660378f, 0.5660378f, 0.6627451f);
        contact = new Color(0.5660378f, 0.0f, 0.0f, 0.6627451f);
    }

    // Update is called once per frame
    void Update() {
        
    }

	void OnTriggerEnter(Collider col) {
		// Play sound
		col.gameObject.GetComponent<Renderer>().material.color = contact;
		trialManager.collisions++;
    }

	void OnTriggerExit(Collider col) {
		col.gameObject.GetComponent<Renderer>().material.color = noContact;
    }
}
