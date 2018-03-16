using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class TestUnit : MonoBehaviour {

    private AIAgent ai;

	// Use this for initialization
	void Start () {
        ai = GetComponentInChildren<AIAgent>();
        ai.Activate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
