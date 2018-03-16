using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIPlanner
    {
        private AIAgent agent;

        private Queue<AIAction> actions = new Queue<AIAction>();

        private AIGoal currentGoal;
        private AIAction currentAction;

        public AIPlanner (AIAgent agent)
        {
            this.agent = agent;
        }

        public event Action OnPlannerIdle = delegate { };

        public void CalculateNewGoal()
        {
            if (agent.GetGoals().Count == 0) return;

            bool goalAchievable = false;
            foreach (AIGoal goal in agent.GetGoals())
            {
                // Start with a single condition, then expand to multiple conditions later
                KeyValuePair<string, object> condition = new KeyValuePair<string, object>();
                foreach (KeyValuePair<string, object> c in goal.GetConditions())
                {
                    condition = c;
                    break;
                }

                if (condition.Value == null) { Debug.Log("Goal has no conditions"); return; }

                // Check each action, see if it fulfills the goal
                // if not, check actions and see if they fulfil the preconditions
                // need to check the agent state
                foreach (AIAction action in agent.GetActions())
                {
                    // For now, assume preconditions are met
                    if (action.GetEffects().Count > 0)
                    {
                        if (action.GetEffects().ContainsKey(condition.Key))
                        {
                            if (action.GetEffects()[condition.Key].Equals(condition.Value))
                            {
                                // Act like there's only one action for now
                                goalAchievable = true;
                                actions.Enqueue(action);
                                break;
                            }
                        }
                    }
                    if (goalAchievable) break;
                }

                if (goalAchievable)
                {
                    currentGoal = goal;
                    break;
                }
            }
        }

        public void Run()
        {
            if (currentAction == null)
            {
                SelectNextAction();
            }
            else
            {
                currentAction.Run();
            }
        }
        
        private void SelectNextAction()
        {
            if (actions.Count == 0)
            {
                currentAction = null;
                if (OnPlannerIdle != null) OnPlannerIdle();
            }
            else
            {
                if (currentAction != null)
                {
                    currentAction.OnSuccess -= ActionSuccess;
                    currentAction.OnFailure -= ActionFailure;
                }

                currentAction = actions.Dequeue();
                currentAction.OnSuccess += ActionSuccess;
                currentAction.OnFailure += ActionFailure;
                currentAction.Run();
            }
        }

        private void ActionSuccess()
        {
            SelectNextAction();

            // TODO apply effects of action
            // Check if goal has succeeded
        }

        private void ActionFailure()
        {

        }
    }
}
