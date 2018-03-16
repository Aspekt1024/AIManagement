using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    class MoveState : AIMachineState
    {
        private Transform target;

        public override void Tick(float deltaTime)
        {
            if (Vector3.Distance(target.position, agent.Owner.transform.position) < 3f)
            {
                StateComplete();
            }
            else
            {
                agent.Owner.GetComponent<Rigidbody>().velocity = (target.position - agent.Owner.transform.position).normalized * 4f;
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
