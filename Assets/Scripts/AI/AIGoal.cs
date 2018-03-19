using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI
{
    public abstract class AIGoal : MonoBehaviour
    {
        public float Priority = 1f;

        private Dictionary<string, object> goal = new Dictionary<string, object>();

        public AIGoal()
        {
            SetConditions();
        }

        public Dictionary<string, object> GetConditions()
        {
            return goal;
        }

        public override string ToString()
        {
            return GetType().ToString();
        }

        protected virtual void SetConditions() { }

        protected void AddCondition(string label, object value)
        {
            goal.Add(label, value);
        }

    }
}
