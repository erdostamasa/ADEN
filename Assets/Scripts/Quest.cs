using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {

    public bool isActive;
    
    public string title;
    [TextArea(10,3)]
    public string description;

    public QuestGoal goal;

}