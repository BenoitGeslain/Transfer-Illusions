using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFromOptitrack : MonoBehaviour
{

	public Transform optitrackGameObject;
	public Transform markersOculus;
	public Transform realOculus;

	private Vector3 offsetDist;
	private Vector3 offsetAngle;

    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (/*this.name != "World" || */Input.GetKeyDown(KeyCode.KeypadEnter)) {
            offsetDist = markersOculus.position - realOculus.position;

            offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            //this.transform.rotation.eulerAngles = optitrackGameObject.rotation - offsetAngle;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles - offsetAngle;
            start = true;
        } else if (start && this.name != "World") {
            this.transform.position = optitrackGameObject.position - offsetDist;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles - offsetAngle;
        }
    }
}
