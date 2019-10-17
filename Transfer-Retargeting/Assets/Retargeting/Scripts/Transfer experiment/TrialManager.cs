using System.Globalization;
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Use Color instead of renderer to change the phantom's color

public class TrialManager : MonoBehaviour {

    public GameObject hand;
    public Transform armHandMetaphor, armHandTracked;
    public GameObject trackedCube;
    public GameObject cubePrefab;

    public Material phantomRightMat, phantomMat;

    public int collisions;

    int condition;

    int[] col;

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
    PathManager pathScript;

    AudioSource collisionSource;

    Vector3 initPos;
    Vector3 cubePrevPos, handPrevPos;
    float cubeDistGone, handDistGone;

    int step = 0, prevStep = -1;
    int index = 0;

    public bool start = false, log = false;
    public bool pause = false;
    public bool nextCube = false;
    bool paused = false;

    Stopwatch watch;
    DateTime time, startTrialTime;
    
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
        pathScript = GetComponent<PathManager>();

        collisionSource = GetComponent<AudioSource>();


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

        col = new int[4];

        cubePrevPos = warpedCube.transform.position;
        handPrevPos = hand.transform.position;
        cubeDistGone = 0f;
        handDistGone = 0f;

        if (!log) {
            print("WARNING : Logging is disabled !!");
        }

        watch = new Stopwatch();

