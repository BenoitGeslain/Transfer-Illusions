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

    GameObject[] curtain;

    void Start() {
        realOculus = GameObject.Find("Camera Oculus").transform;
        markersOculus = GameObject.Find("Markers Oculus").transform;

        configScript = GameObject.Find("World").GetComponent<SceneConfiguration>();

        curtain = new GameObject[3];
        curtain[0] = GameObject.Find("Curtains 1");
        curtain[1] = GameObject.Find("Curtains 2");
        curtain[2] = GameObject.Find("Curtains 3");
    }

    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
        	/*foreach(GameObject g in curtain)
        		g.SetActive(false);*/// uncomment for main exp
            offsetDist = markersOculus.position - realOculus.position;
            offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            if (this.name == "World") {
                //this.transform.eulerAngles = new Vector3(0f, optitrackGameObject.eulerAngles.y/* - offsetAngle.y*/, 0f); // uncomment for main exp
                offsetAngle = new Vector3(0f, 0f, 0f);
            }
            else
                this.transform.eulerAngles = optitrackGameObject.eulerAngles;

        	/*foreach(GameObject g in curtain)
        		g.SetActive(true);*/// uncomment for main exp
            //configScript.saveConfig = true;
            start = true;
        } else if (start && this.name != "World") {
            //offsetDist = markersOculus.position - realOculus.position;
            //offsetAngle = markersOculus.eulerAngles - realOculus.eulerAngles;

            this.transform.position = optitrackGameObject.position - offsetDist;
            this.transform.eulerAngles = optitrackGameObject.eulerAngles;

            //Debug.DrawLine(this.transform.position, optitrackGameObject.position, offsetColor);
        }
    }
}
