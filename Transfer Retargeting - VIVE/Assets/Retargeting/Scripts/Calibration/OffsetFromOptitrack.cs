using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFromOptitrack : MonoBehaviour
{
	Transform markersOculus;
	Transform realOculus;

    public Transform optitrackGameObject;
	Vector3 offsetDist;
	Vector3 offsetAngle;

    LogSceneConfiguration configScript;

    bool start = false;

    void Start() {
        realOculus = GameObject.Find("Camera Oculus").transform;
        markersOculus = GameObject.Find("Markers Oculus").transform;

        configScript = GameObject.Find("World").GetComponent<LogSceneConfiguration>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            offsetDist = markersOculus.position - realOculus.position;
            offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            if (this.name == "World") 
                this.transform.eulerAngles = new Vector3(0f, optitrackGameObject.eulerAngles.y, 0f);
            else
                this.transform.eulerAngles = optitrackGameObject.eulerAngles;
            configScript.saveConfig = true;
            start = true;
        } else if (start && this.name != "World") {
            offsetDist = markersOculus.position - realOculus.position;
            offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            // this.transform.eulerAngles = optitrackGameObject.eulerAngles - offsetAngle;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles;

			Debug.DrawLine(this.transform.position, optitrackGameObject.position, new Color(1f, 0f, 0f));
        }
    }
}
