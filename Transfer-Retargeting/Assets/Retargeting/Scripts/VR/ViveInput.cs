using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    Vector3[] grabbableObjects;
    Vector3[] phantomObjects;

    GameObject collidingObject;
    public GameObject objectInHand;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Down");
            if (collidingObject)
            {
                GrabObject();
            }
        } else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
            //print("Release");
        }
        //print(collidingObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        objectInHand.transform.SetParent(this.transform);
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void ReleaseObject()
    {
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.SetParent(null);
        objectInHand = null;
    }

}
