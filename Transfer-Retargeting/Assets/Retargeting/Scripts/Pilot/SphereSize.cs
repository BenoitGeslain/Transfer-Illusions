using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSize : MonoBehaviour {

	public Transform rightHand;
	Transform fingers;
	public Transform rightHandMetaphor;
	public GameObject RealCube, Button;
	Renderer ButtonR;
	public Material active, passive;

	public Transform[] virtualCubes;
	Renderer[] virtualCubesR;

	BodyWarping bWScript;

	bool cubeCollision = false, buttonCollision = false;
	bool stopWarping = false;

	int state = 0, trialState = 0, indexC = 0, indexD = 0;

	float[] valuesD = new float[5] {0f, 0.05f, 0.1f, 0.15f, 0.2f};
	float delta = 0.05f;

	KeyValuePair<Vector3, Vector3> result;

    void Start() {
    	fingers = rightHand.GetChild(0);

        ButtonR = Button.GetComponent<Renderer>();
		ButtonR.material = active;

		virtualCubesR = new Renderer[virtualCubes.Length];
		for (int i=0; i<virtualCubes.Length; i++) {
			virtualCubesR[i] = virtualCubes[i].GetComponent<Renderer>();
			virtualCubesR[i].material = passive;
		}

        bWScript = this.GetComponent<BodyWarping>();

        stopWarping = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
        switch (state) {
        	case 0:
	        	if (buttonCollision || Input.GetKeyDown(KeyCode.Keypad1)) {
	        		state = 1;
    				virtualCubesR[indexC].material = active;
    				ButtonR.material = passive;
    				stopWarping = false;
	        	}
	        	break;
	        case 1:
	        	switch (trialState) {
	        		case 0:	// touch Cube
	        			result = bWScript.BodyWarpP(rightHandMetaphor.GetChild(0).position, Button.transform.position, valuesD[indexD], RealCube.transform.position, virtualCubes[indexC].position);

	        			for (int i=0; i<1; i++) {
	        				rightHand.GetChild(i).position = rightHandMetaphor.GetChild(i).position + result.Value;
		        			rightHand.GetChild(i).eulerAngles = rightHandMetaphor.GetChild(i).eulerAngles;
	        			}

	        			if (cubeCollision || Input.GetKeyDown(KeyCode.Keypad2)) {
	        				cubeCollision = false;
	        				trialState = 1;
	        				virtualCubesR[indexC].material = passive;
	        				ButtonR.material = active;
	        			}
	        			break;
	        		case 1:	// Touch button
	        			if (!stopWarping) {
	        				result = bWScript.BodyWarpP(rightHandMetaphor.GetChild(0).position, Button.transform.position, valuesD[indexD], RealCube.transform.position, virtualCubes[indexC].position);

		        			if (result.Value == Vector3.zero)
		        				stopWarping = true;
	        				for (int i=0; i<rightHand.childCount; i++) {
		        				rightHand.GetChild(i).position = rightHandMetaphor.GetChild(i).position + result.Value;
		        				rightHand.GetChild(i).eulerAngles = rightHandMetaphor.GetChild(i).eulerAngles;
		        			}
	        			}
		        		
	        			if (buttonCollision || Input.GetKeyDown(KeyCode.Keypad1)) {
	        				buttonCollision = false;
	        				stopWarping = false;
	        				indexD++;
	        				if (indexD==valuesD.Length) {
	        					indexD = 0;
	        					indexC++;
	        					if (indexC==virtualCubes.Length)
	        						indexC = 0;
	        				}
	        				trialState = 0;
	        				virtualCubesR[indexC].material = active;
	        				ButtonR.material = passive;
	        			}
	        			break;
	        	}
	        	break;
        }
    }

    public void Collision(int id) {
    	switch (id) {
    		case -1:
    			if (trialState==1 || state==0) {
    				buttonCollision = true;
    			}
    			break;
    		case 0:
    			if (trialState==0 && state==1)
    				cubeCollision = true;
    			break;
    		case 1:
    			if (trialState==0 && state==1 && indexC==0) {
    				cubeCollision = true;
    			}
    			break;
    		case 2:
    			if (trialState==0 && state==1 && indexC==1) {
    				cubeCollision = true;
    			}
    			break;
    		case 3:
    			if (trialState==0 && state==1 && indexC==2) {
    				cubeCollision = true;
    			}
    			break;
    	}
    }

    public bool IsWarping() {
    	return !stopWarping;
    }
}
