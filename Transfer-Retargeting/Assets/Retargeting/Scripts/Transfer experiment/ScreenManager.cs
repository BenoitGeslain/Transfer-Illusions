using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
	public GameObject tvTextCube, tvTextTime, tvTextCube2, tvTextTime2, tvTextInstructions;
    public int index = 0;
    public bool start = false;
    public Transform hand, fixedPoint;
    
    public int step = 0, prevStep = -1;

    public AudioClip bump, coin, fire;
    
    string[] entries;
    DateTime tic;

    string scoreText = "Score : ";

    GameObject[] cubes;
    GameObject obstacle;

    int score, sumScore = 0;
    int ind = 1, count = 0;

    bool changeText = false;

    AudioSource collisionSource;
    public int collisions = 0;

    public Material phantomRightMat, phantomMat, cubeLogo1;

    bool soundPlayed = false;

    void Start() {
        entries = new string[3];
        // entries[0] = "Votre objectif est de :\n\n- Prendre le <color=#36c>cube bleu</color> et de le placer, avec\n la bonne orientation (face blanche),\ndans la <color=#36c>cible bleue</color>." +
        //               "\nLorsqu'il est bien positionné, la <color=#36c>cible bleue</color> devient <color=#00af40>verte</color>.\n\n- De ne pas toucher les obstacles gris."
        //               +"\nIls deviennent <color=#d33>rouge</color> lorsque vous les touchez" +
        //               "\n\nPrêt? Appuyer sur la <color=#00af40>sphère verte</color>.";
        // entries[1] = "\n\nPlacer le <color=#36c>cube bleu</color> dans la <color=#36c>cible bleue</color>,\n\npuis toucher la <color=#00af40>sphère verte</color>";
        entries[0] = "<color=#FFFFFF>1</color>";
        entries[1] = "Time";
        entries[2] = "Placer le <color=#36c>cube bleu</color> dans la <color=#36c>cible bleue</color>,\n\npuis toucher la <color=#00af40>sphère verte</color>";
        tvTextCube.GetComponent<TextMesh>().text = entries[0];
        tvTextTime.GetComponent<TextMesh>().text = entries[1];
        tvTextInstructions.GetComponent<TextMesh>().text = entries[2];

        cubes = new GameObject[2];
        cubes[0] = GameObject.Find("Warped Cube");
        cubes[1] = GameObject.Find("Target Intro");
        obstacle = GameObject.FindGameObjectsWithTag("Obstacle")[0];

        cubes[0].GetComponent<Renderer>().enabled = false;
        cubes[1].GetComponent<Renderer>().enabled = false;
        obstacle.SetActive(false);

        collisionSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (start) {
            if (step == 0) {
                if (Input.GetKeyDown(KeyCode.Keypad0)) {
                    step++;
                    index = 1;
                }
                if (index == 0) {
                    if (prevStep == -1) {
                        cubes[0].GetComponent<Renderer>().enabled = true;
                        cubes[1].GetComponent<Renderer>().enabled = true;
                        obstacle.SetActive(true);
                        tvTextCube.GetComponent<TextMesh>().text = entries[0];
                        prevStep = 0;
                    }

                    if ((hand.position - fixedPoint.position).magnitude < 0.07f) {
                        index++;
                    }
                } else if (index == 1) {
                    // UpdateScore(0, false);
                    index = 2;
                }

                if ((cubes[0].transform.position - cubes[1].transform.position).magnitude < 0.02f &&
                    Quaternion.Angle(cubes[0].transform.rotation, cubes[1].transform.rotation) < 7f) {
                    Material[] tmpMat = cubes[1].GetComponent<Renderer>().materials;
                    tmpMat[0] = phantomRightMat;
                    cubes[1].GetComponent<Renderer>().materials = tmpMat;
                    if ((hand.position - fixedPoint.position).magnitude < 0.075f) {
                        // collisionSource.Stop();
                        // collisionSource.clip = fire;
                        // collisionSource.Play();

                        cubes[1].SetActive(false);
                        tmpMat = cubes[0].GetComponent<Renderer>().materials;
                        tmpMat[0] = cubeLogo1;
                        cubes[0].GetComponent<Renderer>().materials = tmpMat;

                        step++;
                    }
                    // if (!soundPlayed && !collisionSource.isPlaying) {
                    //     soundPlayed = true;
                    //     collisionSource.clip = coin;
                    //     collisionSource.Play();
                    // }
                } else {
                    soundPlayed = false;
                    Material[] tmpMat = cubes[1].GetComponent<Renderer>().materials;
                    tmpMat[0] = phantomMat;
                    cubes[1].GetComponent<Renderer>().materials = tmpMat;
                }

                if (collisions!=0) {
                    if (!collisionSource.isPlaying) {
                        collisionSource.clip = bump;
                        collisionSource.Play();
                    }
                    collisions=0;
                }
            } else if (step==1) {
                cubes[1].SetActive(false);
                step++;
            } else {

            }
        }
    }

    public void goRed() {
        entries[0] = entries[0].Remove(entries[0].Length-24);
        entries[0] += "<color=#FF0000>" + ind.ToString() + "</color>";
        tvTextCube.GetComponent<TextMesh>().text = entries[0];
    }

    public void goGreen() {
        print("!");
        entries[0] = entries[0].Remove(entries[0].Length-24);
        entries[0] += "<color=#00FF00>" + ind.ToString() + "</color>";

        if (ind<6) {
            ind++;
            entries[0] += "\t<color=#FFFFFF>" + ind.ToString() + "</color>";
        } else {
            ind = 1;
            count++;
            if (count==8) {
                changeText = true;
            } else {
                entries[0] += "\n<color=#FFFFFF>1</color>";
            }
        }
        tvTextCube.GetComponent<TextMesh>().text = entries[0];
    }

    public void addTime(float time) {
        entries[1] += "\n" + time.ToString("F1");
        tvTextTime.GetComponent<TextMesh>().text = entries[1];
        if (changeText) {
            tvTextTime = tvTextTime2;
            tvTextCube = tvTextCube2;
            entries[0] = "<color=#FFFFFF>1</color>";
            entries[1] = "Time";
            count++;
            tvTextCube.GetComponent<TextMesh>().text = entries[0];
            changeText = false;
        }
    }

    public void Pause(int pause) {
        if (pause==1) {
            tvTextInstructions.GetComponent<TextMesh>().text = "Jeu en pause pendant 30 secondes";
        } else if (pause==0) {
            tvTextInstructions.GetComponent<TextMesh>().text = entries[2];
        } else {
            tvTextInstructions.GetComponent<TextMesh>().text = "Jeu en pause\nPrêt? Appuyer sur la <color=#00af40>sphère verte</color>\n\n";
        }
    }

    // public void UpdatePause(int pause) {
    //     if (pause==1) {
    //         tvTextCube.GetComponent<TextMesh>().text = "Jeu en pause\n\n" + scoreText + score;
    //     } else if (pause==0) {
    //         tvTextCube.GetComponent<TextMesh>().text = scoreText + score + entries[1];
    //     } else {
    //         tvTextCube.GetComponent<TextMesh>().text = "Jeu en pause\nPrêt? Appuyer sur la <color=#00af40>sphère verte</color>\n\n" + scoreText + score;
    //     }
    // }

    // public void UpdateScore(int newScore, bool pause) {
    //     tvTextCube.GetComponent<TextMesh>().text = scoreText + newScore + entries[1];
    //     if (newScore==0) {
    //         sumScore += score;
    //         tvTextTime.GetComponent<TextMesh>().text = "Score total : " + sumScore;
    //     }
    //     score = newScore;
    // }
}
