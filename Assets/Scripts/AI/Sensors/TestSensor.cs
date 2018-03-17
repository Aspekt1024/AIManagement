using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aspekt.AI;

public class TestSensor : AISensor {

    public TestSensor(AIAgent agent) : base(agent)
    {
        this.agent = agent;
    }

    private void Update()
    {
        Transform target = GameObject.Find("Cube").transform;
        if (Vector3.Distance(target.position, agent.transform.position) > 4f)
        {
            agent.GetMemory().UpdateCondition("Reached Target", false);
        }
    }
}
