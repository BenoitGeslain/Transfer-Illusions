using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Uduino;


public class Test_objet_n : MonoBehaviour {
    public Slider slider;
    public int vitesse;
    public int Pos_min;
    public int Pos_max;
    public KeyCode ToPress;
    public Text ToDisplay;


    // Use this for initialization
    void Start () {
        UduinoManager.Instance.alwaysRead = true;
        //UduinoManager.Instance.SetReadCallback(ReadEncoder);

    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(ToPress))
        {
            UduinoManager.Instance.sendCommand("Objet_i", Pos_min, Pos_max, vitesse);
        }
    }

    public void ReadEncoder(string data, UduinoDevice firstUduino)
    {
        slider.value = int.Parse(data);
        ToDisplay.text = data.ToString() + "mm";
    }
}
