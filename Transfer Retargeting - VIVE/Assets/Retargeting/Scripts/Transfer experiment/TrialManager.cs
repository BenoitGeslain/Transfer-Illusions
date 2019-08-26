using System.Globalization;
using System;
using System.Diagnostics;
using System.Collections;
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

    GameObject warpedCube;

    //  Reset variables
    Transform clones;
    Vector3[] grabbablesPosition;
    Vector3[] phantomPosition;

    public GameObject fixedPoint;
    Renderer fixedPointR;

    BodyWarping bwScript;
    ExperimentManager experimentManager;
    ScoreManager scoreManager;
    MultipleUduinoManager uduinoScript;

    Vector3 initPos;

    int step = 0, prevStep = -1;
    int index = 0;

    public bool start = false;

    Stopwatch watch;
    DateTime time;
    
    void Start() {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");

        grabbablesR = new Renderer[grabbables.Length];
        for (int i = 0; i < grabbables.Length; i++)
        {
            grabbablesR[i] = grabbables[i].GetComponent<Renderer>();
        }
        for (int i = 0; i < phantoms.Length; i++)
        {
            phantoms[i].GetComponent<Renderer>().enabled = false;
        }

        foreach (Renderer r in grabbablesR)
        {
            r.enabled = false;
        }

        experimentManager = GetComponent<ExperimentManager>();
        bwScript = GetComponent<BodyWarping>();
        scoreManager = GetComponent<ScoreManager>();
        uduinoScript = GameObject.Find("Uduino").GetComponent<MultipleUduinoManager>();

        warpedCube = trackedCube.transform.GetChild(0).gameObject;

        clones = GameObject.Find("/World/Clones").transform;

        grabbablesPosition = new Vector3[grabbables.Length];
        phantomPosition = new Vector3[phantoms.Length];

        for(int i=0; i<grabbables.Length; i++) {
            grabbablesPosition[i] = grabbables[i].transform.position;
        }
        for(int i=0; i<phantoms.Length; i++) {
            phantomPosition[i] = phantoms[i].transform.position;
        }

        watch = new Stopwatch();

        print("INIT::TrialManager::DONE");
    }
    
    void FixedUpdate() {
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
                print((hand.transform.position - fixedPoint.transform.position).magnitude);
                    if (!((warpedCube.transform.position - phantoms[index].transform.position).magnitude < 0.01f) ||
                        !(Quaternion.Angle(trackedCube.transform.rotation, phantoms[index].transform.rotation) < 5f)) { // Produit scalaire entre les forwards (Elodie)
                        step = 0;
                    } else if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.1f) {
                        step = 2;
                    }
                    break;
                case 2:
                    step = 0;
                    break;
            }

            print("Step: " + step + "/" + prevStep);
            time = DateTime.Now;
            experimentManager.LogContinous(time.ToString("HH:mm:ss.fff"), index, trackedCube.transform.position,
                                           trackedCube.transform.eulerAngles, warpedCube.transform.position,
                                           warpedCube.transform.eulerAngles, uduinoScript.GetAcceleration(),
                                           scoreManager.GetScore()); 

            switch (step) {
                case 0:
                    if (prevStep==-1) {
                        print("Starting watch and arduinos");
                        uduinoScript.BroadcastCommand("CountHits", 1);
                        watch.Start();
                    }
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
                        warpedCube.transform.position = bwScript.BodyWarp(initPos, grabbables[index].transform.position,
                                                                          phantoms[index].transform.position);
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
                        warpedCube.transform.position = bwScript.BodyWarp(initPos, grabbables[index].transform.position,
                                                                          phantoms[index].transform.position);
                    }
                    break;
                case 2:
                    if (prevStep == 1) {
                        watch.Stop();
                        uduinoScript.BroadcastCommand("CountHits", 0);

                        print("EXEC::TrialManager::Trial over (Saving and resetting)");
                        scoreManager.AddScoreTime((int)watch.ElapsedMilliseconds/1000);
                        scoreManager.AddScoreCube((warpedCube.transform.position-phantoms[index].transform.position).magnitude);
                        TimeSpan elapsed = watch.Elapsed;
                        experimentManager.LogDiscrete(elapsed.Milliseconds.ToString(), index,
                                                      warpedCube.transform.position - phantoms[index].transform.position,
                                                      Quaternion.Angle( trackedCube.transform.rotation, phantoms[index].transform.rotation),
                                                      0, uduinoScript.GetHitCount(), scoreManager.GetScore());

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

                        if (index==grabbables.Length) {
                            experimentManager.EndTrial();
                        }
                    }
                    break;
            }
        }
    }

    public void ResetScene() {
        step = 0; prevStep = -1; index = 0;

        Material[] tmpMat = phantoms[0].GetComponent<Renderer>().materials;
        tmpMat[1] = phantomMat;
        
        for(int i=0; i<phantoms.Length; i++) {
            phantoms[i].GetComponent<Renderer>().materials = tmpMat;
            phantoms[i].GetComponent<Renderer>().enabled = false;
        }

        // Détruit tous les clones du cube tracké
        foreach(Transform c in clones) {
            Destroy(c.gameObject);
        }

        scoreManager.ResetScore();

        print("RESET::SceneReset::DONE");
    }
}
