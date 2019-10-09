using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmWarping : MonoBehaviour
{
    public Transform armHandMetaphor, armHandTracked;

	BodyWarping bodyWarping;

    void Start() {
        bodyWarping = GameObject.Find("World").GetComponent<BodyWarping>();
    }

    void Update() {
        if (!bodyWarping.warp) {
			for (int i = 0; i<armHandMetaphor.childCount; i++) {
                armHandMetaphor.GetChild(i).position = armHandTracked.GetChild(i).position;
                armHandMetaphor.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
            }
        }
    }
}
