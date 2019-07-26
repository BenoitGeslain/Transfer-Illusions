 ﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameters {

	public static int blockFactor = 100;
	public static int trialFactor = 10000;

    public static float[] CDRatio = {1, 1.2f};

    public static string[] tracebackHeader = {"Participant", "Practice", "Block", "Trial", "Time", "leftX", "leftY", "leftZ", "rightX", "rightY", "rightZ"};
    public static string[] experimentHeader = {"Participant", "Practice", "Block", "Trial", "Time"};

    public static float tracebackRepeatRate = 1f;
    public static float experimentRepeatRate = 10f;
}
