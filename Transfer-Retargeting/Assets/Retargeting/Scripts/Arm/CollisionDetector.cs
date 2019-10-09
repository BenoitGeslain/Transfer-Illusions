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
        trialManager = GameObject.Find("World").GetComponent<TrialManager>();

        noContact = new Color(0.5660378f, 0.5660378f, 0.5660378f);
        contact = new Color(0.5660378f, 0.0f, 0.0f, 0.6627451f);
    }

    // Update is called once per frame
    void Update() {
        
    }

	void OnTriggerEnter(Collider col) {
        contact.a = col.gameObject.GetComponent<Renderer>().material.color.a;
        col.gameObject.GetComponent<Renderer>().material.color = contact;
		trialManager.collisions++;
    }

	void OnTriggerExit(Collider col) {
        noContact.a = col.gameObject.GetComponent<Renderer>().material.color.a;
        col.gameObject.GetComponent<Renderer>().material.color = noContact;
    }
}
