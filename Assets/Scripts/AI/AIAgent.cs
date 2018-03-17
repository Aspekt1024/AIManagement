﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    // TODO pause/unpause states when awaiting and receiving plan
    public class AIAgent : MonoBehaviour
    {
        public GameObject Owner;

        private List<AIGoal> goals;
        private List<AIAction> actions;

        private AIAgentState agentState;
        private AIPlanner planner;
        private AIExecutor executor;

        private enum States
        {
            Stopped, Active, Paused, FindNewGoal
        }
        private States state;

        private void Awake()
        {
            Owner = GetComponentInParent<TestUnit>().gameObject;
            agentState = new AIAgentState();
            planner = new AIPlanner(this);
            executor = new AIExecutor(this);

            // TODO defined in editor
            goals = new List<AIGoal>();
            goals.Add(new FollowTargetGoal());

            actions = new List<AIAction>();
            actions.Add(new MoveToTargetAction());
            // TODO end defined in editor

            executor.OnFinishedPlan += FindNewGoal;
            planner.OnActionPlanFound += PlanFound;
        }

        private void OnDestroy()
        {
            executor.OnFinishedPlan -= FindNewGoal;
            planner.OnActionPlanFound -= PlanFound;
        }

        private void Update()
        {
            switch (state)
            {
                case States.Active:
                    executor.Tick(Time.deltaTime);
                    break;
                case States.Paused:
                    break;
                case States.Stopped:
                    break;
                case States.FindNewGoal:
                    planner.CalculateNewGoal();
                    break;
                default:
                    break;
            }
        }

        public void Activate()
        {
            if (state == States.Active) return;
            state = States.FindNewGoal;
        }

        public void Unpause()
        {
            if (state == States.Paused)
            {
                executor.Unpause();
                state = States.Active;
            }
        }

        public void Stop()
        {
            if (state == States.Stopped) return;
            executor.Stop();
            state = States.Stopped;
        }

        public void Pause()
        {
            if (state != States.Active) return;
            executor.Pause();
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

        private void PlanFound()
        {
            if (state != States.FindNewGoal) return;
            state = States.Active;
            executor.ExecutePlan(planner.GetActionPlan());
        }
    }
}
