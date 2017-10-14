using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTriggerArea : MonoBehaviour
{
    public int roomTotalHitPoints = 10;
    public int roomCurrentHitPoints;
    public int minionTotalPoints;
    public int playerTotalPoints;

	// Use this for initialization
	void Start ()
    {
        roomCurrentHitPoints = roomTotalHitPoints;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Minion")
        {
            Debug.Log("Enemy in room");
        }

        else if (other.tag == "RTSUnit")
        {
            Debug.Log("Ally in room");
        }
    }
}
