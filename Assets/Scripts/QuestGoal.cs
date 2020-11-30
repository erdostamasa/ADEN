﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal {
    public GoalType goalType;

    public List<GameObject> targetPlanets;
    public int requiredAmount;
    public int currentAmount;

    public bool IsQuestComplete() {
        return (currentAmount >= requiredAmount);
    }

    
}

public enum GoalType {
    CollectIce,
    CollectIron,
    ScanPlanet,
    BuildScanningPost,
    MoonLanding,
}