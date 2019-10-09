using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;﻿
using UnityEditor;

/*	Script computing the forces and gains relating to the coupling of
	a haptic interface
 * 		- Gets haptic interface information through "SocketImpedance.cs"
 * 			by creating an instance of the class
 * 		- Requires "Handle" GameObject which has no collision
 * 			to behave correctly
 * 		- Requires "Proxy" GameObject which has collisions and a
 * 			rigidbody to simulate a realistic behavior
 */

public class CouplingImpedance : MonoBehaviour {

	//public GameObject[] attractives, repulsives;

	struct Point {
		public float distance;
		public Vector3 position;
		public int index;

		public Point(float d, Vector3 p, int i) {
			this.distance = d;
			this.position = p;
			this.index = i;
		}
	}

	SocketImpedance socket;
	Rigidbody rb;
	GameObject handle;
	HandleManager handleScript;

	Vector3 force;
	Vector3 deltaPos;

	int sizeFactor = 35;

	float k = 1f; // Virtuose = 80
	float b = 0;
	float m = 0;

	//Vector3 B = new Vector3(1/8, 1/8, 1/8);

	// Use this for initialization
	void Start() {
		this.socket = ScriptableObject.CreateInstance<SocketImpedance>();
		this.force = Vector3.zero;	// force vector to be sent to haptic interface

		// Getting proxy Rigidbody and its parameters
		this.rb = GetComponent<Rigidbody>();
		this.m = this.rb.mass;

		// Computing dampening (stability)
		this.b = 2f*(float)Math.Pow(this.k*this.m, 0.5f);
		print("INIT:: Coefficients: k = " + this.k + ", b = " + this.b);

		// Getting Handle GameObject and its script
		handle = GameObject.Find("Handle");
		handleScript = this.handle.GetComponent<HandleManager>();

		// Initialize position of handle and proxy to avoid huge forces
		// at the start of the game
		this.rb.position = this.sizeFactor*(socket.GetPosition());
		handle.transform.position = this.sizeFactor*(socket.GetPosition());
		print("INIT:: Haptics Done");
	}

	void FixedUpdate() {
		// Retreive positions and velocity of handle and proxy
		Vector3 posHandle = this.sizeFactor*(socket.GetPosition());
		Vector3 velHandle = this.sizeFactor*(socket.GetSpeed());
		Vector3 posProxy  = this.rb.position;
		Vector3 velProxy  = this.rb.velocity;

		// Copy the position of the haptic interface to the proxy
		handle.transform.position = posHandle;

		// Compute force applied to proxy and haptic interface (spring-dampener equations)
		this.deltaPos = posHandle-posProxy;
		this.force = this.k*deltaPos - this.b*(velProxy-velHandle);

		/*Vector3 magForce = getMagneticForce(posProxy);
		print(magForce);*/
		// Applying forces to proxy
		this.rb.AddForce(force);

		if (this.handleScript.GetStatus()) {		// Applies a force if handle is colliding with an object
			this.socket.SetForce(- (force));
		}
	}

	/*Vector3 getMagneticForce(Vector3 posProxy) {
		Vector3 f = new Vector3(0f, 0f, 0f);
		foreach (GameObject a in attractives) {
			f += Vector3.Scale(B, invertVector(posProxy-a.transform.position));
			print(posProxy-a.transform.position + " " + invertVector(posProxy-a.transform.position));
		}
		foreach (GameObject r in repulsives) {
			f += Vector3.Scale(B, invertVector(posProxy-r.transform.position));
		}
		return f;
	}*/

	public Vector3 GetPosition() {
		return this.rb.position;
	}

	public Vector3 GetDeltaPosition() {
		return deltaPos;
	}

	public Vector3 GetForce() {
		return force;
	}

	/*Vector3 invertVector(Vector3 v) {
		if (v.x<0.1 && v.x>0) v.x=0.1f;
		else if (v.x>-0.1) v.x=-0.1f;
		if (v.y<0.1 && v.y>0) v.y=0.1f;
		else if (v.y>-0.1) v.y=-0.1f;
		if (v.z<0.1 && v.y>0) v.z=0.1f;
		else if (v.z>-0.1) v.z=-0.1f;
		return new Vector3(1/v.x, 1/v.y, 1/v.z);
	}*/
}
