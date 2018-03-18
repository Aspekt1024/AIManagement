using System.Collections.Generic;
using UnityEngine;
using Aspekt.AI;

[CreateAssetMenu()]
public class AIAgentProfile : ScriptableObject
{
    public List<AIGoalSerializable> Goals = new List<AIGoalSerializable>();
    public List<AIActionSerializable> Actions = new List<AIActionSerializable>();
    
    public void AddGoal(string goalName, float priority)
    {
        var newGoal = new AIGoalSerializable() { goalName = goalName, priority = priority };
        if (Goals.Contains(newGoal)) return;

        Goals.Add(newGoal);
    }

    public void AddAction(string actionName, float cost)
    {
        var newAction = new AIActionSerializable() { actionName = actionName, cost = cost };
        if (Actions.Contains(newAction)) return;

        Actions.Add(newAction);
    }

    [System.Serializable]
    public struct AIGoalSerializable
    {
        public string goalName;
        public float priority;
    }

    [System.Serializable]
    public struct AIActionSerializable
    {
        public string actionName;
        public float cost;
    }
}

