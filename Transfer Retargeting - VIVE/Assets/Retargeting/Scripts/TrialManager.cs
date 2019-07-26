using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    public GameObject hand;
    public GameObject trackedCube;
    public GameObject cubePrefab;

    public Material activeCube, passiveCube;
    public Material activePhantom, passivePhantom, activePhantomRight, buttonMatActive, buttonMatIdle;

    GameObject[] grabbables;
    GameObject[] phantoms;
    GameObject[] targets;

    Renderer[] grabbablesR;
    Renderer[] phantomsR;

    GameObject warpedCube;

    public GameObject fixedPoint;
    Renderer fixedPointR;

    ViveInput inputScript;
    BodyWarping bwScript;

    Vector3 initPos;

    bool trialOver;
    int step = 0, prevStep = -1;
    int index = 0;

    bool start = false;
    
    void Start()
    {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");

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

        //inputScript = hand.GetComponent<ViveInput>();
        bwScript = this.GetComponent<BodyWarping>();

        warpedCube = trackedCube.transform.GetChild(0).gameObject;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            start = true;

        if (start)
        {
            switch (step)
            {
                case 0: // Cube is being placed // TODO Add rotation
                    if ((warpedCube.transform.position - phantoms[index].transform.position).magnitude < 0.05f)
                    {
                        step = 1;
                    }
                    break;
                case 1: // Cube placé, attente du bouton
                    if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.05f)
                    {
                        step = 2;
                    }
                    break;
                case 2:
                    step = 0;
                    break;
            }

            print("Step: " + step + "/" + prevStep);

            switch (step)
            {
                case 0:
                    if (prevStep != 0)
                    {
                        initPos = trackedCube.transform.position;
                        warpedCube.GetComponent<Renderer>().enabled = true;
                        phantomsR[index].material = activePhantom;
                        prevStep = 0;
                    }
                    //print(initPos + ", " + grabbables[index+1].transform.position + ", " + phantoms[index].transform.position);
                    //print(grabbables[index + 1] + " " + grabbables[index + 1].transform.position);
                    //print(phantoms[index] + " " + phantoms[index].transform.position);
                    warpedCube.transform.position = bwScript.BodyWarp(initPos, grabbables[index].transform.position, phantoms[index].transform.position);
                    break;
                case 1:
                    if (prevStep == 0)
                    {
                        initPos = fixedPoint.transform.position;
                        phantomsR[index].material = activePhantomRight;
                        prevStep = 1;
                    }
                    warpedCube.transform.position = bwScript.BodyWarp(initPos, grabbables[index].transform.position, phantoms[index].transform.position);
                    break;
                case 2:
                    if (prevStep == 1)
                    {
                        //Deactivate mesh renderer of tracked cube
                        warpedCube.GetComponent<Renderer>().enabled = false;
                        Instantiate(cubePrefab, warpedCube.transform.position, warpedCube.transform.rotation);

                        warpedCube.transform.position = trackedCube.transform.position;

                        index++;
                        prevStep = 2;
                    }
                    break;
            }
        }
    }
}
