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

    private bool start = true;

    // Start is called before the first frame update
    void Start()
    {
        realOculus = GameObject.Find("Camera Oculus").transform;
        markersOculus = GameObject.Find("Oculus OptiTrack").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            offsetDist = markersOculus.position - realOculus.position;
            offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles;
            //this.transform.eulerAngles = optitrackGameObject.eulerAngles - offsetAngle;
            start = true;

        } else if (start && this.name != "World") {
            /*offsetDist = markersOculus.position - realOculus.position;
            offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;*/

            this.transform.position = optitrackGameObject.position - offsetDist;
            // this.transform.eulerAngles = optitrackGameObject.eulerAngles - offsetAngle;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles;

			Debug.DrawLine(this.transform.position, optitrackGameObject.position, new Color(1f, 0f, 0f));
        }
    }
}
