using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinionRadarLowEnd : MonoBehaviour
{
	#region Classes

	#endregion

	#region Variables
	[Header("General Variables")]
	private MinionAI minionAI;
	[SerializeField]
	private GameObject parentObject;

	[Header("Unit Variables")]
	[SerializeField]
	private List<GameObject> unitesInRadarList = new List<GameObject>();
	[SerializeField]
	private GameObject unitTarget;

	[Header("BlockItem Variables")]
	[SerializeField]
	private List<GameObject> blockItemsInRadarList = new List<GameObject>();
	#endregion

	#region System Functions
	// Use this for initialization
	void Start () 
	{
		minionAI = parentObject.GetComponent<MinionAI>();
	}

	// Update is called once per frame
	void Update () 
	{
		AssignUnitTarget();
		FindActiveBlockItem();
	}

	void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("RTSUnit")) && (!other.GetComponent<ObjectEngage>().fullEngaged))
		{
			if(!unitesInRadarList.Any(i=>i == other.gameObject))
			{
				unitesInRadarList.Add(other.gameObject);
			}
		}

		if (other.CompareTag("BlockItems"))
		{
			if(!blockItemsInRadarList.Any(i=>i == other.gameObject))
			{
				blockItemsInRadarList.Add(other.gameObject);
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
		unitesInRadarList.RemoveAll(GameObject => GameObject.GetComponent<ObjectEngage>().fullEngaged == true);
		unitesInRadarList = unitesInRadarList.OrderBy(x=>Vector3.Distance(this.transform.position, x.transform.position)).ToList();
	}

	private void AssignUnitTarget()
	{
		if (!minionAI.lockedOnUnit)
		{
			SortUnitList();
			if (unitesInRadarList.Count != 0)
			{
				unitTarget = unitesInRadarList.First();
				this.GetComponentInParent<MinionAI>().unitTarget = unitTarget;
			}
			else
			{
				unitTarget = null;
				this.GetComponentInParent<MinionAI>().unitTarget = unitTarget;
			}
		}
	}
	#endregion

	#region Block Items Functions
	private void FindActiveBlockItem()
	{
		if (minionAI.destinationTarget != null)
		{
			if(minionAI.destinationTarget.CompareTag("BlockItemPoints"))
			{
				foreach(GameObject blockItem in blockItemsInRadarList)
				{
					if (blockItem == minionAI.destinationTarget.transform.parent.parent.gameObject)
					{
						minionAI.closeToBlockItem = true;
					}
				}
			}
		}
	}
	#endregion
}