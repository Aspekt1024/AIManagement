using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class AIAction
    {
        private Dictionary<string, object> preconditions = new Dictionary<string, object>();
        private Dictionary<string, object> effects = new Dictionary<string, object>();
        
        public event Action OnFailure = delegate { };
        public event Action OnSuccess = delegate { };

        public AIAction()
        {
            SetPreconditions();
            SetEffects();
        }
        
        public abstract void Run();
        protected abstract void SetPreconditions();
        protected abstract void SetEffects();

        public void Tick(float deltaTime)
        {
            if (CheckProceduralPrecondition())
            {
                Run();
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
            if (OnFailure != null)
            {
                OnFailure();
            }
        }

        protected void Success()
        {
            if (OnSuccess != null)
            {
                OnSuccess();
            }
        }
    }
}
