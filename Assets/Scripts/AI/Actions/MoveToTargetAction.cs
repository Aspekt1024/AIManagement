using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aspekt.AI;

    public class MoveToTargetAction : AIAction
    {
        public Transform target;

        private MoveState moveState;

        public override bool CheckProceduralPrecondition()
        {
            return base.CheckProceduralPrecondition();
        }

        public override void Enter(AIStateMachine stateMachine)
        {
            base.Enter(stateMachine);

            target = GameObject.Find("Cube").transform;
            
            moveState = stateMachine.AddState<MoveState>();
            moveState.SetTarget(target);
        }

        protected override void Update()
        {
            // Use if target changes
            //moveState.SetTarget(target);
        }

        protected override void SetPreconditions()
        {

        }

        protected override void SetEffects()
        {
            AddEffect("Reached Target", true);
        }
    }
