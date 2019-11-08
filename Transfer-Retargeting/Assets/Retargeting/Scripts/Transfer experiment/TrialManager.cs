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
    public GameObject cubePrefab;

    public Material phantomRightMat, phantomMat, cubePassive;

    public int collisions = 0;

    public AudioClip bump, coin, fire;

    int condition;

    int[] col;

    GameObject[] grabbables;
    GameObject[] phantoms;

    Renderer[] grabbablesR;

    GameObject[] warpedCubes;
    GameObject[] physicalCubes;

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

    public bool start = false;
    public bool pause = false;
    public bool nextCube = false;
    bool paused = false;
    bool soundPlayed = false;
    bool skippable = true;


    KeyValuePair<Vector3, Vector3> result;
    bool warping = false;

    int N;

    Stopwatch watch, wSkippable;
    DateTime time, startTrialTime;
    
    void Start() {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");

        grabbablesR = new Renderer[grabbables.Length];
        for (int i = 0; i < grabbables.Length; i++) {
            grabbablesR[i] = grabbables[i].GetComponent<Renderer>();
        }
        for (int i = 0; i < phantoms.Length; i++) {
            phantoms[i].GetComponent<Renderer>().enabled = false;
        }

        foreach (Renderer r in grabbablesR) {
            r.enabled = false;
        }
        N = grabbables.Length;

        experimentManager = GetComponent<ExperimentManager>();
        bwScript = GetComponent<BodyWarping>();
        scoreManager = GetComponent<ScoreManager>();
        uduinoScript = GameObject.Find("Uduino").GetComponent<MultipleUduinoManager>();
        pathScript = GetComponent<PathManager>();
        GetComponent<SceneConfiguration>().saveConfig = true;

        collisionSource = GetComponent<AudioSource>();

        physicalCubes = GameObject.FindGameObjectsWithTag("PhysicalCubes");
        warpedCubes = GameObject.FindGameObjectsWithTag("WarpedCubes");
        if (physicalCubes.Length != N || warpedCubes.Length != N) {
            print("ERROR::Incorrect number of cubes");
        }

        clones = GameObject.Find("/World/Clones Cubes").transform;

        grabbablesPosition = new Vector3[grabbables.Length];
        phantomPosition = new Vector3[phantoms.Length];

        for(int i=0; i<grabbables.Length; i++) {
            grabbablesPosition[i] = grabbables[i].transform.position;
        }
        for(int i=0; i<phantoms.Length; i++) {
            phantomPosition[i] = phantoms[i].transform.position;
        }

        col = new int[N];

        cubePrevPos = warpedCubes[0].transform.position;
        handPrevPos = hand.transform.position;
        cubeDistGone = 0f;
        handDistGone = 0f;

        watch = new Stopwatch();

        print("INIT::TrialManager::DONE");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            foreach (GameObject g in warpedCubes)
                g.GetComponent<Renderer>().enabled = true;
        }

    }
    
    void FixedUpdate() {
        if (start) {
            //pathScript.ShowPath(index+1);

            switch (step) {
                case 0: // Cube is being placed
                    if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                        if ((warpedCubes[0].transform.position - phantoms[index].transform.position).magnitude < 0.022f &&
                            Quaternion.Angle( warpedCubes[0].transform.rotation, phantoms[index].transform.rotation) < 10f) {
                            step = 1;

                            if (!soundPlayed) {
                                collisionSource.Stop();
                                soundPlayed = true;
                                collisionSource.clip = coin;
                                collisionSource.Play();
                            }
                        }
                    } else {
                        if ((warpedCubes[index].transform.position - phantoms[index].transform.position).magnitude < 0.022f &&
                            Quaternion.Angle( warpedCubes[index].transform.rotation, phantoms[index].transform.rotation) < 10f) {
                            step = 1;

                            if (!soundPlayed) {
                                collisionSource.Stop();
                                soundPlayed = true;
                                collisionSource.clip = coin;
                                collisionSource.Play();
                            }
                        }
                    }

                    /*if ((condition == (int)Condition.RW4 || condition == (int)Condition.RW1) && skippable) {
                        if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.075f) {
                            collisionSource.Stop();
                            collisionSource.clip = fire;
                            collisionSource.Play();

                            wSkippable = new Stopwatch();
                            wSkippable.Start();
                            skippable = false;

                            step = 2;
                        }
                    }*/
                    break;
                case 1: // Cube placé, attente du bouton
                	if (nextCube) {
                		step = 2;
                		break;
                	}
                    if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                        if (!((warpedCubes[0].transform.position - phantoms[index].transform.position).magnitude < 0.022f) ||
                            !(Quaternion.Angle(warpedCubes[0].transform.rotation, phantoms[index].transform.rotation) < 10f)) {
                            step = 0;
                            soundPlayed = false;
                        } else if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.075f) {
                            collisionSource.Stop();
                            collisionSource.clip = fire;
                            collisionSource.Play();

                            step = 2;
                        }
                    } else {
                        if (!((warpedCubes[index].transform.position - phantoms[index].transform.position).magnitude < 0.022f) ||
                            !(Quaternion.Angle(warpedCubes[index].transform.rotation, phantoms[index].transform.rotation) < 10f)) {
                            step = 0;
                            soundPlayed = false;
                        } else if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.075f) {
                            collisionSource.Stop();
                            collisionSource.clip = fire;
                            collisionSource.Play();

                            step = 2;
                        }
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
	                    if (paused) {
	                    	print("Game resuming");
	                    	paused = false;
	                    	prevStep = -1;
	                    }

                		if (prevStep==-1) {
	                        phantoms[index].GetComponent<Renderer>().enabled = true;
	                        if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                                initPos = warpedCubes[0].transform.position;
                                warpedCubes[0].GetComponent<Renderer>().enabled = true;
                                cubePrevPos = warpedCubes[0].transform.position;
                            } else {
                                foreach (GameObject t in warpedCubes) {
                                    t.GetComponent<Renderer>().enabled = false;
                                }
                                warpedCubes[index].GetComponent<Renderer>().enabled = true;
                                cubePrevPos = warpedCubes[index].transform.position;
                            }

					        handPrevPos = hand.transform.position;
	                        prevStep = 0;
	                        print("Starting watch and arduinos");
                            watch = new Stopwatch();
                            uduinoScript.BroadcastCommand("CountHits", 1);
	                        startTrialTime = DateTime.Now;
	                        watch.Start();
	                    } else if (prevStep == 1) {
	                        Material[] tmpMat = phantoms[index].GetComponent<Renderer>().materials;
	                        tmpMat[1] = phantomMat;
	                        phantoms[index].GetComponent<Renderer>().materials = tmpMat;
	                        prevStep = 0;
	                    }
	                    if (condition == (int)Condition.VBW) {
	                        result = bwScript.BodyWarp(physicalCubes[0].transform.position, initPos, grabbables[index].transform.position,
	                                                   phantoms[index].transform.position);
                            warpedCubes[0].transform.position = result.Key;
	                        for (int i = 0; i<armHandMetaphor.childCount; i++) {
	                            armHandMetaphor.GetChild(i).position = armHandTracked.GetChild(i).position + result.Value;
	                            armHandMetaphor.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
	                    	}
                            warping = result.Value != Vector3.zero;
	                    }
                	} else {
                		if (!paused) {
                			print("Game paused");
	                    	paused = true;
                            watch.Stop();
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
                        result = bwScript.BodyWarp(physicalCubes[0].transform.position, initPos, grabbables[index].transform.position,
                                                   phantoms[index].transform.position);
                        warpedCubes[0].transform.position = result.Key;
                        warping = result.Value != Vector3.zero;
                        for (int i = 0; i<armHandMetaphor.childCount; i++) {
                            result = bwScript.BodyWarp(armHandTracked.GetChild(i).position, initPos, grabbables[index].transform.position,
                                                       phantoms[index].transform.position);
                            armHandMetaphor.GetChild(i).position = armHandTracked.GetChild(i).position + result.Value;
                            armHandMetaphor.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
                        }
                    }
                    break;
                case 2:
                    if (prevStep == 1) {
                        watch.Stop();
                        uduinoScript.BroadcastCommand("CountHits", 0);
                        TimeSpan elapsed = watch.Elapsed;

                        print("EXEC::TrialManager::Operation over (Saving and resetting)");
                        scoreManager.AddScoreTime((int)watch.Elapsed.TotalSeconds);

                        if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                            scoreManager.AddScoreCube((warpedCubes[0].transform.position-phantoms[index].transform.position).magnitude);
                        	experimentManager.LogDiscrete(elapsed.TotalSeconds.ToString(),
                        								  startTrialTime.ToString("HH:mm:ss.fff"), DateTime.Now.ToString("HH:mm:ss.fff"),
                        								  index,
                                                          physicalCubes[0].transform.position, physicalCubes[0].transform.eulerAngles,
                                                          warpedCubes[0].transform.position, warpedCubes[0].transform.eulerAngles,
                                                          phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                                          cubeDistGone, handDistGone,
                                                          uduinoScript.GetHitCount(), col, scoreManager.GetScore());
                    	} else {
                            scoreManager.AddScoreCube((warpedCubes[index].transform.position-phantoms[index].transform.position).magnitude);
                    		experimentManager.LogDiscrete(elapsed.TotalSeconds.ToString(),
                        								  startTrialTime.ToString("HH:mm:ss.fff"), DateTime.Now.ToString("HH:mm:ss.fff"),
                        								  index,
                                                          physicalCubes[index].transform.position, physicalCubes[index].transform.eulerAngles,
                                                          warpedCubes[index].transform.position, warpedCubes[index].transform.eulerAngles, 
                                                          phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                                          cubeDistGone, handDistGone,
                                                          uduinoScript.GetHitCount(), col, scoreManager.GetScore());
                    	}

                        cubeDistGone = 0f;
                        handDistGone = 0f;

                        phantoms[index].GetComponent<Renderer>().enabled = false;
                        if (condition == (int)Condition.VBW) {
                            //Deactivate mesh renderer of tracked cube
                            warpedCubes[0].GetComponent<Renderer>().enabled = false;

                            GameObject tmp = Instantiate(cubePrefab, warpedCubes[0].transform.position, warpedCubes[0].transform.rotation);
                            tmp.transform.parent = clones;
                            warpedCubes[0].transform.localPosition = Vector3.zero;
                        } else if (condition == (int)Condition.V) {
                            warpedCubes[index].GetComponent<Renderer>().enabled = false;

                            GameObject tmp = Instantiate(cubePrefab, warpedCubes[index].transform.position, warpedCubes[index].transform.rotation);
                        }

                        index++;
                        prevStep = -1;

                        if (index==grabbables.Length) {
                            pause = true;
                            experimentManager.EndTrial();
                        }
                    }
                    break;
            }

            if (!pause) {
                if (collisions!=0) {
                    if (!collisionSource.isPlaying) {
                        collisionSource.clip = bump;
                        collisionSource.Play();
                    } 
                    collisions=0;
                	col[index] += 1;
                }

                if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                    cubeDistGone += (warpedCubes[0].transform.position - cubePrevPos).magnitude;
                    cubePrevPos = warpedCubes[0].transform.position;
                } else {
                    cubeDistGone += (warpedCubes[index].transform.position - cubePrevPos).magnitude;
                    cubePrevPos = warpedCubes[index].transform.position;
                }
        		handDistGone += (hand.transform.position - handPrevPos).magnitude;
                handPrevPos = hand.transform.position;
            }

            print("Step: " + step + "/" + prevStep + ". Index: " + index + "/" + grabbables.Length);
            time = DateTime.Now;
            if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                experimentManager.LogContinous(time.ToString("HH:mm:ss.fff"), index,
                                               physicalCubes[0].transform.position, physicalCubes[0].transform.eulerAngles,
                                               warpedCubes[0].transform.position, warpedCubes[0].transform.eulerAngles,
                                               phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                               warping,
                                               cubeDistGone, handDistGone,
                                               uduinoScript.GetAcceleration(), col,
                                               scoreManager.GetScore(), pause);
            } else {
                experimentManager.LogContinous(time.ToString("HH:mm:ss.fff"), index,
                                               physicalCubes[index].transform.position, physicalCubes[index].transform.eulerAngles,
                                               warpedCubes[index].transform.position, warpedCubes[index].transform.eulerAngles,
                                               phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                               warping,
                                               cubeDistGone, handDistGone,
                                               uduinoScript.GetAcceleration(), col,
                                               scoreManager.GetScore(), pause);
            }
        }
    }

    public bool IsWarping() {
        return warping;
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

        foreach(GameObject c in warpedCubes) {
            c.GetComponent<Renderer>().enabled = false;
        }

        // Détruit tous les clones du cube tracké
        foreach(Transform c in clones) {
            Destroy(c.gameObject);
        }

        if (resetScore)
            scoreManager.ResetScore();

        col = new int[N];

        print("RESET::SceneReset::DONE");
    }
}
