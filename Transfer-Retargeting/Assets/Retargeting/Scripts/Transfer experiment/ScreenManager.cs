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
        entries[0] = "Votre objectif est de :\n\n- Sélectionner le cube bleu et de le placer, avec la\nbonne orientation, dans le cube rouge semi-transparent.\nLorsqu'il est bien positionner, le cube rouge devient vert.\n\n- De ne pas toucher les obstacles gris semi-transparent\n\nPrêt? Appuyer sur la sphère verte.";
        entries[1] = "\n\nPlacer le cube bleu dans le cube\n\nrouge, puis toucher la sphère verte";
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
        print("Updating Score : " + scoreText + newScore + entries[1]);
        tvTextScore.GetComponent<TextMesh>().text = scoreText + newScore + entries[1];
        if (newScore==0) {
            sumScore += score;
            tvTextSumScore.GetComponent<TextMesh>().text = "Score total : " + sumScore;
        }
        score = newScore;
    }
}
