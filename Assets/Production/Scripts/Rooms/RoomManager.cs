using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [Header("TAGS for Treasure items ")]
    public string[] treasureItemTags;
    [Header("TAGS for Threat items/Mobs etc ")]
    public string[] threatTags;

    [Header("Set Room treasureScore color codes")]
    public Color lowTreasureColor;
    public Color midTreasureColor;
    public Color highTreasureColor;

    [Header("Set Room treasureScore Thresholds")]
    public int lowTreasureScore;
    public int midTreasureScore;
    public int highTreasureScore;

    [Header("Total Treasure")]
    public float totalTreasure;

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update () 
    {
        CalculateTotalRoomTreasure();
    }

    public void ShowTreasureInRooms()
    {
        GameObject[] roomTreasures = GameObject.FindGameObjectsWithTag("RoomArea");
        foreach (GameObject room in roomTreasures)
        {
            if (room.GetComponent<Renderer>().enabled == true)
            {
                room.GetComponent<Renderer>().enabled = false;
            }
            else if (room.GetComponent<Renderer>().enabled == false)
            {
                room.GetComponent<Renderer>().enabled = true;
                SetTreasureRoomColor(room);
            }
        }
    }

    private void SetTreasureRoomColor(GameObject room)
    {
        
            RoomEntityIdentifier roomTS = room.GetComponent<RoomEntityIdentifier>();
            if (roomTS.roomTreasureScore <= lowTreasureScore)
            {
                Debug.Log("Room " + room.name + " has treasure score of " + roomTS.roomThreatScore);
                room.GetComponent<Renderer>().material.color = lowTreasureColor;
            }
            else if (roomTS.roomTreasureScore > lowTreasureScore && roomTS.roomTreasureScore <= midTreasureScore)
            {
                Debug.Log("Room " + room.name + " has treasure score of " + roomTS.roomThreatScore);
                room.GetComponent<Renderer>().material.color = midTreasureColor;
            }
            else if (roomTS.roomTreasureScore > midTreasureScore)
            {
                room.GetComponent<Renderer>().material.color = highTreasureColor;
                Debug.Log("Room " + room.name + " has treasure score of " + roomTS.roomThreatScore);
            }
    }


    public void CalculateTotalRoomTreasure()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("RoomArea");
        totalTreasure = 0;
        foreach (GameObject room in rooms)
        {
            RoomEntityIdentifier roomScript = room.GetComponent<RoomEntityIdentifier>();
            float roomTreasure = roomScript.roomTreasureScore;
            totalTreasure += roomTreasure;
        }
        GameMainManager.Instance._totalRoomTreasure = totalTreasure;
    }
    
}
