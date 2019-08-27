using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCorrespondance : MonoBehaviour
{
	public GameObject optirackGameObject;

	OptitrackStreamingClient optitrackClient;
	List<OptiMarker> unlabeledMarkers;

	Transform shoulder, elbow, wrist;

	Transform arm, forearm, forearmRoll, hand;
	Transform finger, thumb;

    // Start is called before the first frame update
    void Start() {
        optitrackClient = optirackGameObject.GetComponent<OptitrackStreamingClient>();

        shoulder = GameObject.Find("Shoulder").transform;
        elbow = GameObject.Find("Elbow").transform;
        wrist = GameObject.Find("Wrist").transform;

        arm = GameObject.Find("RightArm").transform;
        forearm = GameObject.Find("RightForearm_").transform;
        forearmRoll = GameObject.Find("RightForearmRoll").transform;
        hand = GameObject.Find("RightHand").transform;
        /*finger = GameObject.Find("RightArm");
        thumb = GameObject.Find("RightArm");*/
    }

    // Update is called once per frame
    void FixedUpdate() {
        unlabeledMarkers = optitrackClient.unlabeledMarkers;
        if (unlabeledMarkers.Count<2) {
        	Debug.LogWarning("Not enough unlabeled markers to track fingers");
        }

        arm.position = shoulder.position;
        forearm.position = elbow.position;
        forearmRoll.position = (elbow.position + wrist.position)/2;	// L'avant-bras ne se déplace pas par rapport au coude et à la main
        hand.position = wrist.position;
        //TODO: Which unlabeled markers correspond to the fingers?

        // Shoulder
        Vector3 z = (arm.position - forearm.position).normalize;

        // Forearm
        z = (forearm.position - hand.position).Normalize;
        y = 
    }
}
