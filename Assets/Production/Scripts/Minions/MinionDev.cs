using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class MinionDev : MonoBehaviour
{

	#region Classes
	[System.Serializable]
	public class BlockItems
	{
		public GameObject blockItems;
		public Transform blockItemsTransform;
		public float blockItemsDistance;
	}

	#endregion

    #region Properties
    [Header("NavMesh")]
    private NavMeshAgent navMeshAgent;
	private NavMeshPath navMeshPath;
    public bool setDebugOn = false;
   
	[Header("Rooms & Targets")]
	public Transform destinationTarget;
	public Transform finalDestinationTarget;
	[SerializeField]
	private Transform oldDestinationTarget;
	public bool isBlockDestroyed;
    
	[Header("Animations")]
    public string walkingAnimation;
    public string idleAnimation;

    [Header("BlockItems")]
	[SerializeField]
	private List<BlockItems> blockItemList;
	private BlockItems blockItemsClass;
	[SerializeField]
	private GameObject[] blockItems;
	[SerializeField]
	private GameObject blockItemTarget;
	[SerializeField]
	private bool isPathBlocked;
	#endregion


    // Use this for initialization
	void Awake()
	{
		finalDestinationTarget = GameObject.Find("Treasure").transform;  //Get tranform of the destination Point
	}

	void Start ()
    {
        this.GetComponent<Animation>().CrossFade(walkingAnimation);
        //NavMeshMove
		navMeshAgent = this.GetComponent<NavMeshAgent>();    //Get NavMeshAgent
		navMeshPath = new NavMeshPath();

		destinationTarget = finalDestinationTarget;
		oldDestinationTarget = null;

		blockItemList = new List<BlockItems>();
    }

    void Update()
    {
		MoveToRoom();
    }


    #region NavMesh Functions
	private bool CheckIfRoomIsReachable()
	{
		navMeshAgent.CalculatePath(destinationTarget.position, navMeshPath);
		if(navMeshPath.status == NavMeshPathStatus.PathComplete)
		{
			Debug.Log("Room Access");
			return true;
		}
		else
		{
			Debug.Log("No Room Access");
			return false;
		}
	}

	private void MoveToRoom()
	{
		if(CheckIfSelectedRoomIsTheSame() == false)
		{
			isPathBlocked = false;
		}

		if (isPathBlocked == false)
		{
			if((CheckIfRoomIsReachable() == true))
			{
				navMeshAgent.SetDestination(destinationTarget.position);
				oldDestinationTarget = destinationTarget;
				Debug.Log("Move To Room & Set OldDestiantion");
                if (CheckIfUnitReachPoint())
                {
                    this.GetComponent<Animation>().CrossFade(idleAnimation);
                    navMeshAgent.isStopped = true;
                }
            }
			else
			{
				ChooseBlockItemTarget();
				Debug.Log("Choose Block Item");
			}
		}
	}

	private void CreateListOfBlockItems()
	{
		blockItemList.Clear();
		blockItems = GameObject.FindGameObjectsWithTag("BlockItems");

		for(int i = 0; i < blockItems.Length; i++)
		{
			blockItemsClass = new BlockItems();
			blockItemsClass.blockItems = blockItems[i];
			blockItemsClass.blockItemsTransform = blockItems[i].transform;
			blockItemsClass.blockItemsDistance = Vector3.Distance(destinationTarget.position, blockItems[i].transform.position);
			blockItemList.Add(blockItemsClass);
		}
		blockItemList.Sort(SortBlockItems);
		blockItemList.Reverse();
	}

	private bool CheckIfBlockItemIsReachable(Transform blockItemDestinationTarget)
	{
		navMeshAgent.CalculatePath(blockItemDestinationTarget.position, navMeshPath);
		if(navMeshPath.status == NavMeshPathStatus.PathComplete)
		{
			Debug.Log("Block Item Path is Clear");
			return true;
		}
		else
		{
			Debug.Log("Block Item Path is NOT Clear");
			return false;
		}
	}

	private void ChooseBlockItemTarget()
	{
		isPathBlocked = true;

		CreateListOfBlockItems();
		for(int i = 0; i < blockItemList.Count; i++)
		{
			Debug.Log("Start List Checking");
			blockItemTarget = blockItemList[i].blockItems.transform.Find("BlockPoints").gameObject.transform.Find("FrontPoint").gameObject;
			if (CheckIfBlockItemIsReachable(blockItemTarget.transform) == true)
			{
				navMeshAgent.SetDestination(blockItemTarget.transform.position);
                Debug.Log(navMeshAgent.destination);
                if (CheckIfUnitReachPoint() == true)
                {
                    Debug.Log("Changing Animation and stopping Navmesh");
                    this.GetComponent<Animation>().CrossFade(idleAnimation);
                    navMeshAgent.isStopped = true;
                }
                else
                {
                    Debug.Log("NOT REACHED YET");
                }
                Debug.Log("Set Block Item Target Front Point");
				return;
			}
			else
			{
				blockItemTarget = blockItemList[i].blockItems.transform.Find("BlockPoints").gameObject.transform.Find("BackPoint").gameObject;
				if (CheckIfBlockItemIsReachable(blockItemTarget.transform) == true)
				{
					navMeshAgent.SetDestination(blockItemTarget.transform.position);
                    if (CheckIfUnitReachPoint()==true)
                    {
                        Debug.Log("Changing Animation and stopping Navmesh");
                        this.GetComponent<Animation>().CrossFade(idleAnimation);
                        navMeshAgent.isStopped = true;
                    }
                    else
                    {
                        Debug.Log("NOT REACHED YET");
                    }
                    Debug.Log("Set Block Item Target Back Point");
					return;
				}
			}
			Debug.Log("Block Item not Accessible");
		}
	}

	private bool CheckIfSelectedRoomIsTheSame()
	{
		if(destinationTarget == oldDestinationTarget)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	static int SortBlockItems(BlockItems tr1, BlockItems tr2)
	{
		return tr2.blockItemsDistance.CompareTo(tr1.blockItemsDistance);
	}

    private bool CheckIfUnitReachPoint()
    {
        Debug.Log("Checking Destination Reach Point");
        //navMeshAgent = GetComponent<NavMeshAgent>();
        if (Vector3.Distance(this.gameObject.transform.position, navMeshAgent.destination) <= 0.5f)
        {
            //this.GetComponent<Animation>().CrossFade(idleAnimation);
            Debug.Log("Destination REACHED");
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}
