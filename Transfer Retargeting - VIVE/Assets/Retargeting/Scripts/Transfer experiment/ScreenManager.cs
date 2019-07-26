using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
	public GameObject tvText;
	string[] entries;
    // Start is called before the first frame update
    void Start()
    {
        entries = new string[6];
        entries[0] = "Lors de cette expérience, vous allez devoir saisir des cubes et les placer à l'endroit indiqué";
        entries[1] = "Les cubes bleux représentent les cubes que vous pouvez manipuler";
        entries[2] = "Les cubes rouges translucides sont les cibles où vous devez poser les cubes bleus.";
        entries[3] = "Les obstacles sont transparents pour que vous puissiez voir au travers, vous ne devez pas les toucher où ils deviendront rouges et vous perdrez des points";
        entries[4] = "Tendez les bras. Vous pouvez les voir tout au long de l'expérience, vous pouvez également saisir le cube bleu devant vous.";
    	entries[5] = "Prenez le temps de vous familiariser avec l'environment, lorsque vous serez prêt, appuyez sur le bouton vert.";
    }

    void Update()
    {
        
    }
}
