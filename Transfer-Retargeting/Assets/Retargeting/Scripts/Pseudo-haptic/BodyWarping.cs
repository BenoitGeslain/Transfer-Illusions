using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BodyWarping : MonoBehaviour {
    public Transform trackedCube;
    
    Color warpColor = new Color(0.5f, 0.1f, 0.1f);
    Color lambdaColor = new Color(0.1f, 0.1f, 0.5f);

    public KeyValuePair<Vector3, Vector3> BodyWarp(Vector3 realHandPos, Vector3 wOrigin, Vector3 wTargetReal, Vector3 wTargetVirtual) {
        Vector3 d = realHandPos - wTargetReal;
        Vector3 D = wOrigin - wTargetReal;
        Vector3 virtHandPos = realHandPos;

        Vector3 lambda;
        if (d.magnitude > D.magnitude) {
            return new KeyValuePair<Vector3, Vector3>(realHandPos, Vector3.zero);
        } else {
            lambda = wTargetVirtual - wTargetReal;

            virtHandPos = (D.magnitude - d.magnitude) / D.magnitude * lambda + realHandPos;

            //print("d = " + d + ", lambda = " + lambda + ", offset = " + (D.magnitude - d.magnitude) / D.magnitude * lambda);

            Debug.DrawLine(realHandPos, virtHandPos, warpColor);
            Debug.DrawLine(virtHandPos, realHandPos + lambda, lambdaColor);
        }
        return new KeyValuePair<Vector3, Vector3>(virtHandPos, (D.magnitude - d.magnitude) / D.magnitude * lambda);
    }
}
