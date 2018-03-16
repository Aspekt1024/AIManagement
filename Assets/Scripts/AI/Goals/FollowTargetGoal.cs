using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class FollowTargetGoal : AIGoal
    {
        protected override void SetConditions()
        {
            AddCondition("Reached Target", true);
        }

    }
}
