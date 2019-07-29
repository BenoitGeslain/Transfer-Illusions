using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// TODO : Use Color instead of renderer to change the phantom's color

public class TrialManager : MonoBehaviour {

    public GameObject hand;
    public GameObject trackedCube;
    public GameObject cubePrefab;

    public Material phantomRightMat, phantomMat;

    public int condition;

    GameObject[] grabbables;
    GameObject[] phantoms;

    Renderer[] grabbablesR;
    Material[] phantomsM;

    GameObject warpedCube;
    Transform clones;

    public GameObject fixedPoint;
    Renderer fixedPointR;

    BodyWarping bwScript;
    ExperimentManager experimentManager;

    Vector3 initPos;

    int step = 0, prevStep = -1;
    int index = 0;

    public bool start = false;

    Color phantomColor = new Color(255f, 70f, 70f, 112f), phantomRightColor = new Color(30f, 255f, 30f, 112f);
    
    void Start() {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");

        grabbablesR = new Renderer[grabbables.Length];
        phantomsM = new Material[phantoms.Length];
        for (int i = 0; i < grabbables.Length; i++)
        {
            grabbablesR[i] = grabbables[i].GetComponent<Renderer>();
        }
        for (int i = 0; i < phantoms.Length; i++)
        {
            phantoms[i].GetComponent<Renderer>().enabled = false;
        }

        /*foreach (Renderer r in phantomsR)
        {
            r.enabled = false;
        }*/
        foreach (Renderer r in grabbablesR)
        {
            r.enabled = false;
        }
        fixedPointR = fixedPoint.GetComponent<Renderer>();

        experimentManager = this.GetComponent<ExperimentManager>();
        bwScript = this.GetComponent<BodyWarping>();

        warpedCube = trackedCube.transform.GetChild(0).gameObject;

        print("INIT::TrialManager::DONE");
    }
    
    void Update() {
        if (start)
        {
            switch (step)
            {
                case 0: // Cube is being placed // TODO Add rotation
                    if ((warpedCube.transform.position - phantoms[index].transform.position).magnitude < 0.01f &&
                        Quaternion.Angle( trackedCube.transform.rotation, phantoms[index].transform.rotation) < 5f) {
                        step = 1;
                    }
                    break;
                case 1: // Cube placé, attente du bouton
                    if (!((warpedCube.transform.position - phantoms[index].transform.position).magnitude < 0.01f) ||
                        !(Quaternion.Angle(trackedCube.transform.rotation, phantoms[index].transform.rotation) < 5f)) { // Produit scalaire entre les forwards (Elodie)
                        step = 0;
                    } else if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.05f) {
                        step = 2;
                        experimentManager.LogDiscrete(index, warpedCube.transform.position - phantoms[index].transform.position, Quaternion.Angle( trackedCube.transform.rotation, phantoms[index].transform.rotation), 0);
                    }
                    break;
                case 2:
                    if (index==grabbables.Length) {
                        index = 0;
                        step = 0;
                    }
                    step = 0;
                    break;
            }

            print("Step: " + step + "/" + prevStep);

            experimentManager.LogContinous(DateTime.Now.ToString("HH:mm:ss"), index, trackedCube.transform.position,
                                           trackedCube.transform.eulerAngles, warpedCube.transform.position, warpedCube.transform.eulerAngles); 

            switch (step) {
                case 0:
                    if (prevStep == -1 || prevStep == 2) {
                        initPos = trackedCube.transform.position;
                        warpedCube.GetComponent<Renderer>().enabled = true;
                        phantoms[index].GetComponent<Renderer>().enabled = true;
                        prevStep = 0;
                    } else if (prevStep == 1) {
                        Material[] tmpMat = phantoms[index].GetComponent<Renderer>().materials;
                        tmpMat[1] = phantomMat;
                        phantoms[index].GetComponent<Renderer>().materials = tmpMat;
                        prevStep = 0;
                    }
                    //print(initPos + ", " + grabbables[index+1].transform.position + ", " + phantoms[index].transform.position);
                    //print(grabbables[index + 1] + " " + grabbables[index + 1].transform.position);
                    //print(phantoms[index] + " " + phantoms[index].transform.position);
                    if (condition == (int)Condition.VBW)
                        warpedCube.transform.position = bwScript.BodyWarp(initPos, grabbables[index].transform.position, phantoms[index].transform.position);
                        print("warping");
                    break;
                case 1:
                    if (prevStep == 0) {
                        initPos = fixedPoint.transform.position;
                        Material[] tmpMat = phantoms[index].GetComponent<Renderer>().materials;
                        tmpMat[1] = phantomRightMat;
                        phantoms[index].GetComponent<Renderer>().materials = tmpMat;
                        prevStep = 1;
                    }
                    if (condition == (int)Condition.VBW) {
                        print("warping");
                        warpedCube.transform.position = bwScript.BodyWarp(initPos, grabbables[index].transform.position, phantoms[index].transform.position);
                    }
                    break;
                case 2:
                    if (prevStep == 1) {
                        if (condition == (int)Condition.VBW) {
                            //Deactivate mesh renderer of tracked cube
                            warpedCube.GetComponent<Renderer>().enabled = false;
                            phantoms[index].GetComponent<Renderer>().enabled = false;

                            GameObject tmp = Instantiate(cubePrefab, warpedCube.transform.position, warpedCube.transform.rotation);
                            tmp.transform.parent = clones;

                            warpedCube.transform.position = trackedCube.transform.position;
                        }

                        index++;
                        prevStep = 2;
                    }
                    break;
            }
        }
    }
}
