using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSize : MonoBehaviour {

	public Transform rightHand;
	Transform fingers;
	public Transform rightHandMetaphor;
	public GameObject RealCube, Button;
	Renderer RealCubeR, ButtonR;
	public Material active, passive;

	public Transform[] virtualCubes;

	BodyWarping bWScript;

	bool cubeCollision = false, buttonCollision = false;
	bool stopWarping = false;

	int state = 0, trialState = 0, indexC = 0, indexD = 0;

	float[] valuesD = new float[4] {0.3f, 0.35f, 0.4f, 0.45f};

	KeyValuePair<Vector3, Vector3> result;

    void Start() {
    	fingers = rightHand.GetChild(0);

        RealCubeR = RealCube.GetComponent<Renderer>();
        ButtonR = Button.GetComponent<Renderer>();
		RealCubeR.material = passive;
		ButtonR.material = active;

        bWScript = this.GetComponent<BodyWarping>();
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
        	case 0:
	        	if (buttonCollision || Input.GetKeyDown(KeyCode.Keypad1)) {
	        		state = 1;
    				RealCubeR.material = active;
    				ButtonR.material = passive;
	        	}
	        	break;
	        case 1:
	        	switch (trialState) {
	        		case 0:
	        			result = bWScript.BodyWarpP(fingers.position, valuesD[indexD], RealCube.transform.position, virtualCubes[indexC].position);
	        			for (int i=0; i<rightHand.childCount; i++) {
	        				rightHand.GetChild(i).position = rightHandMetaphor.GetChild(i).position + result.Value;
	        			}
	        			if (cubeCollision || Input.GetKeyDown(KeyCode.Keypad2)) {
	        				cubeCollision = false;
	        				trialState = 1;
	        				RealCubeR.material = passive;
	        				ButtonR.material = active;
	        			}
	        			break;
	        		case 1:
		        			
	        			if (!stopWarping) {
	        				result = bWScript.BodyWarpP(fingers.position, valuesD[indexD], RealCube.transform.position, virtualCubes[indexC].position);
		        			if (result.Value == Vector3.zero)
		        				stopWarping = true;
	        				for (int i=0; i<rightHand.childCount; i++) {
		        				rightHand.GetChild(i).position = rightHandMetaphor.GetChild(i).position + result.Value;
		        			}
	        			}
		        		
	        			if (buttonCollision || Input.GetKeyDown(KeyCode.Keypad1)) {
	        				buttonCollision = false;
	        				stopWarping = false;
	        				indexD++;
	        				if (indexD==4) {
	        					indexD = 0;
	        					indexC++;
	        					if (indexC==3)
	        						indexC = 0;
	        				}
	        				trialState = 0;
	        				RealCubeR.material = active;
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
    			if (trialState==2 || state==0)
    				buttonCollision = true;
    			break;
    		case 0:
    			if (trialState==0 && state==1)
    				cubeCollision = true;
    			break;
    	}
    }

    public bool IsWarping() {
    	return false;
    }
}
