using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        //Display.displays[2].Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
