using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class StageManager : MonoBehaviour {

	public Transform finalTarget;
	public bool aiActivation;

	void Start()
	{

	}

	void Update()
	{
		GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
		foreach(GameObject minion in minions)
		{
			minion.GetComponent<StateController>().SetupAI(aiActivation, finalTarget);
		}
	}
}
