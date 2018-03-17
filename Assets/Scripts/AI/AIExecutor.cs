using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI
{
    public class AIExecutor
    {
        public event Action OnFinishedPlan = delegate { };

        private Queue<AIAction> actionPlan;
        private AIAction currentAction;
        private AIGoal currentGoal;
        private AIStateMachine stateMachine;
        private AIAgent agent;

        private enum States
        {
            Stopped, Running, Paused
        }
        private States state;

        public AIExecutor(AIAgent agent)
        {
            this.agent = agent;
            stateMachine = new AIStateMachine(agent);
        }
        
        public void Tick(float deltaTime)
        {
            if (currentAction == null) return;

            switch (state)
            {
                case States.Stopped:
                    break;
                case States.Running:
                    currentAction.Tick(deltaTime);
                    stateMachine.Tick(deltaTime);
                    break;
                case States.Paused:
                    break;
                default:
                    break;
            }

        }

        public void ExecutePlan(Queue<AIAction> newActionPlan, AIGoal goal)
        {
            if (currentAction != null)
            {
                stateMachine.Stop();
                stateMachine.Activate();    // TODO.. stop activate should be simpler
            }
            currentGoal = goal;
            actionPlan = newActionPlan;
            BeginNextAction();
        }

        public void Stop()
        {
            if (state == States.Stopped)
            {
                Debug.Log("Already stopped");
            }
            else
            {
                state = States.Stopped;
                stateMachine.Stop();
                actionPlan = null;
                currentAction = null;
            }
        }

        public void Pause()
        {
            if (state == States.Running)
            {
                state = States.Paused;
                stateMachine.Pause();
            }
            else
            {
                Debug.Log("Cannot Pause when not running. Current state = " + state.ToString());
            }
        }

        public void Unpause()
        {
            if (state == States.Paused)
            {
                state = States.Running;
                // TODO Unpause state machine
            }
            else
            {
                Debug.Log("Error, cannot unpause from non-paused state. Current state = " + state.ToString());
            }
        }

        private void BeginNextAction()
        {
            if (currentAction != null)
            {
                currentAction.OnSuccess -= ActionSuccess;
                currentAction.OnFailure -= ActionFailure;
            }

            if (actionPlan.Count == 0)
            {
                currentAction = null;
                state = States.Stopped;
                if (OnFinishedPlan != null) OnFinishedPlan();
            }
            else
            {
                state = States.Running;

                currentAction = actionPlan.Dequeue();
                currentAction.OnSuccess += ActionSuccess;
                currentAction.OnFailure += ActionFailure;
                currentAction.Enter(stateMachine);
            }
        }

        private void ActionSuccess()
        {
            foreach (var effect in currentAction.GetEffects())
            {
                agent.GetMemory().UpdateCondition(effect.Key, effect.Value);
            }

            bool goalAchieved = true;
            foreach (var condition in currentGoal.GetConditions())
            {
                if (agent.GetMemory().ConditionMet(condition.Key, condition.Value) == false)
                {
                    goalAchieved = false;
                    break;
                }
            }

            if (goalAchieved)
            {
                Stop();
                if (OnFinishedPlan != null) OnFinishedPlan();
            }
            else
            {
                BeginNextAction();
            }
        }

        private void ActionFailure()
        {
            Debug.Log("Action failed: " + currentAction.ToString());
            Stop();
            if (OnFinishedPlan != null) OnFinishedPlan();
        }
    }
}
