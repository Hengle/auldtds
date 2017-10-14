using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionSave : MonoBehaviour {

	#region Classes
	[System.Serializable]
	public class BlockItems
	{
		public GameObject blockItems;
		public float blockItemsDistance;
	}
	#endregion

	#region Properties
	public enum MinionState {SelectTarget, CheckTarget, MoveToTarget, ReachTarget}

	[Header("State")]
	[SerializeField]
	private MinionState state;

	[Header("Targets")]
	public GameObject destinationTarget;
	[SerializeField]
	private GameObject saveRoomTarget;
	[SerializeField]
	private GameObject blockItemTarget;
	[SerializeField]
	private GameObject unitBlockItemTarget;
	[SerializeField]
	private GameObject finalTarget;
	[SerializeField]
	private float reachDistance;
	public GameObject unitTarget;

	[Header("Radars")]
	public GameObject roomTarget;
	public GameObject lockedRoomTarget;
	[SerializeField]
	private GameObject lockedRoomDestination;
	private bool roomTargetIsLocked;

	[Header("NavMesh")]
	private NavMeshAgent navMeshAgent;
	private NavMeshPath navMeshPath;
	private float minionMovingSpeed;

	[Header("BlockItemList")]
	[SerializeField]
	private List<BlockItems> blockItemList;
	private BlockItems blockItemsClass;
	private GameObject[] blockItems;

	[Header("Debug")]
	[SerializeField]
	private bool activeDebug;

	[Header("Animations")]
	[SerializeField]
	private string walkingAnimation;
	[SerializeField]
	private string reachDestinationAnimation;
	[SerializeField]
	private string reachBlockItemAnimation;
	[SerializeField]
	private string reachRTSUnitAnimation;
	private float timeLengthAnimation;
	private Animation anim;

	[Header("Positioning")]
	private bool faceTargetOnce;
	[SerializeField]
	private float minionReach;

	[Header("Do Damage")]
	[SerializeField]
	private float attackSpeed;
	private bool isDoDamageExecuting;

	[Header("Loot Room")]
	[SerializeField]
	private int minionTreasureCurry;
	private bool isMinionLooting;
	#endregion

	#region System Functions
	void Awake()
	{
		SetNavMeshAgent();
		finalTarget = GameObject.Find("Treasure");
		minionMovingSpeed = navMeshAgent.speed;
	}

	// Use this for initialization
	void Start () 
	{
		state = MinionState.SelectTarget;
		blockItemList = new List<BlockItems>();
		SetAnimation(walkingAnimation);
		faceTargetOnce = true;
		isDoDamageExecuting = false;
		isMinionLooting = false;
	}

	// Update is called once per frame
	void Update () 
	{
		CheckStates();
	}
	#endregion

	#region General Functions
	private void SetNavMeshAgent()
	{
		navMeshAgent = this.GetComponent<NavMeshAgent>();
		navMeshPath = new NavMeshPath();
	}

	private void ActivateDebug(string comment)
	{
		if(activeDebug)
		{
			Debug.Log(comment);
		}
	}

	private void SetAnimation(string animationName)
	{
		anim = this.GetComponent<Animation>();
		timeLengthAnimation = anim.GetClip(animationName).length;
		if (animationName == reachBlockItemAnimation)
		{
			anim[animationName].speed = (timeLengthAnimation / attackSpeed);
			this.GetComponent<Animation>().CrossFade(animationName);
		}
		else if (animationName == walkingAnimation)
		{
			anim[animationName].speed = (timeLengthAnimation / minionMovingSpeed);
			this.GetComponent<Animation>().CrossFade(animationName);
		}
		else if (animationName == reachDestinationAnimation)
		{
			this.GetComponent<Animation>().CrossFade(animationName);
		}
		else if (animationName == reachRTSUnitAnimation)
		{
			anim[animationName].speed = (timeLengthAnimation / attackSpeed);
			this.GetComponent<Animation>().CrossFade(animationName);
		}
	}

	private void FaceTarget()
	{
		if(faceTargetOnce)
		{	if(destinationTarget.tag == "BlockItems")
			{
				this.transform.LookAt(destinationTarget.transform.parent.parent);
			}
		else if(destinationTarget.tag == "RTSUnit")
		{
			this.transform.LookAt(destinationTarget.transform.GetChild(0));
		}
		faceTargetOnce = false;
	}
	}

	private IEnumerator DoDamage(float time)
	{
		if(isDoDamageExecuting)
		{
			yield break;
		}

		isDoDamageExecuting = true;

		yield return new WaitForSeconds(time/2);
		this.GetComponent<MinionDoDamage>().DoDamage();
		yield return new WaitForSeconds(time/2);

		isDoDamageExecuting = false;
	}
	#endregion

	#region Structure Functions
	private void CheckStates()
	{
		if (state == MinionState.SelectTarget)
		{
			SelectTarget();
			ActivateDebug("SelectTarget");
		}
		else if (state == MinionState.CheckTarget)
		{
			CheckTarget();
			ActivateDebug("CheckTarget");
		}
		else if (state == MinionState.MoveToTarget)
		{
			MoveToTarget();
			ActivateDebug("MoveToTarget");
		}
		else if (state == MinionState.ReachTarget)
		{
			ReachTarget();
			ActivateDebug("Reached Target");
		}
	}

	private void FindBlockItem()
	{
		CreateListOfBlockItems();

		for(int i = 0; i < blockItemList.Count; i++)
		{
			blockItemTarget = blockItemList[i].blockItems.transform.Find("BlockPoints").gameObject.transform.Find("FrontPoint").gameObject;
			if (CheckIfItemIsReachable(blockItemTarget) == true)
			{
				destinationTarget = blockItemTarget;
				return;
			}
			else
			{
				blockItemTarget = blockItemList[i].blockItems.transform.Find("BlockPoints").gameObject.transform.Find("BackPoint").gameObject;
				if (CheckIfItemIsReachable(blockItemTarget) == true)
				{
					destinationTarget = blockItemTarget;
					return;
				}
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
			blockItemsClass.blockItemsDistance = Vector3.Distance(destinationTarget.transform.position, blockItems[i].transform.position);
			blockItemList.Add(blockItemsClass);
		}
		blockItemList.Sort(SortBlockItems);
		blockItemList.Reverse();
	}

	static int SortBlockItems(BlockItems tr1, BlockItems tr2)
	{
		return tr2.blockItemsDistance.CompareTo(tr1.blockItemsDistance);
	}

	private bool CheckIfItemIsReachable(GameObject ItemDestinationTarget)
	{
		navMeshAgent.CalculatePath(ItemDestinationTarget.transform.position, navMeshPath);
		if(navMeshPath.status == NavMeshPathStatus.PathComplete)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool CheckIfSelectedRoomIsTheSame()
	{
		if(roomTarget == saveRoomTarget)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool CheckIfTargetIsReached()
	{
		reachDistance = Vector3.Distance(this.transform.position, new Vector3(destinationTarget.transform.position.x, 0, destinationTarget.transform.position.z));

		if(reachDistance <= navMeshAgent.stoppingDistance + minionReach)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void CheckIfRoomTargetIsLocked()
	{
		if (lockedRoomTarget && (lockedRoomTarget.GetComponent<RoomEntityIdentifier>().isDestroyed == false))
		{
			if (lockedRoomTarget == roomTarget)
			{
				lockedRoomDestination = lockedRoomTarget;
				roomTargetIsLocked = true;
			}
		}
		else
		{
			roomTargetIsLocked = false;
		}
	}
	#endregion

	#region SelectTarget Functions
	private void SelectTarget()
	{
		if((unitBlockItemTarget == null) || (unitBlockItemTarget.transform.parent.parent.gameObject.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitIsAlive == false))
		{
			if ((unitTarget == null) || (unitTarget.GetComponent<UnitAttributes>().unitAttributes.unitIsAlive == false))
			{
				ActivateDebug("No Unit");
				if ((blockItemTarget == null) || (blockItemTarget.transform.parent.parent.gameObject.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitIsAlive == false))
				{	
					ActivateDebug("No BlockItem");

					if (roomTarget == null)
					{
						ActivateDebug("No Room");
						SelectFinalTarget();
					}
					else
					{
						ActivateDebug("Room Found");
						SelectRoomTarget();
					}
				}
				else
				{
					ActivateDebug("BlockItem Found");
					SelectBlockItemTarget();
				}
			}
			else
			{
				ActivateDebug("Unit Found");
				SelectUnitTarget();
			}
		}
		else
		{
			ActivateDebug("UnitBlockItem Found");
			SelectUnitBlockItemTarget();
		}
	}

	private void SelectFinalTarget()
	{
		destinationTarget = finalTarget;
		state = MinionState.CheckTarget;
	}

	private void SelectRoomTarget()
	{
		CheckIfRoomTargetIsLocked();
		if(roomTargetIsLocked == false)
		{
			if(CheckIfSelectedRoomIsTheSame())
			{
				destinationTarget = roomTarget;
				state = MinionState.CheckTarget;
				ActivateDebug("Destination = Room Target");
			}
			else
			{
				saveRoomTarget = roomTarget;
				blockItemTarget = null;
				state = MinionState.SelectTarget;
				ActivateDebug("RoomTarget Changed");
			}
		}
		else
		{
			destinationTarget = lockedRoomDestination;
			state = MinionState.CheckTarget;
			ActivateDebug("Destination = Locked Room Target");
		}
	}

	private void SelectBlockItemTarget()
	{
		CheckIfRoomTargetIsLocked();
		if (roomTargetIsLocked == false)
		{
			if(CheckIfSelectedRoomIsTheSame())
			{
				if (!CheckIfItemIsReachable(roomTarget))
				{
					state = MinionState.CheckTarget;
				}
				else
				{
					destinationTarget = roomTarget;
					state = MinionState.CheckTarget;
				}
			}
			else
			{
				destinationTarget = roomTarget;
				saveRoomTarget = destinationTarget;
				blockItemTarget = null;
				state = MinionState.SelectTarget;
				ActivateDebug("RoomTarget Changed");
			}
		}
		else
		{
			if (!CheckIfItemIsReachable(lockedRoomDestination))
			{
				state = MinionState.CheckTarget;
			}
			else
			{
				destinationTarget = lockedRoomDestination;
				state = MinionState.CheckTarget;
			}
		}
	}

	private void SelectUnitTarget()
	{
		destinationTarget = unitTarget;
		state = MinionState.CheckTarget;
	}

	private void SelectUnitBlockItemTarget()
	{
		destinationTarget = unitBlockItemTarget;
		state = MinionState.CheckTarget;
	}
	#endregion

	#region CheckTarget Functions
	private void CheckTarget()
	{
		if(destinationTarget != null)
		{
			if (destinationTarget.CompareTag("FinalTreasure"))
			{
				ActivateDebug("Check Final Target");
				CheckFinalTarget();
			}
			else if (destinationTarget.CompareTag("RoomArea"))
			{
				ActivateDebug("Check Room Target");
				CheckRoomTarget();
			}
			else if (destinationTarget.CompareTag("BlockItemPoints"))
			{
				ActivateDebug("Check Block Item");
				CheckBlockItemTarget();
			}
			else if (destinationTarget.CompareTag("RTSUnit"))
			{
				ActivateDebug("Check RTSUnit");
				CheckRTSUnit();
			}
		}
		else
		{
			state = MinionState.SelectTarget;
		}
	}

	private void CheckFinalTarget()
	{
		if (CheckIfItemIsReachable(destinationTarget))
		{
			state = MinionState.MoveToTarget;
		}
		else
		{
			FindBlockItem();
			state = MinionState.SelectTarget;
		}
	}

	private void CheckRoomTarget()
	{
		if (CheckIfItemIsReachable(destinationTarget))
		{
			ActivateDebug(destinationTarget + "is Reachable");
			state = MinionState.MoveToTarget;
			ActivateDebug("Move to the Room");
		}
		else
		{
			FindBlockItem();
			ActivateDebug("Found Block to the Room");
			state = MinionState.SelectTarget;
		}
	}

	private void CheckBlockItemTarget()
	{
		if (blockItemTarget)
		{
			if (CheckIfItemIsReachable(destinationTarget))
			{
				ActivateDebug(destinationTarget + "is Reachable");
				state = MinionState.MoveToTarget;
				ActivateDebug("Move to the BlockItem");
			}
			else
			{
				FindBlockItem();
				ActivateDebug("Found Block of the Block Item");
				state = MinionState.SelectTarget;
			}
		}
		else
		{
			destinationTarget = roomTarget;
			blockItemTarget = null;
			state = MinionState.SelectTarget;
			ActivateDebug("BlockItem Destroyed");
		}
	}

	private void CheckRTSUnit()
	{
		if (unitTarget)
		{
			if (CheckIfItemIsReachable(destinationTarget))
			{
				ActivateDebug(destinationTarget + "is Reachable");
				state = MinionState.MoveToTarget;
				ActivateDebug("Move to the UnitRTS");
			}
			else
			{
				FindBlockItem();
				unitBlockItemTarget = blockItemTarget;	//testing
				ActivateDebug("Found Block of the RTSUnit");
				state = MinionState.SelectTarget;
			}	
		}
		else
		{
			destinationTarget = roomTarget;
			state = MinionState.SelectTarget;
			ActivateDebug("RTSUnit Destroyed");
		}
	}
	#endregion

	#region MoveToTarget Functions
	private void MoveToTarget()
	{
		if (destinationTarget != null)
		{
			if (!CheckIfTargetIsReached())
			{
				navMeshAgent.isStopped = false;	//restart moving
				navMeshAgent.SetDestination(destinationTarget.transform.position);
				SetAnimation(walkingAnimation);
				state = MinionState.ReachTarget;
				ActivateDebug("Move To Destination");
			}
			else
			{
				navMeshAgent.isStopped = true;
				state = MinionState.ReachTarget;
			}
		}
		else
		{
			state = MinionState.SelectTarget;
		}
	}
	#endregion

	#region ReachTarget Functions
	private void ReachTarget()
	{
		if (destinationTarget != null)
		{
			if (destinationTarget.CompareTag("FinalTreasure"))
			{
				ActivateDebug("Reach Final Target");
				ReachFinalTarget();
			}
			else if (destinationTarget.CompareTag("RoomArea"))
			{
				ActivateDebug("Reach Room Target");
				ReachRoomTarget();
			}
			else if (destinationTarget.CompareTag("BlockItemPoints"))
			{
				ActivateDebug("Reach Block Item");
				ReachBlockItemTarget();
			}
			else if (destinationTarget.CompareTag("RTSUnit"))
			{
				ActivateDebug("Reach RTSUnit");
				ReachRTSUnit();
			}
		}
		else
		{
			state = MinionState.SelectTarget;
		}
	}

	private void ReachFinalTarget()
	{
		if (CheckIfTargetIsReached())
		{
			FaceTarget();
			ActivateDebug("Finaly Reached Final Target");
			SetAnimation(reachDestinationAnimation);
			Destroy(this.gameObject, 3.0f);	//Kaput
		}
		else
		{
			faceTargetOnce = true;
			state = MinionState.SelectTarget;
		}
	}

	private void ReachRoomTarget()
	{
		if (CheckIfTargetIsReached())
		{
			ActivateDebug("Finaly Reached Room Target");
			FaceTarget();
			SetAnimation(reachDestinationAnimation);
			if (!isMinionLooting)
			{
				destinationTarget.GetComponent<RoomEntityIdentifier>().roomTreasureScore -= minionTreasureCurry;
			}
			Destroy(this.gameObject, timeLengthAnimation);
			isMinionLooting = true;
		}
		else
		{
			faceTargetOnce = true;
			state = MinionState.SelectTarget;
		}
	}

	private void ReachBlockItemTarget()
	{
		if (CheckIfTargetIsReached())
		{
			ActivateDebug("Finaly Reached Block Item Target");
			FaceTarget();
			SetAnimation(reachBlockItemAnimation);
			StartCoroutine(DoDamage(attackSpeed));
			state = MinionState.SelectTarget;
		}
		else
		{
			faceTargetOnce = true;
			state = MinionState.SelectTarget;
		}
	}

	private void ReachRTSUnit()
	{
		if (CheckIfTargetIsReached())
		{
			ActivateDebug("Finaly Reached RTSUnit");
			FaceTarget();
			SetAnimation(reachRTSUnitAnimation);
			StartCoroutine(DoDamage(attackSpeed));
			state = MinionState.SelectTarget;
		}
		else
		{
			faceTargetOnce = true;
			state = MinionState.SelectTarget;
		}
	}
	#endregion
}