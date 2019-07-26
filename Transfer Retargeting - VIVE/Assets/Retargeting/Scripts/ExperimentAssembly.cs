using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ExperimentAssembly : MonoBehaviour
{
    public GameObject hand;
    public GameObject trackedCube;
    GameObject warpedCube;

    Vector3 initPos;

    public Material activeCube, passiveCube;
    public Material activePhantom, passivePhantom, activePhantomRight, buttonMatActive, buttonMatIdle;

    public GameObject fixedPoint;
    Renderer fixedPointR;

    ViveInput inputScript;
    BodyWarping bwScript;

    GameObject[] grabbables;
    GameObject[] phantoms;
    GameObject[] targets;

    Renderer[] grabbablesR;
    Renderer[] phantomsR;

    Vector3 wOrigin, wTargetReal, wTargetVirtual;
    bool warping;

    int index = 0;
    int step = 0, prevStep = -1;
    bool noWrap;

    void Start()
    {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        targets = GameObject.FindGameObjectsWithTag("Target");

        grabbablesR = new Renderer[grabbables.Length];
        phantomsR = new Renderer[phantoms.Length];
        for (int i = 0; i < grabbables.Length; i++)
        {
            grabbablesR[i] = grabbables[i].GetComponent<Renderer>();
        }
        for (int i = 0; i < phantoms.Length; i++)
        {
            phantomsR[i] = phantoms[i].GetComponent<Renderer>();
        }

        foreach (Renderer r in phantomsR)
        {
            r.material = passivePhantom;
        }
        foreach (Renderer r in grabbablesR)
        {
            r.material = passiveCube;
        }
        fixedPointR = fixedPoint.GetComponent<Renderer>();

        inputScript = hand.GetComponent<ViveInput>();
        bwScript = hand.GetComponent<BodyWarping>();

        warpedCube = trackedCube.transform.GetChild(0).gameObject;
        initPos = warpedCube.transform.position;

        wOrigin = trackedCube.transform.position;
        wTargetReal = trackedCube.transform.position;
        wTargetVirtual = trackedCube.transform.position;
    }

    void Update()
    {
        phantomsR[0].material = activePhantom;
        warpedCube.transform.position = bwScript.BodyWarp(initPos, grabbables[1].transform.position, phantoms[0].transform.position);
        if ((phantoms[0].transform.position- trackedCube.transform.position).magnitude<0.1f)
        {
            print("Success");
        }

        switch (step)
        {
            case 0:
                if ((grabbables[index].transform.position - trackedCube.transform.position).magnitude < 0.1f) {
                    step = 1;
                    prevStep = 0;
                }
                break;
            case 1:
                //if ()
                break;
        }
    }
}
