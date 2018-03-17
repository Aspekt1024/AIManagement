using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIExecutor
    {
        public event Action OnFinishedPlan = delegate { };

        private Queue<AIAction> actionPlan;
        private AIAction currentAction;
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
            stateMachine = new AIStateMachine();
            stateMachine.Activate();
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

        public void ExecutePlan(Queue<AIAction> newActionPlan)
        {
            if (currentAction != null)
            {
                stateMachine.Stop();
                stateMachine.Activate();    // TODO.. stop activate should be simpler
            }
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
            if (actionPlan.Count == 0)
            {
                currentAction = null;
                state = States.Stopped;
                if (OnFinishedPlan != null) OnFinishedPlan();
            }
            else
            {
                state = States.Running;
                if (currentAction != null)
                {
                    currentAction.OnSuccess -= ActionSuccess;
                    currentAction.OnFailure -= ActionFailure;
                }

                currentAction = actionPlan.Dequeue();
                currentAction.OnSuccess += ActionSuccess;
                currentAction.OnFailure += ActionFailure;
                currentAction.Enter();
                // TODO order matters... maybe find a better way to do this
                if (currentAction.RequiresMove)
                {
                    MoveState newMoveState = new MoveState();
                    newMoveState.SetParentAgent(agent);
                    newMoveState.SetTarget(((MoveToTargetAction)currentAction).target);
                    stateMachine.Enqueue(newMoveState);
                }
            }
        }

        private void ActionSuccess()
        {
            // apply action effects
            // check if goal has been achieved
            BeginNextAction();
        }

        private void ActionFailure()
        {
            Debug.Log("Action failed: " + currentAction.ToString());
            Stop();
        }
    }
}
