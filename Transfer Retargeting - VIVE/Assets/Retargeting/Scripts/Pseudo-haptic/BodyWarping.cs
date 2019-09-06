using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BodyWarping : MonoBehaviour {
    public Transform trackedCube;
    public bool warp;
    Color warpColor = new Color(1f, 0f, 0f);
    Color lambdaColor = new Color(0f, 1f, 0f);

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public Vector3 BodyWarp(Vector3 realPosition, Vector3 wOrigin, Vector3 wTargetReal, Vector3 wTargetVirtual) {
        Vector3 realHandPos = realPosition;
        Vector3 d = realHandPos - wTargetReal;
        Vector3 D = wOrigin - wTargetReal;
        Vector3 virtHandPos = realHandPos;

        if (d.magnitude > D.magnitude) {
            warp = false;
            return realHandPos;
        }
        else {
            warp = true;
            Vector3 lambda = wTargetVirtual - wTargetReal;

            virtHandPos = (D.magnitude - d.magnitude) / D.magnitude * lambda + realHandPos;

            //print("d = " + d + ", lambda = " + lambda + ", offset = " + (D.magnitude - d.magnitude) / D.magnitude * lambda);

            Debug.DrawLine(virtHandPos, realHandPos, warpColor);
            //Debug.DrawLine(realHandPos + lambda, realHandPos, lambdaColor);
        }
        return virtHandPos;
    }
}
