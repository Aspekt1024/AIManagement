using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI.Planning
{
    public class AINode
    {
        private float g;    // Node Cost
        private float h;    // Heuristic

        private AIAgent agent;
        private AIPlanner planner;
        private AIAction action;
        private AINode parent;
        private AIState state;

        public AINode(AIAgent agent, AIPlanner planner, AIAction action = null, AINode parent = null)
        {
            this.agent = agent;
            this.planner = planner;
            this.action = action;
            this.parent = parent;

            if (parent == null)
            {
                // This is the goal node
                state = new AIState(planner.GetGoal(), agent.GetMemory().CloneState());
                state.AddUnmetPreconditions(planner.GetGoal().GetConditions());
                g = 0;
            }
            else
            {
                // This is an action node
                SetNodeActionDetails();
            }

            h = GetNumUnmetPreconditions();
        }

        public void Update(AINode newParent)
        {
            if (newParent.g + action.Cost < g)
            {
                parent = newParent;
                SetNodeActionDetails();
            }
        }

        private void SetNodeActionDetails()
        {
            g = parent.g + action.Cost;
            state = parent.CloneState();
            state.ClearMetPreconditions(action.GetEffects());
            state.AddUnmetPreconditions(action.GetPreconditions());
        }

        private int GetNumUnmetPreconditions()
        {
            return state.GetPreconditions().Count;
        }

        public AIState CloneState()
        {
            return new AIState(state);
        }

        public AIState GetState()
        {
            return state;
        }

        public AIAction GetAction()
        {
            return action;
        }

        public AINode GetParent()
        {
            return parent;
        }

        public bool ConditionsMet()
        {
            return state.GetPreconditions().Count == 0;
        }

        public float GetFCost()
        {
            return g + h;
        }
    }
}

