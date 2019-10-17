using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathManager : MonoBehaviour
{
    LineRenderer lineRenderer;

    GameObject[] grabbables;
    GameObject[] phantoms;
    Transform phantomIntro;
    public int index = -1, prevIndex = -1;

    Vector3[] points;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();

        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        phantomIntro = GameObject.Find("Cube Phantom Intro").transform;
        points = new Vector3[5];
    }

    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            prevIndex = -1;
        }
    	switch (index) {
            case 0:
                points[0] = grabbables[0].transform.position;
                points[1] = grabbables[0].transform.position + new Vector3(0f, 0.04f, 0f);
                points[2] = points[1]; points[2].x = phantomIntro.position.x;
                points[3] = phantomIntro.position;
                lineRenderer.positionCount = 4;
                lineRenderer.SetPositions(points);
                break;
            case 1:
                if (prevIndex !=1) {
                    points[0] = phantomIntro.transform.position;
                    points[1] = phantomIntro.transform.position + new Vector3(0f, 0.04f, 0f);
                    points[2] = points[1]; points[2].x = phantoms[0].transform.position.x;
                    points[3] = phantoms[0].transform.position;
                    lineRenderer.positionCount = 4;
                    lineRenderer.SetPositions(points);
                    prevIndex = 1;
                }
                break;
            case 2:
                if (prevIndex !=2) {
                    points[0] = grabbables[0].transform.position;
                    points[1] = grabbables[0].transform.position + new Vector3(0f, 0.32f, 0f);
                    points[2] = points[1]; points[2].x = phantoms[1].transform.position.x;
                    points[3] = points[2]; points[3].z = phantoms[1].transform.position.z;
                    points[4] = phantoms[1].transform.position;
                    lineRenderer.positionCount = 5;
                    lineRenderer.SetPositions(points);
                    prevIndex = 2;
                }
                break;
            case 3:
                if (prevIndex !=3) {
                    points[0] = grabbables[1].transform.position;
                    points[1] = grabbables[1].transform.position + new Vector3(0f, 0.3f, -0.1f);
                    points[2] = points[1]; points[2].x = phantoms[2].transform.position.x;
                    points[3] = points[2]; points[3].z = phantoms[2].transform.position.z;
                    points[4] = phantoms[2].transform.position;
                    lineRenderer.positionCount = 5;
                    lineRenderer.SetPositions(points);
                    prevIndex = 3;
                }
                break;
            case 4:
                if (prevIndex !=4) {
                    points[0] = grabbables[2].transform.position;
                    points[1] = grabbables[2].transform.position + new Vector3(0.135f, 0f, 0);
                    points[2] = points[1]; points[2].y = phantoms[3].transform.position.y;
                    points[3] = points[2]; points[3].x = phantoms[3].transform.position.x;
                    points[4] = phantoms[3].transform.position;
                    lineRenderer.positionCount = 5;
                    lineRenderer.SetPositions(points);
                    prevIndex = 3;
                }
                break;
        }
    }

    public void ShowPath(int i) {
        index = i;
        //print("Showing path : " + i);
    }
}