        print("INIT::TrialManager::DONE");
    }
    
    void FixedUpdate() {
        if (start) {
            pathScript.ShowPath(index+1);

            switch (step) {
                case 0: // Cube is being placed
                    if (((warpedCube.transform.position - phantoms[index].transform.position).magnitude < 0.025f &&
                        Quaternion.Angle( trackedCube.transform.rotation, phantoms[index].transform.rotation) < 10f) || nextCube) {
                        step = 1;
                    }
                    break;
                case 1: // Cube placé, attente du bouton
                	if (nextCube) {
                		print("step = 2");
                		step = 2;
                		break;
                	}
                    if (!((warpedCube.transform.position - phantoms[index].transform.position).magnitude < 0.025f) ||
                        !(Quaternion.Angle(trackedCube.transform.rotation, phantoms[index].transform.rotation) < 10f)) {
                        step = 0;
                    } else if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.07f) {
                        step = 2;
                    }
                    break;
                case 2:
                	nextCube = false;
                    step = 0;
                    break;
            }

            switch (step) {
                case 0:
                	if (!pause) {
	                    if (paused && (prevStep != -1)) {
	                    	print("Game resuming");
	                    	paused = false;
	                    	watch.Restart();
	                    }

                		if (prevStep==-1) {
	                    	initPos = trackedCube.transform.position;
	                        warpedCube.GetComponent<Renderer>().enabled = true;
	                        phantoms[index].GetComponent<Renderer>().enabled = true;
					        cubePrevPos = warpedCube.transform.position;
					        handPrevPos = hand.transform.position;
	                        prevStep = 0;
	                        col = new int[4];
	                        print("Starting watch and arduinos");
	                        uduinoScript.BroadcastCommand("CountHits", 1);
	                        startTrialTime = DateTime.Now;
	                        watch = new Stopwatch();
	                        watch.Start();
	                    } else if (prevStep == 1) {
	                        Material[] tmpMat = phantoms[index].GetComponent<Renderer>().materials;
	                        tmpMat[1] = phantomMat;
	                        phantoms[index].GetComponent<Renderer>().materials = tmpMat;
	                        prevStep = 0;
	                    }
	                    
	                    if (condition == (int)Condition.VBW) {
	                        bwScript.warp = true;
	                        warpedCube.transform.position = bwScript.BodyWarp(trackedCube.transform.position, initPos, grabbables[index].transform.position,
	                                                                          phantoms[index].transform.position);
	                        for (int i = 0; i<armHandMetaphor.childCount; i++) {
	                            armHandMetaphor.GetChild(i).position = bwScript.BodyWarp(armHandTracked.GetChild(i).position, initPos, grabbables[index].transform.position,
	                                                                          phantoms[index].transform.position);
	                            armHandMetaphor.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
	                        }
	                    } else {
	                        bwScript.warp = false;
	                    }
                	} else {
                		if (!paused) {
                			print("Game paused");
	                    	paused = true;
                		}
                	}
                    
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
                        bwScript.warp = true;
                        warpedCube.transform.position = bwScript.BodyWarp(trackedCube.transform.position, initPos, grabbables[index].transform.position,
                                                                          phantoms[index].transform.position);
                        for (int i = 0; i<armHandMetaphor.childCount; i++) {
                            armHandMetaphor.GetChild(i).position = bwScript.BodyWarp(armHandTracked.GetChild(i).position, initPos, grabbables[index].transform.position,
                                                                                     phantoms[index].transform.position);
                            armHandMetaphor.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
                        }
                    } else {
                        bwScript.warp = false;
                    }
                    break;
                case 2:
                    if (prevStep == 1) {
                        watch.Stop();
                        uduinoScript.BroadcastCommand("CountHits", 0);
                        TimeSpan elapsed = watch.Elapsed;

                        print("EXEC::TrialManager::Operation over (Saving and resetting)");
                        scoreManager.AddScoreTime((int)watch.ElapsedMilliseconds/1000);
                        scoreManager.AddScoreCube((warpedCube.transform.position-phantoms[index].transform.position).magnitude);
                        if (log) {
                            experimentManager.LogDiscrete(elapsed.TotalSeconds.ToString(), startTrialTime.ToString("HH:mm:ss.fff"), index,
                                                          warpedCube.transform.position, warpedCube.transform.eulerAngles, 
                                                          phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                                          cubeDistGone, handDistGone,
                                                          uduinoScript.GetHitCount(), col, scoreManager.GetScore());
                        }

                        cubeDistGone = 0f;
                        handDistGone = 0f;

                        phantoms[index].GetComponent<Renderer>().enabled = false;
                        if (condition == (int)Condition.VBW) {
                            //Deactivate mesh renderer of tracked cube
                            bwScript.warp = false;
                            warpedCube.GetComponent<Renderer>().enabled = false;

                            GameObject tmp = Instantiate(cubePrefab, warpedCube.transform.position, warpedCube.transform.rotation);
                            tmp.transform.parent = clones;

                            warpedCube.transform.position = trackedCube.transform.position;
                        }

                        index++;
                        prevStep = -1;

                        print("Index : " + index + "/" + grabbables.Length);
                        if (index==grabbables.Length) {
                            experimentManager.EndTrial();
                        }
                    }
                    break;
            }

            if (collisions!=0) {
                if (!collisionSource.isPlaying)
                    collisionSource.Play();
                collisions=0;
            	col[index]++;
            }

            if (!pause) {
            	if (!paused) {
            		cubeDistGone += (warpedCube.transform.position - cubePrevPos).magnitude;
            		handDistGone += (hand.transform.position - handPrevPos).magnitude;
            	}
        		cubePrevPos = warpedCube.transform.position;
        		handPrevPos = hand.transform.position;
            }

            print("Step: " + step + "/" + prevStep);
            time = DateTime.Now;
            if (log) {
                experimentManager.LogContinous(time.ToString("HH:mm:ss.fff"), index,
                							   trackedCube.transform.position, trackedCube.transform.eulerAngles,
                							   warpedCube.transform.position, warpedCube.transform.eulerAngles,
                							   phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                               cubeDistGone, handDistGone,
                                               uduinoScript.GetAcceleration(), col,
                                               scoreManager.GetScore(), pause);
            }
        }

    }

    public void SetCondition(int c) {
        condition = c;
    }

    public void ResetScene(bool resetScore) {
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

        if (resetScore)
            scoreManager.ResetScore();

        print("RESET::SceneReset::DONE");
    }
}
