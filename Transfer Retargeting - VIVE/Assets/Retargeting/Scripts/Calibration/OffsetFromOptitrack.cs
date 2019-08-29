using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFromOptitrack : MonoBehaviour
{

	public Transform optitrackGameObject;
	Transform markersOculus;
	Transform realOculus;

	private Vector3 offsetDist;
	private Vector3 offsetAngle;

    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        realOculus = optitrackGameObject.Find("Camera Oculus");
        markersOculus = optitrackGameObject.Find("Markers Oculus");
    }

    // Update is called once per frame
    void Update()
    {
        if (/*this.name != "World" || */Input.GetKeyDown(KeyCode.KeypadEnter)) {
            offsetDist = markersOculus.position - realOculus.position;

            offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles - offsetAngle;
            start = true;

        } else if (start && this.name != "World") {
            this.transform.position = optitrackGameObject.position - offsetDist;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles - offsetAngle;
			//Debug.DrawLine(this.transform.position, optitrackGameObject.position, new Color(1f, 0f, 0f));
        }
    }
}
