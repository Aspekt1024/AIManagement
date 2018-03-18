using System.Collections.Generic;

namespace Aspekt.AI
{
    public abstract class AIGoal
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

        protected abstract void SetConditions();

        protected void AddCondition(string label, object value)
        {
            goal.Add(label, value);
        }
    }
}
