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

    public void Collision(bool pause) {
    	// score -= 10;
     //    setScore(pause);
    }

    public void AddScoreCube(float magnitude, bool pause) {
    	// score += magnitude*10;
    	// setScore(pause);
    }

    public void AddScoreTime(float time, bool pause) {
    	// score += (30 - time<30 ? time : 30)*10;
    	// setScore(pause);
    }

    public void ResetScore() {
    	// score = 0;
    	// setScore(false);
    }

    public float GetScore() {
    	return score;
    }

    public void UpdatePause(int pause) {
        // screenManager.UpdatePause(pause);
    }

    void setScore(bool pause) {
    	// screenManager.UpdateScore((int)score, pause);
    }
}
