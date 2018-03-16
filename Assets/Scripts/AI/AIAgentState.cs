using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIAgentState
    {
        private Dictionary<string, object> stateInfo = new Dictionary<string, object>();

        public object ConditionMet(string label, object value)
        {
            if (stateInfo.ContainsKey(label))
            {
                return stateInfo[label] == value;
            }
            else
            {
                return false;
            }
        }

        public void UpdateCondition(string label, object newValue)
        {
            if (stateInfo.ContainsKey(label))
            {
                stateInfo[label] = newValue;
            }
            else
            {
                stateInfo.Add(label, newValue);
            }
        }
    }
}
