using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt
{
    public abstract class AIGoal
    {
        private Dictionary<string, object> goal = new Dictionary<string, object>();

        public AIGoal()
        {
            SetConditions();
        }

        public Dictionary<string, object> GetConditions()
        {
            return goal;
        }

        protected abstract void SetConditions();

        protected void AddCondition(string label, object value)
        {
            goal.Add(label, value);
        }
    }
}
