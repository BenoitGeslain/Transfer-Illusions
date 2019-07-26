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

    void Start() {
        entries = new string[8];
        entries[0] = "Lors de cette expérience,\nvous allez devoir saisir des cubes\net les placer à l'endroit indiqué";
        entries[1] = "Les cubes bleux représentent\nles cubes que vous pouvez manipuler";
        entries[2] = "Les cubes rouges translucides\nsont les cibles où vous devez poser\nles cubes bleus.";
        entries[3] = "Les obstacles sont transparents\npour que vous puissiez voir au travers,\nvous ne devez pas les toucher où ils deviendront rouges et vous perdrez des points";
        entries[4] = "Tendez les bras. Vous pouvez les voir\ntout au long de l'expérience,\nvous pouvez également saisir le cube bleu devant vous.";
        entries[5] = "Prenez le temps de vous familiariser\navec l'environment, lorsque vous serez prêt,\nappuyez sur le bouton vert.";
        entries[6] = "Prener le cube bleu et superposé\nle au cube rouge.";
        entries[7] = "Touchez la sphère vert pour continuer.";
        tvText.GetComponent<TextMesh>().text = entries[0];
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && step == 0) {
            tvText.GetComponent<TextMesh>().text = entries[index];
            index++;
            if (index == 6)
                step++;
                print("EXEC::ScreenManager::Next message");
        } else if (step == 1) {
            tvText.GetComponent<TextMesh>().text = entries[6];
        }
    }
}
