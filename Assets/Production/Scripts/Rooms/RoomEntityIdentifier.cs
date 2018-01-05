using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomEntityIdentifier : MonoBehaviour
{

    public List<GameObject> gameObjectsInRoom = new List<GameObject>();
    public float roomThreatScore;
    public float roomTreasureScore;
    public float currentTreasureScore;
    public float roomTotalScore;
    public bool isDestroyed = false;

    // Use this for initialization
    private void OnEnable()
    {
        SceneManager.sceneLoaded += InitializeThis;
    }
    private void Awake()
    {
    
    }
    void InitializeThis(Scene scene, LoadSceneMode mode)
    {
        CalculateRoomTotalPoints();
    }
    private void Start()
    {
        currentTreasureScore = 0;	//current
    }

    void Update()
	{
		CalculateRoomTotalPoints();
		TreasureScoreNeverNegative();
        DestroyRoom();
        //GM_UpdateTreasure();

    }

	private void CalculateRoomTotalPoints()
	{
		roomTotalScore = roomTreasureScore - roomThreatScore;
	}

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Found Items in the room-Trigger Stay");
        if (other.tag == "TreasureItem")
        {
            //roomTreasureScore = 0;
            if (ItemNotInList(other.gameObject))
            {
                gameObjectsInRoom.Add(other.gameObject);
                BasicItem basicItemValues =  other.gameObject.GetComponent<BasicItem>();
                roomTreasureScore += basicItemValues.itemStats.itemTreasureFactor;
                //Debug.Log("GO added in the List as " + other.gameObject.name + " score:" + basicItemValues.itemStats.itemTreasureFactor);

            }
        }

        if (other.tag == "RTSUnit")
        {
            //Debug.Log("Unit Stays the room");
            if (ItemNotInList(other.gameObject))
            {
                gameObjectsInRoom.Add(other.gameObject);
                UnitAttributes basicUnitAttributes = other.gameObject.GetComponent<UnitAttributes>();
                roomThreatScore += basicUnitAttributes.unitBaseAttributes.unitThreatFactor;
                //Debug.Log("GO added in the List as " + other.gameObject.name + " score:" + basicItemValues.itemStats.itemTreasureFactor);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Item Entered the room");
        if (other.tag == "TreasureItem")
        {
            //roomTreasureScore = 0;
            if (ItemNotInList(other.gameObject))
            {
                gameObjectsInRoom.Add(other.gameObject);
                BasicItem basicItemValues = other.gameObject.GetComponent<BasicItem>();
                roomTreasureScore += basicItemValues.itemStats.itemTreasureFactor;
                //Debug.Log("GO added in the List as " + other.gameObject.name + " score:"+basicItemValues.itemStats.itemTreasureFactor);
            }
        }

       if (other.tag == "RTSUnit")
        {
            //Debug.Log("Unit Entered the room");
            if (ItemNotInList(other.gameObject))
            {
                gameObjectsInRoom.Add(other.gameObject);
                UnitAttributes basicUnitAttributes = other.gameObject.GetComponent<UnitAttributes>();
//UPFRADE                roomThreatScore += basicUnitAttributes.unitBaseAttributes.unitThreatFactor;
                //Debug.Log("GO added in the List as " + other.gameObject.name + " score:" + basicItemValues.itemStats.itemTreasureFactor);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TreasureItem")
        {
            if (!ItemNotInList(other.gameObject))
            {
                gameObjectsInRoom.Remove(other.gameObject);
                BasicItem basicItemValues = other.gameObject.GetComponent<BasicItem>();
//UPGRADE                roomTreasureScore -= basicItemValues.itemStats.itemTreasureFactor;
                //Debug.Log("GO Removed from the List as " + other.gameObject.name + " score:" + basicItemValues.itemStats.itemThreatFactor);
            }
        }

        if (other.tag == "RTSUnit")
        {
            Debug.Log("Unit Left the room");
            if (!ItemNotInList(other.gameObject))
            {
                gameObjectsInRoom.Remove(other.gameObject);
                UnitAttributes basicUnitAttributes = other.gameObject.GetComponent<UnitAttributes>();
//UPGRADE                roomThreatScore -= basicUnitAttributes.unitBaseAttributes.unitThreatFactor;
                //Debug.Log("GO added in the List as " + other.gameObject.name + " score:" + basicItemValues.itemStats.itemTreasureFactor);

            }
        }
    }

    private bool ItemNotInList(GameObject gameObjectItem)
    {
        if (!gameObjectsInRoom.Contains(gameObjectItem))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DestroyRoom()
    {
		if (roomTreasureScore <= 0)
        {
			isDestroyed = true;
            int childrenNumber = this.transform.childCount;
            //Debug.Log("Found " + childrenNumber);
            for (int i = 0; i < childrenNumber; i++)
            {
                GameObject childObject = this.transform.GetChild(i).gameObject;
                if (childObject.name == "FireEffect1")
                {
                    childObject.SetActive(true);
                }
            }
        }
    }

    private void GM_UpdateTreasure()
    {
        if (roomTreasureScore != currentTreasureScore)
        {
            //GameMainManager.Instance._totalRoomTreasure = 0;
			GameMainManager.Instance._totalRoomTreasure += (roomTreasureScore - currentTreasureScore);
            currentTreasureScore = roomTreasureScore;
        }
    }

	private void TreasureScoreNeverNegative()
	{
		if (roomTreasureScore < 0)
		{
			roomTreasureScore = 0;
		}
	}

}