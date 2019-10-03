using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFromOptitrack : MonoBehaviour
{
	static Transform markersOculus;
	static Transform realOculus;

    public Transform optitrackGameObject;
	static Vector3 offsetDist;
	static Vector3 offsetAngle;

    static SceneConfiguration configScript;

    static bool start = false;

    static Color offsetColor = new Color(0.1f, 0.5f, 0.1f);

    void Start() {
        realOculus = GameObject.Find("Camera Oculus").transform;
        markersOculus = GameObject.Find("Markers Oculus").transform;

        configScript = GameObject.Find("World").GetComponent<SceneConfiguration>();
    }

    void FixedUpdate() {
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
            //offsetDist = markersOculus.position - realOculus.position;
            //offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles;

            Debug.DrawLine(this.transform.position, optitrackGameObject.position, offsetColor);
        }
    }
}
