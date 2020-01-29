using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BodyWarping : MonoBehaviour {
    Color warpColor = new Color(0.5f, 0.1f, 0.1f);
    Color lambdaColor = new Color(0.1f, 0.1f, 0.5f);
    Color sphereColor = new Color(0.1f, 0.5f, 0.1f);

    public KeyValuePair<Vector3, Vector3> BodyWarp(Vector3 realHandPos, Vector3 wOrigin, Vector3 wTargetReal, Vector3 wTargetVirtual) {
        Vector3 d = realHandPos - wTargetReal;
        float D = (wOrigin - wTargetReal).magnitude;
        Vector3 virtHandPos = realHandPos;

        Vector3 lambda;
        if (d.magnitude > D) {
            return new KeyValuePair<Vector3, Vector3>(realHandPos, Vector3.zero);
        } else {
            lambda = wTargetVirtual - wTargetReal;

            virtHandPos = (D - d.magnitude) / D * lambda + realHandPos;

            Debug.DrawLine(virtHandPos, realHandPos, warpColor);
            //Debug.DrawLine(virtHandPos, realHandPos + lambda, lambdaColor);
        }
        return new KeyValuePair<Vector3, Vector3>(virtHandPos, (D - d.magnitude) / D * lambda);
    }

    public KeyValuePair<Vector3, Vector3> BodyWarpP(Vector3 realHandPos, Vector3 wOrigin, float delta, Vector3 wTargetReal, Vector3 wTargetVirtual) {
        Vector3 d = realHandPos - wTargetReal;
        float D = (wOrigin - wTargetReal).magnitude - delta;
        Vector3 virtHandPos = realHandPos;

        Debug.DrawLine(wTargetReal, wTargetReal + (realHandPos - wTargetReal).normalized*D, sphereColor);

        Vector3 lambda;
        if (d.magnitude > D) {
            return new KeyValuePair<Vector3, Vector3>(realHandPos, Vector3.zero);
        } else {
            lambda = wTargetVirtual - wTargetReal;

            virtHandPos = (D - d.magnitude) / D * lambda + realHandPos;

            //print("d = " + d + ", lambda = " + lambda + ", offset = " + (D.magnitude - d.magnitude) / D.magnitude * lambda);

            Debug.DrawLine(virtHandPos, realHandPos, warpColor);
            //Debug.DrawLine(virtHandPos, realHandPos + lambda, lambdaColor);
        }
        return new KeyValuePair<Vector3, Vector3>(virtHandPos, (D - d.magnitude) / D * lambda);
    }
}
