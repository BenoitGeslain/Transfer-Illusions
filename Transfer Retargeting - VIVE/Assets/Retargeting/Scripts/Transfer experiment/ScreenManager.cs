using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
	public GameObject tvText;
    public int index = 0;
    int step = 0;
	string[] entries;
    DateTime tic;

    string scoreText = "Score : ";

    void Start() {
        entries = new string[8];
        entries[0] = "Lors de cette expérience,\nvous allez devoir saisir des cubes\net les placer à l'endroit indiqué";
        entries[1] = "Les cubes bleux représentent\nles cubes que vous pouvez manipuler";
        entries[2] = "Les cubes rouges translucides\nsont les cibles où vous devez poser\nles cubes bleus.";
        entries[3] = "Les obstacles sont transparents\npour que vous puissiez voir\nau travers,vous ne devez pas\nles toucher où ils deviendront\nrouges et vous perdrez des points";
        entries[4] = "Tendez les bras. Vous pouvez les\nvoir tout au long de\nl'expérience, vous pouvez\négalement saisir le cube bleu\ndevant vous.";
        entries[5] = "Prenez le temps de vous familiariser\navec l'environment, lorsque vous\nserez prêt, appuyez sur\nle bouton vert.";
        entries[6] = "\n\nPrenez le cube bleu et superposez\nle au cube rouge.\nPuis touchez la sphère verte\npour continuer.";
        tvText.GetComponent<TextMesh>().text = entries[0];
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && step == 0) {
            tvText.GetComponent<TextMesh>().text = entries[index];
            index++;
            print("EXEC::ScreenManager::Next message");
            if (index == 7) {
                step++;
                print("EXEC::ScreenManager::Intro Over");
            }
        } else if (step == 1) {
            tvText.GetComponent<TextMesh>().text = scoreText + 0 + entries[6];
        }
    }

    public void UpdateScore(int score) {
        tvText.GetComponent<TextMesh>().text = scoreText + score + entries[6];
    }
}
