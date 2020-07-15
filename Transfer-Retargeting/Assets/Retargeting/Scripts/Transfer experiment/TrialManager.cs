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

    public Material phantomRightMat, phantomMat, cubePassive, cubeLogo;

    public int collisions = 0;

    public AudioClip bump, coin, fire;

    public Material[] logosMat;

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
    ScreenManager screenManager;
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
    bool crRunning = false;
    bool buttonTimer = true;

    Vector3 prevPosition;
    float sumVelocity;
    float velocity;
    int nbVelocity;
    float cubeTime, cubeTotalTime;
    int nbError;

    KeyValuePair<Vector3, Vector3> result;
    bool warping = false;

    int N;

    Stopwatch watch, cubeWatch;
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

        /*foreach (Renderer r in grabbablesR) {
            r.enabled = false;
        }*/

        /*foreach (Renderer r in grabbablesR) {
            Material[] tmpMat = r.materials;
            tmpMat[1] = cubePassive;
            r.materials = tmpMat;
        }*/
        //grabbablesR[0].enabled = false;
        N = grabbables.Length;

        experimentManager = GetComponent<ExperimentManager>();
        bwScript = GetComponent<BodyWarping>();
        screenManager = GetComponent<ScreenManager>();
        scoreManager = GetComponent<ScoreManager>();
        uduinoScript = GameObject.Find("Uduino").GetComponent<MultipleUduinoManager>();
        pathScript = GetComponent<PathManager>();
        // GetComponent<SceneConfiguration>().saveConfig = true;

        collisionSource = GetComponent<AudioSource>();

        physicalCubes = GameObject.FindGameObjectsWithTag("PhysicalCubes");
        warpedCubes = GameObject.FindGameObjectsWithTag("WarpedCubes");
        if (physicalCubes.Length != N || warpedCubes.Length != N) {
            print("ERROR::Incorrect number of cubes" + N + " " + physicalCubes.Length + " " + warpedCubes.Length);
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
        cubeWatch = new Stopwatch();

        print("INIT::TrialManager::DONE");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            foreach (GameObject g in warpedCubes) 
                g.GetComponent<Renderer>().enabled = true;
        }

        if (start && Input.GetKeyDown(KeyCode.Keypad0)) {
            pause = false;
            nextCube = true;
        }
    }
    
    void FixedUpdate() {
        if (start) {
            if (pause && nextCube)
                nextCube = false;
            //pathScript.ShowPath(index+1);
            switch (step) {
                case 0: // Cube is being placed
                    if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                        if (((warpedCubes[0].transform.position - phantoms[index].transform.position).magnitude < 0.022f &&
                            Quaternion.Angle( warpedCubes[0].transform.rotation, phantoms[index].transform.rotation) < 10f) || nextCube) {
                            step = 1;
                        }
                    } else if (condition == (int)Condition.RW4 || condition == (int)Condition.V) {
                        if ((((warpedCubes[index].transform.position - phantoms[index].transform.position).magnitude < 0.022f) &&
                            Quaternion.Angle(warpedCubes[index].transform.rotation, phantoms[index].transform.rotation) < 10f) || nextCube) {
                            step = 1;
                        }
                    }

                    if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.075f && buttonTimer) {
                        collisionSource.Stop();
                        collisionSource.clip = fire;
                        collisionSource.Play();

                        if (!pause) {
                            screenManager.goRed();
                            StartCoroutine("Timer");
                            nbError++;
                        }
                    }

                    break;
                case 1: // Cube placé, attente du bouton
                    if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                        if ((!((warpedCubes[0].transform.position - phantoms[index].transform.position).magnitude < 0.022f) ||
                            !(Quaternion.Angle(warpedCubes[0].transform.rotation, phantoms[index].transform.rotation) < 10f)) && !nextCube) {
                            step = 0;
                            soundPlayed = false;
                        } else if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.075f || nextCube) {
                            collisionSource.Stop();
                            collisionSource.clip = coin;
                            collisionSource.Play();

                            screenManager.goGreen();
                            StartCoroutine("Timer");

                            step = 2;
                        }
                    } else if (condition == (int)Condition.RW4) {
                        if ((!((warpedCubes[index].transform.position - phantoms[index].transform.position).magnitude < 0.022f) ||
                            !(Quaternion.Angle(warpedCubes[index].transform.rotation, phantoms[index].transform.rotation) < 10)) && !nextCube) {
                            step = 0;
                            soundPlayed = false;
                        } else if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.075f || nextCube) {
                            collisionSource.Stop();
                            collisionSource.clip = coin;
                            collisionSource.Play();

                            screenManager.goGreen();
                            StartCoroutine("Timer");

                            step = 2;
                        }
                    }
                    break;
                case 2:
                    step = 0;
                    break;
            }

            switch (step) {
                case 0:
                    if (!crRunning && pause) {
                        if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.075f) {
                            scoreManager.UpdatePause(0);
                            screenManager.Pause(0);
                            pause = false;
                        }
                    }
                    if (!pause) {
                        if (paused) {
                            print("Game resuming");
                            paused = false;
                            scoreManager.UpdatePause(0);
                            screenManager.Pause(0);
                            prevStep = -1;
                        }

                        if (prevStep==-1) {
                            phantoms[index].GetComponent<Renderer>().enabled = true;
                            if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                                initPos = warpedCubes[0].transform.position;
                                warpedCubes[0].GetComponent<Renderer>().enabled = true;
                                Material[] tmpMat = warpedCubes[0].GetComponent<Renderer>().materials;
                                tmpMat[0] = logosMat[0];
                                warpedCubes[0].GetComponent<Renderer>().materials = tmpMat;
                                cubePrevPos = warpedCubes[0].transform.position;
                            } else {
                                foreach (GameObject g in warpedCubes) {
                                    g.GetComponent<Renderer>().enabled = false;
                                }
                                warpedCubes[index].GetComponent<Renderer>().enabled = true;
                                cubePrevPos = warpedCubes[index].transform.position;
                            }

                            sumVelocity = 0f;
                            nbVelocity = 0;

                            handPrevPos = hand.transform.position;
                            prevStep = 0;
                            print("Starting watch and arduinos");
                            watch = new Stopwatch();
                            uduinoScript.BroadcastCommand("CountHits", 1);
                            startTrialTime = DateTime.Now;
                            watch.Start();
                        } else if (prevStep == 1) {
                            Material[] tmpMat = phantoms[index].GetComponent<Renderer>().materials;
                            tmpMat[0] = phantomMat;
                            phantoms[index].GetComponent<Renderer>().materials = tmpMat;
                            prevStep = 0;
                        }
                        if (condition == (int)Condition.VBW) {
                            result = bwScript.BodyWarpP(physicalCubes[0].transform.position, initPos, 0.025f, grabbables[index].transform.position,
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
                            scoreManager.UpdatePause(1);
                            screenManager.Pause(1);
                            watch.Stop();
                        }
                    }
                    
                    break;
                case 1:
                    if (prevStep == 0) {
                        initPos = fixedPoint.transform.position;
                        // Material[] tmpMat = phantoms[index].GetComponent<Renderer>().materials;
                        // tmpMat[0] = phantomRightMat;
                        // phantoms[index].GetComponent<Renderer>().materials = tmpMat;
                        prevStep = 1;
                    }   
                    if (condition == (int)Condition.VBW) {
                        result = bwScript.BodyWarpP(physicalCubes[0].transform.position, initPos, 0.025f, grabbables[index].transform.position,
                                                   phantoms[index].transform.position);
                        warpedCubes[0].transform.position = result.Key;
                        warping = result.Value != Vector3.zero;
                        for (int i = 0; i<armHandMetaphor.childCount; i++) {
                            result = bwScript.BodyWarpP(armHandTracked.GetChild(i).position, initPos, 0.025f, grabbables[index].transform.position,
                                                       phantoms[index].transform.position);
                            armHandMetaphor.GetChild(i).position = armHandTracked.GetChild(i).position + result.Value;
                            armHandMetaphor.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
                        }
                    }
                    break;
                case 2:
                    watch.Stop();
                    cubeTime += (float)watch.Elapsed.TotalSeconds;

                    nextCube = false;
                    uduinoScript.BroadcastCommand("CountHits", 0);
                    TimeSpan elapsed = watch.Elapsed;

                    print("EXEC::TrialManager::Operation over (Saving and resetting)");
                    scoreManager.AddScoreTime((int)watch.Elapsed.TotalSeconds, pause);
                    if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                        scoreManager.AddScoreCube((warpedCubes[0].transform.position-phantoms[index].transform.position).magnitude, pause);
                        experimentManager.LogDiscrete(elapsed.TotalSeconds.ToString(),
                                                      startTrialTime.ToString("HH:mm:ss.fff"), DateTime.Now.ToString("HH:mm:ss.fff"),
                                                      index,
                                                      physicalCubes[0].transform.position, physicalCubes[0].transform.eulerAngles,
                                                      warpedCubes[0].transform.position, warpedCubes[0].transform.eulerAngles,
                                                      phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                                      sumVelocity/nbVelocity, cubeTime, nbError,
                                                      cubeDistGone, handDistGone,
                                                      uduinoScript.GetHitCount(), col, scoreManager.GetScore());
                    } else {
                        scoreManager.AddScoreCube((warpedCubes[index].transform.position-phantoms[index].transform.position).magnitude, pause);
                        experimentManager.LogDiscrete(elapsed.TotalSeconds.ToString(),
                                                      startTrialTime.ToString("HH:mm:ss.fff"), DateTime.Now.ToString("HH:mm:ss.fff"),
                                                      index,
                                                      physicalCubes[index].transform.position, physicalCubes[index].transform.eulerAngles,
                                                      warpedCubes[index].transform.position, warpedCubes[index].transform.eulerAngles, 
                                                      phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                                      sumVelocity/nbVelocity, cubeTime, nbError,
                                                      cubeDistGone, handDistGone,
                                                      uduinoScript.GetHitCount(), col, scoreManager.GetScore());
                    }

                    phantoms[index].GetComponent<Renderer>().enabled = false;
                    if (condition == (int)Condition.VBW) {
                        //Deactivate mesh renderer of tracked cube
                        warpedCubes[0].GetComponent<Renderer>().enabled = false;

                        GameObject tmp = Instantiate(cubePrefab, warpedCubes[0].transform.position, warpedCubes[0].transform.rotation);
                        tmp.gameObject.name = "Warped Cube (Clone) " + clones.childCount.ToString();
                        tmp.transform.parent = clones;

                        Material[] tmpMat = new Material[2];
                        tmpMat[0] = logosMat[index];
                        tmpMat[1] = phantomMat;
                        tmp.GetComponent<Renderer>().materials = tmpMat;

                        warpedCubes[0].transform.localPosition = Vector3.zero;

                        grabbablesR[index].enabled = false;

                        tmpMat = warpedCubes[0].GetComponent<Renderer>().materials;
                        tmpMat[0] = logosMat[(index+1)%6];
                        warpedCubes[0].GetComponent<Renderer>().materials = tmpMat;
                    } else if (condition == (int)Condition.V) {
                        warpedCubes[index].GetComponent<Renderer>().enabled = false;

                        GameObject tmp = Instantiate(cubePrefab, warpedCubes[index].transform.position, warpedCubes[index].transform.rotation);
                    }

                    index++;
                    prevStep = -1;

                    cubeTotalTime += cubeTime;
                    if (index==grabbables.Length) {
                        screenManager.addTime(cubeTotalTime);
                        pause = true;
                        warping = false;
                        experimentManager.EndTrial();
                        StartCoroutine("Pause");
                    }
                    sumVelocity = 0f;
                    nbVelocity = 0;
                    cubeDistGone = 0f;
                    handDistGone = 0f;
                    cubeTime = 0f;

                    break;
            }

            if (!pause) {
                print("Step: " + step + "/" + prevStep + ". Index: " + index + "/" + grabbables.Length);
                if (collisions!=0) {
                    if (!collisionSource.isPlaying) {
                        collisionSource.clip = bump;
                        collisionSource.Play();
                        scoreManager.Collision(pause);
                    } 
                    collisions=0;
                    col[index] += 1;
                }

                if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                    if ((warpedCubes[0].transform.position-hand.transform.position).magnitude<0.075f) {
                        print(cubeWatch.IsRunning);
                        if (!cubeWatch.IsRunning) {
                            cubeWatch.Start();
                        }
                        velocity = (warpedCubes[0].transform.position - cubePrevPos).magnitude;
                        cubeDistGone += velocity;
                        velocity /= Time.fixedDeltaTime;
                        sumVelocity += velocity;
                        nbVelocity++;

                    } else {
                        if (cubeWatch.IsRunning) {
                            cubeWatch.Stop();
                            cubeTime += (float)cubeWatch.Elapsed.TotalSeconds;
                            cubeWatch = new Stopwatch();
                        }
                        velocity = 0f;
                    }
                    
                } else {
                    if ((warpedCubes[index].transform.position-hand.transform.position).magnitude<0.075f) {
                        if (!cubeWatch.IsRunning) {
                            cubeWatch.Start();
                        }
                        velocity = (warpedCubes[index].transform.position - cubePrevPos).magnitude;
                        cubeDistGone += velocity;
                        velocity /= Time.fixedDeltaTime;
                        sumVelocity += velocity;
                        nbVelocity++;

                    } else {
                        if (cubeWatch.IsRunning) {
                            cubeWatch.Stop();
                            cubeTime += (float)cubeWatch.Elapsed.TotalSeconds;
                            cubeWatch = new Stopwatch();
                        }
                        velocity = 0f;
                    }
                }
                handDistGone += (hand.transform.position - handPrevPos).magnitude;
                handPrevPos = hand.transform.position;

                time = DateTime.Now;
                if (condition == (int)Condition.VBW || condition == (int)Condition.RW1) {
                    experimentManager.LogContinous(time.ToString("HH:mm:ss.fff"), index,
                                                   physicalCubes[0].transform.position, physicalCubes[0].transform.eulerAngles,
                                                   warpedCubes[0].transform.position, warpedCubes[0].transform.eulerAngles,
                                                   phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                                   velocity, cubeTime, nbError,
                                                   warping,
                                                   cubeDistGone, handDistGone,
                                                   uduinoScript.GetAcceleration(), col,
                                                   scoreManager.GetScore(), pause);
                    cubePrevPos = warpedCubes[0].transform.position;
                } else {
                    experimentManager.LogContinous(time.ToString("HH:mm:ss.fff"), index,
                                                   physicalCubes[index].transform.position, physicalCubes[index].transform.eulerAngles,
                                                   warpedCubes[index].transform.position, warpedCubes[index].transform.eulerAngles,
                                                   phantoms[index].transform.position, phantoms[index].transform.eulerAngles,
                                                   velocity, cubeTime, nbError,
                                                   warping,
                                                   cubeDistGone, handDistGone,
                                                   uduinoScript.GetAcceleration(), col,
                                                   scoreManager.GetScore(), pause);
                    cubePrevPos = warpedCubes[index].transform.position;
                }
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
        cubeTotalTime = 0;

        Material[] tmpMat = new Material[3];
        
        // for(int i=0; i<phantoms.Length; i++) {
        //     tmpMat = phantoms[i].GetComponent<Renderer>().materials;
        //     tmpMat[0] = phantomMat;
        //     phantoms[i].GetComponent<Renderer>().materials = tmpMat;
        //     phantoms[i].GetComponent<Renderer>().enabled = false;
        // }

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

        foreach(Renderer g in grabbablesR) {
            g.GetComponent<Renderer>().enabled = true;
        }
        grabbablesR[5].GetComponent<Renderer>().enabled = false;

        print("RESET::SceneReset::DONE");
    }

    IEnumerator Pause() {
        crRunning = true;

        scoreManager.UpdatePause(1);
        screenManager.Pause(1);
        pause = true;

        yield return new WaitForSeconds(20);

        scoreManager.UpdatePause(-1);
        screenManager.Pause(2);

        crRunning = false;
    }

    IEnumerator Timer() {
        buttonTimer = false;

        yield return new WaitForSeconds(1);

        buttonTimer = true;
    }
}
