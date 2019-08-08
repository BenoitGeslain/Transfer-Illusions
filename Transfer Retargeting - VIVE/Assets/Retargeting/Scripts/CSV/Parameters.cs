 ﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameters {

	public static int blockFactor = 100;
	public static int trialFactor = 10000;

    public static float[] CDRatio = {1, 1.2f};

    public static string[] tracebackHeader = {"Participant", "Practice", "Block", "Trial", "Condition", "Time", "Index", "CubePosRX", "CubePosRY", "CubePosRZ", "CubeOrRX", "CubeOrRY", "CubeOrRZ", "CubePosVX", "CubePosVY", "CubePosVZ", "CubeOrVX", "CubeOrVY", "CubeOrVZ", "Score"};
    public static string[] experimentHeader = {"Participant", "Practice", "Block", "Trial", "Index", "ErrorPosVX", "ErrorPosVY", "ErrorPosVZ", "ErrorOr", "Hit", "Score"};
}
