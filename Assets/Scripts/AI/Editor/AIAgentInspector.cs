using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Linq;

using System.Reflection;

using Aspekt.AI;

[CustomEditor(typeof(AIAgent))]
public class AIAgentInspector : Editor {

    private AIAgent agent;

    private bool newProfileClicked;
    private bool showProfileNameError;
    private string newProfileName = "";

    private int selectedGoalIndex;
    private int selectedActionIndex;

    public override void OnInspectorGUI()
    {
        agent = (AIAgent)target;

        if (EditorGUILayout.Foldout(true, "Agent Profile"))
        {
            if (newProfileClicked)
            {
                DisplayNewProfileOptions();
            }
            else
            {
                DisplayProfileInfo();
            }
        }

        EditorGUILayout.Separator();
        agent.LoggingEnabled = EditorGUILayout.Toggle(new GUIContent("Logging Enabled", "Enable to show logging of the AI"), agent.LoggingEnabled);
        
    }

    private void DisplayNewProfileOptions()
    {
        newProfileName = EditorGUILayout.TextField("New Profile Name", newProfileName);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create"))
        {
            showProfileNameError = false;
            if (NewProfileNameIsValid(newProfileName))
            {
                CreateNewProfile(newProfileName);
            }
            else
            {
                showProfileNameError = true;
            }
        }
        if (GUILayout.Button("Cancel"))
        {
            showProfileNameError = false;
            newProfileClicked = false;
        }
        EditorGUILayout.EndHorizontal();

        if (showProfileNameError)
        {
            EditorGUILayout.HelpBox("'" + newProfileName + "' is not a valid name. It must be unique and non-empty", MessageType.Error);
        }
    }

    private bool NewProfileNameIsValid(string profileName)
    {
        if (profileName == "") return false;
        // TODO check all existing AIAgentProfile in the assembly. If their names are all different, return true, else return false
        return true;
    }

    private void DisplayProfileInfo()
    {
        agent.Profile = (AIAgentProfile)EditorGUILayout.ObjectField(agent.Profile, typeof(AIAgentProfile), false);

        if (agent.Profile == null)
        {
            EditorGUILayout.HelpBox("The profile has not been set", MessageType.Info);
            
            if (GUILayout.Button("Create New Profile"))
            {
                newProfileClicked = true;
            }
        }
        else
        {
            DisplayGoals();
            EditorGUILayout.Space();
            DisplayActions();
        }
    }

    private void DisplayGoals()
    {
        EditorGUILayout.LabelField("Goals:", EditorStyles.boldLabel);
        if (agent.Profile.Goals == null || agent.Profile.Goals.Count == 0)
        {
            EditorGUILayout.HelpBox("You should select at least one goal in order for the AIAgent to do anything!", MessageType.Warning);
        }
        else
        {
            var goalToRemove = new AIAgentProfile.AIGoalSerializable();
            for (int i = 0; i < agent.Profile.Goals.Count; i++)
            {
                var goal = agent.Profile.Goals[i];

                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 50f;
                EditorGUILayout.LabelField(goal.goalName);

                goal.priority = Mathf.Clamp(EditorGUILayout.FloatField("Priority", goal.priority), 0, float.MaxValue);

                if (GUILayout.Button("Remove"))
                {
                    goalToRemove = goal;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                agent.Profile.Goals[i] = goal;
            }

            if (goalToRemove.goalName != null && goalToRemove.goalName != "")
            {
                agent.Profile.Goals.Remove(goalToRemove);
            }
        }

        EditorGUILayout.BeginHorizontal();
        
        string[] allTypes = (from System.Type type in Assembly.GetAssembly(typeof(AIGoal)).GetTypes() where type.IsSubclassOf(typeof(AIGoal)) select type.ToString()).ToArray();

        selectedGoalIndex = EditorGUILayout.Popup(selectedGoalIndex, allTypes);
        if (GUILayout.Button("Add selected goal"))
        {
            agent.Profile.AddGoal(allTypes[selectedGoalIndex], 1);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayActions()
    {
        EditorGUILayout.LabelField("Actions:", EditorStyles.boldLabel);

        if (agent.Profile.Actions == null || agent.Profile.Actions.Count == 0)
        {
            EditorGUILayout.HelpBox("Without any actions, the AIAgent won't do anything!", MessageType.Warning);
        }
        else
        {
            var actionToRemove = new AIAgentProfile.AIActionSerializable();
            for (int i = 0; i < agent.Profile.Actions.Count; i++)
            {
                var action = agent.Profile.Actions[i];

                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 50f;
                EditorGUILayout.LabelField(action.actionName);

                action.cost = EditorGUILayout.FloatField("Cost", action.cost);

                if (GUILayout.Button("Remove"))
                {
                    actionToRemove = action;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                agent.Profile.Actions[i] = action;
            }

            if (actionToRemove.actionName != null && actionToRemove.actionName != "")
            {
                agent.Profile.Actions.Remove(actionToRemove);
            }
        }

        EditorGUILayout.BeginHorizontal();

        string[] allTypes = (from System.Type type in Assembly.GetAssembly(typeof(AIAction)).GetTypes() where type.IsSubclassOf(typeof(AIAction)) select type.ToString()).ToArray();

        selectedActionIndex = EditorGUILayout.Popup(selectedActionIndex, allTypes);
        if (GUILayout.Button("Add selected action"))
        {
            agent.Profile.AddAction(allTypes[selectedActionIndex], 1);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void CreateNewProfile(string ProfileName)
    {

    }
}
