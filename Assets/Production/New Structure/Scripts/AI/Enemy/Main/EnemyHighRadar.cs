using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enemy;

public class EnemyHighRadar: MonoBehaviour
{
	#region Classes
	[System.Serializable]
	public class RoomTarget
	{
		public GameObject room;
		public float roomPoints;
	}
	#endregion

	#region Properties
	[Header("MainParentObject")]
	[SerializeField]
	private GameObject mainParentObject;
	[Header ("Room")]
	[SerializeField]
	private List<RoomTarget> roomList;
	[SerializeField]
	private GameObject targetRoom;
	#endregion

	#region System Functions
	// Use this for initialization

	void Start () 
	{
		roomList = new List<RoomTarget>();
	}

	// Update is called once per frame
	void Update () 
	{
		AssignTargetRoom();
	}

	void OnTriggerStay(Collider other)
	{
		if((other.tag == "RoomArea") && (other.GetComponent<RoomEntityIdentifier>().roomTreasureScore > 0))
		{
			RoomTarget roomClass = new RoomTarget();
			roomClass.room = other.gameObject;
			roomClass.roomPoints = other.GetComponent<RoomEntityIdentifier>().roomTotalScore;
			if(!roomList.Any(i=>i.room == other.gameObject))
			{
				roomList.Add(roomClass);
				roomList.Sort(SortByPoints);
				targetRoom = roomList[0].room;
			}
		}
		else if(roomList.All(i=>i.roomPoints == 0))
		{
			targetRoom = null;
		}
	}
	#endregion

	#region Select Room Functions
	static int SortByPoints(RoomTarget tr1, RoomTarget tr2)
	{
		return tr2.roomPoints.CompareTo(tr1.roomPoints);
	}

	private void AssignTargetRoom()
	{
		if(targetRoom != null)
		{
			if(!mainParentObject.GetComponentInParent<StateController>().lockedRoomTarget)
			{
				mainParentObject.GetComponentInParent<StateController>().roomTarget = targetRoom.transform;
				roomList.Clear();
			}
			else
			{
				mainParentObject.GetComponentInParent<StateController>().roomTarget = mainParentObject.GetComponentInParent<StateController>().lockedRoomTarget;
			}
		}
		else
		{
			mainParentObject.GetComponentInParent<StateController>().roomTarget = null;
		}
	}
	#endregion
}
