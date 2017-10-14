using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinionRadarHighEnd : MonoBehaviour
{
	#region Classes
	[System.Serializable]
	public class Room
	{
		public GameObject room;
		public float roomPoints;
	}
	#endregion

	#region Properties
	[Header ("Room")]
	[SerializeField]
	private List<Room> roomList;
	[SerializeField]
	private GameObject targetRoom;
	#endregion

	#region System Functions
	// Use this for initialization

	void Start () 
	{
		roomList = new List<Room>();
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
			Room roomClass = new Room();
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
	static int SortByPoints(Room tr1, Room tr2)
	{
		return tr2.roomPoints.CompareTo(tr1.roomPoints);
	}

	private void AssignTargetRoom()
	{
			this.GetComponentInParent<MinionAI>().roomTarget = targetRoom;
			roomList.Clear();
	}
	#endregion
}

