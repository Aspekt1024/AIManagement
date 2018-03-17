using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI
{
    public abstract class AIAction
    {
        public event Action OnFailure = delegate { };
        public event Action OnSuccess = delegate { };

        private Dictionary<string, object> preconditions = new Dictionary<string, object>();
        private Dictionary<string, object> effects = new Dictionary<string, object>();
        private bool isRunning;
        private AIStateMachine stateMachine;

        public AIAction()
        {
            SetPreconditions();
            SetEffects();
        }

        public virtual void Enter(AIStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            stateMachine.OnComplete += Success;
        }

        private void Exit()
        {
            stateMachine.OnComplete -= Success;
        }

        protected abstract void Update();
        protected abstract void SetPreconditions();
        protected abstract void SetEffects();

        public void Tick(float deltaTime)
        {
            if (CheckProceduralPrecondition())
            {
                Update();
            }
            else
            {
                Failure();
            }
        }

        public virtual bool CheckProceduralPrecondition()
        {
            // TODO check all preconditions are still fulfilled
            return true;
        }
        
        public Dictionary<string, object> GetPreconditions()
        {
            return preconditions;
        }

        public Dictionary<string, object> GetEffects()
        {
            return effects;
        }

        protected virtual void AddPrecondition(string label, object value)
        {
            preconditions.Add(label, value);
        }

        protected virtual void AddEffect(string label, object value)
        {
            effects.Add(label, value);
        }

        protected void Failure()
        {
            Exit();
            if (OnFailure != null)
            {
                OnFailure();
            }
        }

        protected void Success()
        {
            Exit();
            if (OnSuccess != null)
            {
                OnSuccess();
            }
        }
    }
}
