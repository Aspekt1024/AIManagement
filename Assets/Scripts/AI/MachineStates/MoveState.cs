using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    class MoveState : AIMachineState
    {
        private Transform target;
        private const float speed = 4f;

        public override void Tick(float deltaTime)
        {
            if (Vector3.Distance(target.position, agent.Owner.transform.position) < 4f)
            {
                // TODO this should be done somewhere else
                agent.Owner.GetComponent<Rigidbody>().velocity = Vector3.Lerp(agent.Owner.GetComponent<Rigidbody>().velocity, Vector3.zero, deltaTime);
                StateComplete();
            }
            else
            {
                agent.Owner.GetComponent<Rigidbody>().velocity = (target.position - agent.Owner.transform.position).normalized * speed;
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
