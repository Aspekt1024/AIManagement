using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI.Planning
{
    public class AIAStar
    {
        private List<AINode> openNodes = new List<AINode>();
        private List<AINode> closedNodes = new List<AINode>();

        private AINode currentNode;

        public bool FindActionPlan(AIAgent agent, AIPlanner planner)
        {
            openNodes = new List<AINode>();
            closedNodes = new List<AINode>();

            currentNode = new AINode(agent, planner);
            closedNodes.Add(currentNode);

            while(!currentNode.ConditionsMet())
            {
                foreach (var action in agent.GetActions())
                {
                    if (ActionNotInClosedNodeList(action) && ActionAchievesPrecondition(action))
                    {
                        if (!UpdateActionInOpenList(action))
                        {
                            openNodes.Add(new AINode(agent, planner, action, currentNode));
                        }
                    }
                }

                if (openNodes.Count == 0) return false;

                currentNode = FindCheapestNode();
                closedNodes.Add(currentNode);
                openNodes.Remove(currentNode);
            }
            
            return true;
        }

        public Queue<AIAction> GetActionPlan()
        {
            Queue<AIAction> queue = new Queue<AIAction>();
            while (currentNode.GetAction() != null)
            {
                queue.Enqueue(currentNode.GetAction());
                currentNode = currentNode.GetParent();
            }

            Queue<AIAction> actionPlan = new Queue<AIAction>();
            while (queue.Count > 0)
            {
                actionPlan.Enqueue(queue.Dequeue());
            }

            return actionPlan;
        }


        private bool ActionAchievesPrecondition(AIAction action)
        {
            foreach (var precondition in currentNode.GetState().GetPreconditions())
            {
                if (action.GetEffects().ContainsKey(precondition.Key) && action.GetEffects()[precondition.Key].Equals(precondition.Value))
                {
                    return true;
                }
            }
            return false;
        }

        private bool UpdateActionInOpenList(AIAction action)
        {
            foreach (var node in openNodes)
            {
                if (node.GetAction() == action)
                {
                    node.Update(currentNode);
                    return true;
                }
            }
            return false;
        }

        private bool ActionNotInClosedNodeList(AIAction action)
        {
            foreach (var node in closedNodes)
            {
                if (node.GetAction() == action)
                {
                    return false;
                }
            }
            return true;
        }

        private AINode FindCheapestNode()
        {
            AINode cheapestNode = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].GetFCost() < cheapestNode.GetFCost())
                {
                    cheapestNode = openNodes[i];
                }
            }
            return cheapestNode;
        }
    }
}
