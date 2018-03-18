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
        bool goalExists = false;
        foreach (var goal in Goals)
        {
            if (goal.goalName == goalName)
            {
                goalExists = true;
                break;
            }
        }

        if (!goalExists)
        {
            var newGoal = new AIGoalSerializable() { goalName = goalName, priority = priority };
            Goals.Add(newGoal);
        }
    }

    public void AddAction(string actionName, float cost)
    {
        bool actionExists = false;
        foreach (var action in Actions)
        {
            if (action.actionName == actionName)
            {
                actionExists = true;
                break;
            }
        }

        if (!actionExists)
        {
            var newAction = new AIActionSerializable() { actionName = actionName, cost = cost };
            Actions.Add(newAction);
        }
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

