using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    class AIStateMachine
    {
        private AIMachineState currentState;
        private Queue<AIMachineState> stateQueue; 

        private enum States
        {
            Stopped, Paused, Active
        }
        private States state;

        public AIStateMachine()
        {
            stateQueue = new Queue<AIMachineState>();
            SetIdleState();
        }

        public void Tick(float deltaTime)
        {
            if (currentState.GetType().Equals(typeof(IdleState)) && stateQueue.Count > 0)
            {
                GotoNextState();
            }
            else
            {
                currentState.Tick(deltaTime);
            }
        }

        public bool IsIdle { get { return currentState == null || currentState.GetType().Equals(typeof(IdleState)); } }

        public void Enqueue(AIMachineState newState)
        {
            stateQueue.Enqueue(newState);
        }

        public T CreateState<T>() where T : AIMachineState, new()
        {
            T newState = new T();
            return newState;
        }

        public void Stop()
        {
            state = States.Stopped;
        }

        public void Pause()
        {
            state = States.Paused;
        }

        public void Activate()
        {
            if (state == States.Stopped)
            {
                SetIdleState();
            }
            state = States.Active;
        }

        private void GotoNextState()
        {
            currentState.OnComplete -= StateCompleted;
            currentState = stateQueue.Dequeue();
            currentState.OnComplete += StateCompleted;
            currentState.Enter();
        }

        private void SetIdleState()
        {
            IdleState initialState = CreateState<IdleState>();
            initialState.OnComplete += StateCompleted;
            initialState.Enter();
            currentState = initialState;
        }

        private void StateCompleted()
        {
            if (stateQueue.Count > 0)
            {
                GotoNextState();
            }
            else
            {
                SetIdleState();
            }
        }
    }
}
