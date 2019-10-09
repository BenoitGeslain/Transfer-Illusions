using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderBase : MonoBehaviour
{
	public GameObject rightArm;
	
	Vector3 offset, orientation;
    // Start is called before the first frame update
    void Awake() {
        offset = rightArm.transform.position - transform.position;
        orientation = rightArm.transform.eulerAngles - transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
        transform.position = rightArm.transform.position + offset;
        transform.eulerAngles = rightArm.transform.eulerAngles + orientation;

    }
}
