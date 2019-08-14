using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderBase : MonoBehaviour
{
	public Transform rightArm;
	
	Vector3 position, orientation;
    // Start is called before the first frame update
    void Awake() {
        position = rightArm.position - transform.position;
        orientation = rightArm.eulerAngles - transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
        transform.position = rightArm.position + position;
        transform.eulerAngles = rightArm.eulerAngles + orientation;

    }
}
