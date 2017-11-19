using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEngage : MonoBehaviour {
	#region Variables
	[SerializeField]
	private int totalEngagePoints;
	public List<GameObject> engageEnemiesList;
	public bool fullEngaged;
	#endregion


	// Use this for initialization
	void Start () 
	{
		GetTotalEngagePoints();
		engageEnemiesList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckEngageTable();
	}

	private void GetTotalEngagePoints()
	{
		if (this.tag == "RTSUnit")
		{
			totalEngagePoints = this.GetComponent<UnitAttributes>().unitBaseAttributes.unitEnemyEngagePoints;
		}
		else if(this.tag == "BlockItems")
		{
			totalEngagePoints = this.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitEnemyEngagePoints;
		}
	}

	public void FullEngageTable(GameObject enemy)
	{
		if (engageEnemiesList.Count < totalEngagePoints)
		{
			engageEnemiesList.Add(enemy);
		}
	}

	private void CheckEngageTable()
	{
		for (int i = 0; i <engageEnemiesList.Count; i++)
		{
			if (engageEnemiesList[i] == null)
			{
				engageEnemiesList.Remove(engageEnemiesList[i]);
			}
			else
			{
				if(this.gameObject.CompareTag("BlockItems"))
				{
					if(engageEnemiesList[i].GetComponent<MinionAI>().destinationTarget.transform.parent.parent.gameObject != this.gameObject)
					{
						engageEnemiesList.Remove(engageEnemiesList[i]);
					}
				}
			}
		}

		if (engageEnemiesList.Count < totalEngagePoints)
		{
			fullEngaged = false;
		}
		else
		{
			fullEngaged = true;
		}
	}
}
