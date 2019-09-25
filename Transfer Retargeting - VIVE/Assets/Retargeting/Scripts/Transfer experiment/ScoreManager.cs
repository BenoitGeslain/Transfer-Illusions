using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	ScreenManager screenManager;

	float score;

    void Start() {
        screenManager = GetComponent<ScreenManager>();
    }

    void Update() {
        
    }

    public void Collision() {
    	score -= 10;
        setScore();
    }

    public void AddScoreCube(float magnitude) {
    	score += magnitude*10;
    	setScore();
    }

    public void AddScoreTime(float time) {
    	score += time*10;
    	setScore();
    }

    public void ResetScore() {
    	score = 0;
    	setScore();
    }

    public float GetScore() {
    	return score;
    }

    void setScore() {
    	screenManager.UpdateScore((int)score);
    }
}
