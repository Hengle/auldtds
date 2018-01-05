using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerUnit;
using UnityEngine.AI;
using System.Linq;

public class UnitRadar : MonoBehaviour 
{
	#region Variables
	[Header("MainParentObject")]
	[SerializeField]
	private GameObject mainParentObject;
	private StateController controller;
	[Header("Enemy Variables")]
	[SerializeField]
	private List<GameObject> enemiesInRadarList;
	private NavMeshPath navMeshPath;
	private NavMeshAgent navMeshAgent;
	#endregion

	#region System Functions
	void Start () 
	{
		controller = mainParentObject.GetComponent<StateController>();
		enemiesInRadarList = new List<GameObject>();
		navMeshAgent = mainParentObject.GetComponent<NavMeshAgent>();
		navMeshPath = new NavMeshPath();
	}

	void Update()
	{
		CheckIfReachableChaseTarget();
		AssignUnitTarget();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Minion"))
		{
			if(!enemiesInRadarList.Any(i=>i == other.gameObject))
			{
				enemiesInRadarList.Add(other.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Minion"))
		{
			enemiesInRadarList.Remove(other.gameObject);
		}
	}
	#endregion
	#region Select Enemy Functions
	private void SortUnitList()
	{
		enemiesInRadarList.RemoveAll(GameObject => GameObject == null);
		enemiesInRadarList = enemiesInRadarList.OrderBy(x=>Vector3.Distance(this.transform.position, x.transform.position)).ToList();
	}
		
	private void AssignUnitTarget()
	{
		SortUnitList();
		if (enemiesInRadarList.Count != 0)
		{
			controller.enemyTarget = enemiesInRadarList.First().transform;
		}
		else
		{
			controller.enemyTarget = null;
		}
	}
	#endregion
	#region Checks Functions
	private void CheckIfReachableChaseTarget()
	{
		if(controller.chaseTarget)
		{
			navMeshAgent.CalculatePath(controller.chaseTarget.position, navMeshPath);
			if(navMeshPath.status == NavMeshPathStatus.PathComplete)
			{
				controller.isChaseTargetReachable = true;
			}
			else
			{
				controller.isChaseTargetReachable = false;
			}
		}
		else
		{
			controller.isChaseTargetReachable = true;
		}
	}
	#endregion
}
