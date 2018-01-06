using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using Enemy;
using PlayerUnit;

public class EnemyLowRadar : MonoBehaviour
{
	#region Variables
	[Header("MainParentObject")]
	[SerializeField]
	private GameObject mainParentObject;
	private Enemy.StateController controller;
	[Header("RoomList")]
	[SerializeField]
	private List<GameObject> roomList;
	[Header("Unit Variables")]
	[SerializeField]
	private List<GameObject> unitesInRadarList;
	private NavMeshPath navMeshPath;
	private NavMeshAgent navMeshAgent;
	#endregion

	#region System Functions
	void Start () 
	{
		controller = mainParentObject.GetComponent<Enemy.StateController>();
		roomList = new List<GameObject>();
		unitesInRadarList = new List<GameObject>();
		navMeshAgent = mainParentObject.GetComponent<NavMeshAgent>();
		navMeshPath = new NavMeshPath();
	}

	void Update()
	{
		AssignLockedRoom();
		CheckLockedRoom();
		CheckIfReachableChaseTarget();
		LockedOnUnit();
		AssignUnitTarget();
	}

	void OnTriggerStay(Collider other)
	{
		if((other.tag == "RoomArea") && (other.GetComponent<RoomEntityIdentifier>().roomTreasureScore > 0))
		{
			if(!roomList.Any(i=>i == other.gameObject))
			{
				roomList.Add(other.gameObject);		
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("RTSUnit") && (!other.GetComponent<PlayerUnit.StateController>().fullEngaged))
		{
			if(!unitesInRadarList.Any(i=>i == other.gameObject))
			{
				unitesInRadarList.Add(other.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("RTSUnit"))
		{
			unitesInRadarList.Remove(other.gameObject);
		}
	}

	#endregion
	#region Select Unit Functions

	private void SortUnitList()
	{
		unitesInRadarList.RemoveAll(GameObject => GameObject == null);
		unitesInRadarList.RemoveAll(GameObject => GameObject.GetComponent<PlayerUnit.StateController>().playerUnitStats.currentHealth <= 0);
		unitesInRadarList.RemoveAll(GameObject => GameObject.GetComponent<PlayerUnit.StateController>().fullEngaged == true);
		unitesInRadarList = unitesInRadarList.OrderBy(x=>Vector3.Distance(this.transform.position, x.transform.position)).ToList();
	}


	private void LockedOnUnit()
	{
		if(controller.unitTarget)
		{
			if(Vector3.Distance(this.transform.position, controller.unitTarget.position) <= controller.enemyStats.lockedOnUnitDistance)
			{
				controller.lockedUnitTarget = controller.unitTarget;
				controller.lockedUnitTarget.GetComponent<PlayerUnit.StateController>().UpdateEngageList(controller.gameObject);
			}
		}
	}
		
	private void AssignUnitTarget()
	{
		if ((!controller.lockedUnitTarget) || controller.lockedUnitTarget.GetComponent<PlayerUnit.StateController>().playerUnitStats.currentHealth <= 0)
		{
			SortUnitList();
			if (unitesInRadarList.Count != 0)
			{
				controller.unitTarget = unitesInRadarList.First().transform;
			}
			else
			{
				controller.unitTarget = null;
				controller.lockedUnitTarget = null;
			}
		}
		else
		{
			controller.unitTarget = controller.lockedUnitTarget;
		}
	}
	#endregion
	#region Locked Room Functions
	private void AssignLockedRoom()
	{
		if(!controller.lockedRoomTarget)
		{
			if(roomList.Any(i=>i == controller.roomTarget.gameObject))
			{
				controller.lockedRoomTarget = controller.roomTarget;
			}
			roomList.Clear();
		}
	}

	private void CheckLockedRoom()
	{
		if(controller.lockedRoomTarget)
		{
			if(controller.lockedRoomTarget.GetComponent<RoomEntityIdentifier>().roomTreasureScore <= 0)
			{
				controller.lockedRoomTarget = null;
			}
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
