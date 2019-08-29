using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForearmRoll : MonoBehaviour
{
	public Transform forearm, hand;

    void Start() {

    }

    void Update() {
        //transform.position = (forearm.position - hand.position)/2;
        transform.eulerAngles = forearm.eulerAngles;
        print(transform.position);
    }
}
