using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI
{
    public class AIMemory
    {
        private Dictionary<string, object> state = new Dictionary<string, object>();

        public bool ConditionMet(string label, object value)
        {
            if (state.ContainsKey(label))
            {
                return state[label] == value;
            }
            else
            {
                return false;
            }
        }

        public void UpdateCondition(string label, object newValue)
        {
            if (state.ContainsKey(label))
            {
                state[label] = newValue;
            }
            else
            {
                state.Add(label, newValue);
            }
        }

        public Dictionary<string, object> GetState()
        {
            return state;
        }

        public Dictionary<string, object> CloneState()
        {
            return new Dictionary<string, object>(state);
        }
    }
}
