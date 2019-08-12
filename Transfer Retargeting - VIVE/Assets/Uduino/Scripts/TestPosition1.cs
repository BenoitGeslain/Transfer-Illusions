using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using UnityEngine.UI;


public class TestPosition1 : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        UduinoManager.Instance.alwaysRead = true;
        UduinoManager.Instance.SetReadCallback(ReadEncoder);
    }

    private void Update()
    {

        if (Input.GetKeyDown("1"))
        {
            UduinoManager.Instance.sendCommand("objet1");
        }
        if (Input.GetKeyDown("2"))
        {
            UduinoManager.Instance.sendCommand("objet2");
        }
        if (Input.GetKeyDown("3"))
        {
            UduinoManager.Instance.sendCommand("objet3");
        }
        if (Input.GetKeyDown("4"))
        {
            UduinoManager.Instance.sendCommand("objet4");
        }
        if (Input.GetKeyDown("5"))
        {
            UduinoManager.Instance.sendCommand("objet5");
        }
        if (Input.GetKeyDown("6"))
        {
            UduinoManager.Instance.sendCommand("objet6");
        }
        if (Input.GetKeyDown("7"))
        {
            UduinoManager.Instance.sendCommand("objet7");
        }
    }

    // Reading Encoder thanks to read callback
    void ReadEncoder(string data)
    {
        slider.value = int.Parse(data);
    }

}
