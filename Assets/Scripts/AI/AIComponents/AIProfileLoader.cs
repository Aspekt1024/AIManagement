using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI
{
    public static class AIProfileLoader
    {
        public static List<AIGoal> GetGoals(AIAgentProfile profile)
        {
            List<AIGoal> goals = new List<AIGoal>();

            foreach (var goal in profile.Goals)
            {
                AIGoal newGoal = (AIGoal)Activator.CreateInstance(Type.GetType(goal.goalName));
                newGoal.Priority = goal.priority;
                goals.Add(newGoal);
            }

            return goals;
        }

        public static List<AIAction> GetActions(AIAgentProfile profile)
        {
            List<AIAction> goals = new List<AIAction>();

            foreach (var action in profile.Actions)
            {
                AIAction newAction = (AIAction)Activator.CreateInstance(Type.GetType(action.actionName));
                newAction.Cost = action.cost;
                goals.Add(newAction);
            }

            return goals;
        }
    }
}
