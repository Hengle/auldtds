using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMovingToTarget : MonoBehaviour {
	#region Properties
	[Header("NavMesh")]
	private NavMeshAgent navAgent;
	private Transform destinationTarget;
	#endregion

	#region System Functions
	// Use this for initialization
	void Start () 
	{
		//NavMeshMove
		navAgent = GetComponent<NavMeshAgent>();	//Get NavMeshAgent
		destinationTarget = GameObject.Find("Treasure").transform;	//Get tranform of the destination Point
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveThroughNavMesh();
	}
	#endregion

	#region NavMesh Functions
	private void MoveThroughNavMesh()
	{
		if(destinationTarget)
		{
			navAgent.SetDestination(destinationTarget.position);
		}
	}
	#endregion
}
