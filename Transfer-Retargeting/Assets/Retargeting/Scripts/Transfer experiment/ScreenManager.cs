using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
	public GameObject tvTextScore, tvTextSumScore;
    public int index = 0;
    public bool start = false;
    public Transform hand, fixedPoint;
    
    public int step = 0, prevStep = -1;
	string[] entries;
    DateTime tic;

    string scoreText = "Score : ";

    GameObject[] cubes;
    GameObject obstacle;

    int score, sumScore = 0;

    public Material phantomRightMat, phantomMat;

    void Start() {
        entries = new string[8];
        entries[0] = "Votre objectif est de :\n\n- Prendre le <color=#36c>cube bleu</color> et de le placer, avec\n la bonne orientation (face blanche),\ndans le <color=#d33>cube rouge</color> semi-transparent." +
                      "\nLorsqu'il est bien positionné, le <color=#d33>cube rouge</color> devient <color=#00af40>vert</color>.\n\n- De ne pas toucher les obstacles gris semi-transparent."
                      +"\nIls deviennent <color=#d33>rouge</color> lorsque vous les touchez" +
                      "\n\nPrêt? Appuyer sur la <color=#00af40>sphère verte</color>.";
        entries[1] = "\n\nPlacer le <color=#36c>cube bleu</color> dans le <color=#d33>cube rouge</color>\n\n, puis toucher la <color=#00af40>sphère verte</color>";
        tvTextScore.GetComponent<TextMesh>().text = entries[0];

        cubes = new GameObject[2];
        cubes[0] = GameObject.Find("Warped Cube");
        cubes[1] = GameObject.Find("Cube Phantom Intro");
        obstacle = GameObject.FindGameObjectsWithTag("Obstacle")[0];

        cubes[0].GetComponent<Renderer>().enabled = false;
        cubes[1].GetComponent<Renderer>().enabled = false;
        obstacle.SetActive(false);
    }

    void Update() {
        if (start) {
            if (step == 0) {
                if (index == 0) {
                    if (prevStep == -1) {
                        cubes[0].GetComponent<Renderer>().enabled = true;
                        cubes[1].GetComponent<Renderer>().enabled = true;
                        obstacle.SetActive(true);
                        tvTextScore.GetComponent<TextMesh>().text = entries[index];
                        prevStep = 0;
                    }

                    if ((hand.position - fixedPoint.position).magnitude < 0.07f) {
                        index++;
                    }
                } else if (index == 1 && prevStep == 0) {
                    tvTextScore.GetComponent<TextMesh>().text = entries[index];
                }

                if ((cubes[0].transform.position - cubes[1].transform.position).magnitude < 0.02f &&
                    Quaternion.Angle( cubes[0].transform.rotation, cubes[1].transform.rotation) < 7f) {
                    Material[] tmpMat = cubes[1].GetComponent<Renderer>().materials;
                    tmpMat[1] = phantomRightMat;
                    cubes[1].GetComponent<Renderer>().materials = tmpMat;
                    if (index == 1 && (hand.position - fixedPoint.position).magnitude < 0.07f) {
                        cubes[1].SetActive(false);
                        step++;
                    }
                } else {
                    Material[] tmpMat = cubes[1].GetComponent<Renderer>().materials;
                    tmpMat[1] = phantomMat;
                    cubes[1].GetComponent<Renderer>().materials = tmpMat;
                }
            }
        }
    }

    public void UpdateScore(int newScore) {
        tvTextScore.GetComponent<TextMesh>().text = scoreText + newScore + entries[1];
        if (newScore==0) {
            sumScore += score;
            tvTextSumScore.GetComponent<TextMesh>().text = "Score total : " + sumScore;
        }
        score = newScore;
    }
}
