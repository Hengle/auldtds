using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavmeshDestination : MonoBehaviour {


    private NavMeshAgent myAgent;
    public Transform destination;


	// Use this for initialization
	void Start ()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.SetDestination(destination.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //myAgent.destination = destination.position;
	}
}
