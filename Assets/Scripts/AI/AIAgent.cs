using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        public GameObject Owner;

        private List<AIGoal> goals;
        private List<AIAction> actions;

        private AIAgentState agentState;
        private AIStateMachine stateMachine;
        private AIPlanner planner;

        private enum States
        {
            Stopped, Active, Paused, FindNewGoal
        }
        private States state;

        private void Awake()
        {
            Owner = GetComponentInParent<TestUnit>().gameObject;
            agentState = new AIAgentState();
            stateMachine = new AIStateMachine(this);
            planner = new AIPlanner(this);

            // TODO defined in editor
            goals = new List<AIGoal>();
            goals.Add(new FollowTargetGoal());

            actions = new List<AIAction>();
            actions.Add(new MoveToTargetAction());
            // TODO end defined in editor

            planner.OnPlannerIdle += FindNewGoal;
        }

        private void OnDestroy()
        {
            planner.OnPlannerIdle -= FindNewGoal;
        }

        private void Update()
        {
            switch (state)
            {
                case States.Active:
                    stateMachine.Tick(Time.deltaTime);
                    planner.Run();
                    break;
                case States.Paused:
                    break;
                case States.Stopped:
                    break;
                case States.FindNewGoal:
                    planner.CalculateNewGoal();
                    state = States.Active;
                    break;
                default:
                    break;
            }
        }

        public void Activate()
        {
            if (state == States.Active) return;
            stateMachine.Activate();
            state = States.Active;
        }

        public void Stop()
        {
            if (state == States.Stopped) return;
            stateMachine.Stop();
            state = States.Stopped;
        }

        public void Pause()
        {
            if (state == States.Paused || state == States.Stopped) return;
            stateMachine.Pause();
            state = States.Paused;
        }

        public List<AIAction> GetActions()
        {
            return actions;
        }

        public List<AIGoal> GetGoals()
        {
            return goals;
        }
        
        private void FindNewGoal()
        {
            state = States.FindNewGoal;
        }
    }
}
