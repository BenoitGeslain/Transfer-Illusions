using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ExpermientDemo : MonoBehaviour
{
    public GameObject hand;

    public Material activeCube, passiveCube;
    public Material activePhantom, passivePhantom, activePhantomRight, buttonMatActive, buttonMatIdle;

    public GameObject fixedPoint;
    Renderer fixedPointR;

    ViveInput inputScript;
    SteamVR_Behaviour_Pose steamVRScript;

    GameObject[] grabbables;
    GameObject[] phantoms;

    Renderer[] grabbablesR;
    Renderer[] phantomsR;

    int index = 0;
    int step = 0, prevStep = -1;
    bool noWrap;

    void Start()
    {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");

        grabbablesR = new Renderer[grabbables.Length];
        phantomsR = new Renderer[phantoms.Length];
        for (int i=0; i<grabbables.Length; i++)
        {
            grabbablesR[i] = grabbables[i].GetComponent<Renderer>();
        }
        for (int i = 0; i < phantoms.Length; i++)
        {
            phantomsR[i] = phantoms[i].GetComponent<Renderer>();
        }
        
        foreach (Renderer r in phantomsR)
        {
            r.material = passivePhantom;
        }
        foreach (Renderer r in grabbablesR)
        {
            r.material = passiveCube;
        }
        fixedPointR = fixedPoint.GetComponent<Renderer>();

        inputScript = hand.GetComponent<ViveInput>();
        steamVRScript = hand.GetComponent<SteamVR_Behaviour_Pose>();
    }
    
    void Update()
    {
        //print("Actual (pre) : " + step + ", previous : " + prevStep);
        switch (step)
        {
            case 0:
                if (inputScript.objectInHand == grabbables[index])
                    step = 1;
                break;
            case 1:
                if (inputScript.objectInHand != grabbables[index])
                    step = 0;
                else if ((hand.transform.position - phantoms[index].transform.position).magnitude < 0.1f)
                    step = 2;
                break;
            case 2:
                prevStep = 2;
                if (inputScript.objectInHand != grabbables[index])
                    step = 3;
                else if ((hand.transform.position - phantoms[index].transform.position).magnitude > 0.1f)
                    step = 1;
                break;
            case 3:
                prevStep = 3;
                if ((hand.transform.position - fixedPoint.transform.position).magnitude < 0.1f)
                {
                    step = 4;
                }
                break;
            case 4:
                prevStep = 4;
                index++;
                step = 0;
                break;
            default:
                step = 0;
                break;
        }

        //print("Actual : " + step + ", previous : " + prevStep);

        if (prevStep == 1 && step == 0)
        {
            noWrap = true;
            print("NO WRAP!");
        }
        if (noWrap && step == 2)
            noWrap = false;

        switch (step)
        {
            case 0:
                if (prevStep != 0)
                {
                    foreach (Renderer r in grabbablesR)
                    {
                        r.material = passiveCube;
                    }
                    grabbablesR[index].material = activeCube;

                    foreach (Renderer r in phantomsR)
                    {
                        r.material = passivePhantom;
                    }
                    prevStep = 0;
                }
                break;
            case 1:
                if (prevStep != 1)
                {
                    if (!noWrap)
                    {
                        steamVRScript.wOrigin = grabbables[index].transform.position;
                        steamVRScript.wTargetReal = grabbables[index + 1].transform.position;
                        steamVRScript.wTargetVirtual = phantoms[index].transform.position;
                    }
                    
                    steamVRScript.warping = true;
                    print("Warp origin = " + grabbables[index].transform.position);
                    print("Warp real target = " + grabbables[index+1].transform.position);
                    print("Warp virtual target = " + phantoms[index].transform.position);
                    foreach (Renderer r in phantomsR)
                    {
                        r.material = passivePhantom;
                    }
                    phantomsR[index].material = activePhantom;
                    prevStep = 1;
                }
                break;
            case 2:
                phantomsR[index].material = activePhantomRight;
                break;
            case 3:
                fixedPointR.material = buttonMatActive;
                foreach (Renderer r in phantomsR)
                {
                    r.material = passivePhantom;
                }
                steamVRScript.wOrigin = fixedPoint.transform.position;
                break;
            case 4:
                fixedPointR.material = buttonMatIdle;
                steamVRScript.warping = false;
                break;
        }
        if (prevStep == 1 && step == 0)
        {
            noWrap = true;
            print("NO WRAP!");
        }
        if (noWrap && step == 2)
            noWrap = false;
    }
}
