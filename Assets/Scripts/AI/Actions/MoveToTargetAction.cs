using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class MoveToTargetAction : AIAction
    {
        public override bool CheckProceduralPrecondition()
        {
            return base.CheckProceduralPrecondition();
        }
        
        public override void Run()
        {

            Success();
        }

        protected override void SetPreconditions()
        {

        }

        protected override void SetEffects()
        {
            AddEffect("Reached Target", true);
        }
    }
}
