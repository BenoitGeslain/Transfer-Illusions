 ﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameters {

	public static int blockFactor = 100;
	public static int trialFactor = 10000;

    public static string[] tracebackHeader = {"Participant", "Practice", "Block", "Trial", "Condition", "Time", "Index",
    										  "CubePosRX", "CubePosRY", "CubePosRZ",
    										  "CubeOrRX", "CubeOrRY", "CubeOrRZ",
    										  "CubePosVX", "CubePosVY", "CubePosVZ",
    										  "CubeOrVX", "CubeOrVY", "CubeOrVZ",
    										  "CubePosPX", "CubePosPY", "CubePosPZ",
    										  "CubeOrPX", "CubeOrPY", "CubeOrPZ",
    										  "CubeDistGone", "HandDistGone",
    										  "Acceleration", "collisions1", "collisions2", "collisions3", "collisions4",
    										  "score",
    										  "pause"};

    public static string[] experimentHeader = {"Participant", "Practice", "Block", "Trial", "Condition", "ElapsedTime", "TrialStartTime", "Index",
    										   "CubePosRX", "CubePosRY", "CubePosRZ",
    										   "CubeOrRX", "CubeOrRY", "CubeOrRZ",
    										   "CubePosPX", "CubePosPY", "CubePosPZ",
    										   "CubeOrPX", "CubeOrPY", "CubeOrPZ",
    										   "CubeDistGone", "HandDistGone",
    										   "Hit", "collisions1", "collisions2", "collisions3", "collisions4",
    										   "Score"};

    public static string[] sceneConfiguration = {"Name", "PX", "PY", "PZ", "RX", "RY", "RZ"};
}
