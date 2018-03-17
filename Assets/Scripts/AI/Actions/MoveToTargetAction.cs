using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class MoveToTargetAction : AIAction
    {
        public Transform target;

        public override bool CheckProceduralPrecondition()
        {
            return base.CheckProceduralPrecondition();
        }

        public override void Enter()
        {
            target = GameObject.Find("Cube").transform;
            RequiresMove = true;
        }

        protected override void Update()
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
