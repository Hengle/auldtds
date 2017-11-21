﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class MinionAI : MonoBehaviour
{

	#region Classes
	[System.Serializable]
	public class BlockItems
	{
		public GameObject blockItems;
		public float blockItemsDistance;
	}
	#endregion

	#region Properties
	public enum MinionState {SelectTarget, CheckTarget, Action}
	public enum ActionState {Move, Idle, AttackTarget}

	[Header("State")]
	[SerializeField]
	private MinionState state;
	[SerializeField]
	private ActionState actionState;
	private ActionState saveActionState;

	[Header("Targets")]
	public GameObject destinationTarget;
	private GameObject saveDestinationTarget;
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
    
    public bool lockedOnUnit;

	[Header("Radars")]
	public GameObject roomTarget;
	public GameObject lockedRoomTarget;
	[SerializeField]
	private GameObject lockedRoomDestination;
	private bool roomTargetIsLocked;

	[Header("NavMesh")]
	private NavMeshAgent navMeshAgent;
	private NavMeshPath navMeshPath;
	private float minionVelocity;

	[Header("MinionAttributes")]
	private UnitAttributes unitAttributes;

	[Header("BlockItemList")]
	[SerializeField]
	private List<BlockItems> blockItemList;
	private BlockItems blockItemsClass;
	private GameObject[] blockItems;
	[SerializeField]
	public bool closeToBlockItem;

	[Header("Debug")]
	[SerializeField]
	private bool activeDebug;

	[Header("Animations")]
	private bool dieOnce;
    private Animator animator;
    private MinionDoDamage minionAttack;

	[Header("Positioning")]
	[SerializeField]
	private float minionReach;
	[SerializeField]
	private int enemyEngagePoints;

	[Header("Do Damage")]
	[SerializeField]
	private float attackSpeed;
	private bool isDoDamageExecuting;

	[Header("Loot Room")]
	[SerializeField]
	private int minionTreasureCurry;
	private bool isMinionLooting;

    [Header("Animation Flags")]
    [SerializeField]
    private bool attacking = false;
	[SerializeField]
	private bool removeMinion = false;
	private bool reachedRoomOrTreasure = false;
 
	[Header("EngageFlags")]
	public bool engaged = false;
	[SerializeField]
	private GameObject currentEngagedTarget;
	#endregion
    #region System Functions
    void Awake()
	{
        animator = gameObject.GetComponent<Animator>();
        minionAttack = this.GetComponent<MinionDoDamage>();
		SetNavMeshAgent();
		SetMinionAttributes();
		finalTarget = GameObject.Find("Treasure");
	}

	// Use this for initialization
	void Start () 
	{
		state = MinionState.SelectTarget;
		blockItemList = new List<BlockItems>();
		isDoDamageExecuting = false;
		isMinionLooting = false;
		dieOnce = true;
		saveDestinationTarget = null;
        lockedOnUnit = false;
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
		
	private void SetMinionAttributes()
	{
		unitAttributes = this.GetComponent<UnitAttributes>();
	}

	private void ActivateDebug(string comment)
	{
		if(activeDebug)
		{
			Debug.Log(comment);
		}
	}
    
	private void FaceTarget()
	{
		if(destinationTarget.CompareTag("RTSUnit"))
		{
		Vector3 relativePos = destinationTarget.transform.position - this.transform.position;
		Quaternion minionRotation = Quaternion.LookRotation(relativePos);
		this.transform.rotation = minionRotation;
		}
	} 

	private void KillMinion()
	{
		Destroy(this.gameObject);
	}

	private bool CheckIfMinionIsAlive()
	{
		if (unitAttributes.unitBaseAttributes.unitHealthPoints >0 && unitAttributes.unitBaseAttributes.unitIsAlive == true)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void MinionDeath()
	{
		if (dieOnce == true)
		{
            unitAttributes.unitBaseAttributes.unitIsAlive = false;
            AwardMinionXP();
            AwardGold();
            AwardKill();
            GameObject lootManager = GameObject.Find("LootManager");
            LootTableClass lootTable = lootManager.GetComponent<LootTableClass>();
            lootTable.CalculateLoot(this.transform.position);
            dieOnce = false;
			navMeshAgent.enabled = false;
			SetDeathAnimation();
        }
	}

    public void AwardMinionXP()
    {
        GameObject xpManagerObject = GameObject.Find("XPBarManager");
        XPMan xpMan = xpManagerObject.GetComponent<XPMan>();
        xpMan.AwardXP(this.GetComponent<UnitAttributes>().unitBaseAttributes.unitEXPValue);
    }

    private void AwardGold()
    {
        GameMainManager.Instance._treasureGold += this.GetComponent<UnitAttributes>().unitBaseAttributes.unitTreasureFactor;
    }

    private void AwardKill()
    {
        GameMainManager.Instance._minionsKilled += 1;
    }

    private void DestroyOnDeath()
    {
        Destroy(gameObject);
    }

	private void CheckIfSpaceToEngage()
	{
		if(destinationTarget.tag == "RTSUnit")
		{
			enemyEngagePoints = destinationTarget.GetComponent<UnitAttributes>().unitBaseAttributes.unitEnemyEngagePoints;
		}
	}

	private void SetEngageList()
	{
		if(destinationTarget.CompareTag("RTSUnit"))
		{
			if(!engaged)
			{
				destinationTarget.GetComponent<ObjectEngage>().FullEngageTable(this.gameObject);
				engaged = true;
				currentEngagedTarget = destinationTarget;
			}
		}
		else if (destinationTarget.transform.parent.parent.gameObject.CompareTag("BlockItems"))
		{
			if(!engaged)
			{
				if(!destinationTarget.transform.parent.parent.gameObject.GetComponent<ObjectEngage>().engageEnemiesList.Any(i=>i == this.gameObject))
				{
					destinationTarget.transform.parent.parent.gameObject.GetComponent<ObjectEngage>().FullEngageTable(this.gameObject);
					engaged = true;
					currentEngagedTarget = destinationTarget;
				}
			}
		}
	}

	private void ClearEngage()
	{
		engaged = false;
	}
    #endregion

    #region Animation Functions
	private void SetMoveAnimation()
	{
		minionVelocity = navMeshAgent.velocity.magnitude;
		animator.SetFloat("MinionVelocity", minionVelocity);
	}

	private void SetAttackAnimation()
	{
		animator.SetTrigger("SwordShieldAttack1Trigger");
	}
	    
	private void SetDeathAnimation()
	{
		animator.SetTrigger("DeathTrigger");
	}

	private void SetLootAnimation()
	{
		animator.SetTrigger("PickUpTrigger");
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
		else if (state == MinionState.Action)
		{
			ActionDecision();
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
		if(destinationTarget != null)
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
		else
		{
			return false;
		}
	}

	private void CheckIfRoomTargetIsLocked()
	{
		if ((lockedRoomTarget != null) && (lockedRoomTarget.GetComponent<RoomEntityIdentifier>().isDestroyed == false))
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

    private void UnlockUnitTarget()
    {
        lockedOnUnit = false;
    }
	#endregion

	#region SelectTarget Functions
	private void SelectTarget()
	{
        if (CheckIfMinionIsAlive())
        {
            if ((unitBlockItemTarget == null) || (unitBlockItemTarget.transform.parent.parent.gameObject.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitIsAlive == false))
            {
				closeToBlockItem = false;
                if ((unitTarget == null) || (unitTarget.GetComponent<UnitAttributes>().unitBaseAttributes.unitIsAlive == false))
                {
                    UnlockUnitTarget();
					ClearEngage();
                    
                    ActivateDebug("No Unit");
                    if ((blockItemTarget == null) || (blockItemTarget.transform.parent.parent.gameObject.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitIsAlive == false))
                    {
						closeToBlockItem = false;
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
					closeToBlockItem = false;
                }
            }
            else
            {
                ActivateDebug("UnitBlockItem Found");
                SelectUnitBlockItemTarget();
            }
        }
        else
        {
            MinionDeath();
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
			if (roomTarget != null)
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
				if(!CheckIfItemIsReachable(finalTarget))
				{
					state = MinionState.CheckTarget;
				}
				else
				{
					destinationTarget = finalTarget;
					state = MinionState.CheckTarget;
				}
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
			state = MinionState.Action;
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
			state = MinionState.Action;
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
		if ((blockItemTarget) && (blockItemTarget.transform.parent.parent.gameObject.GetComponent<BlockItemsAttributes>().blockItemsAttributes.unitIsAlive == true))
		{
			if(!blockItemTarget.transform.parent.parent.gameObject.GetComponent<ObjectEngage>().fullEngaged)
			{
				if (CheckIfItemIsReachable(destinationTarget))
				{
					ActivateDebug(destinationTarget + "is Reachable");
					state = MinionState.Action;
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
				state = MinionState.Action;
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
		if ((unitTarget) && (unitTarget.GetComponent<UnitAttributes>().unitBaseAttributes.unitIsAlive == true))
		{

            if (CheckIfItemIsReachable(destinationTarget))
			{
				ActivateDebug(destinationTarget + "is Reachable");
				state = MinionState.Action;
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

	#region Action Functions
	private void ActionDecision()
	{
		if(destinationTarget != null)
		{
			if(CheckIfTargetIsReached())
			{
				navMeshAgent.speed = 0.0f;

				switch(destinationTarget.tag)
				{
				case "FinalTreasure":
					ActionReachFinalTarget();
					break;
				case "RoomArea":
					ActionReachRoomTarget();
					break;
				case "BlockItemPoints":
					ActionAttackBlockItem();
					break;
				case "RTSUnit":
					ActionAttackRTSUnit();
					break;
				}
			}
			else 
			{
				KeepMoving();
			}
			ActionMoveToTarget();
			SetMoveAnimation();
		}
		else
		{
			state = MinionState.SelectTarget;
		}
	}

	private void ActionMoveToTarget()
	{
		navMeshAgent.SetDestination(destinationTarget.transform.position);
	}

	private void ActionAttackRTSUnit()
	{
		lockedOnUnit = true;
		FaceTarget();
		SetEngageList();
		if(!attacking)
		{
			InvokeRepeating("SetAttackAnimation", 0, unitAttributes.unitBaseAttributes.unitCDScore);	
			attacking = true;
		}
		state = MinionState.SelectTarget;
	}

	private void ActionAttackBlockItem()
	{
		FaceTarget();
		SetEngageList();
		if(!attacking)
		{
			InvokeRepeating("SetAttackAnimation", 0, unitAttributes.unitBaseAttributes.unitCDScore);	
			attacking = true;
		}
		state = MinionState.SelectTarget;
	}

	private void ActionIdleFullEngagedClose()
	{
		if((destinationTarget.CompareTag("BlockItemPoints")) && (destinationTarget.transform.parent.parent.gameObject.GetComponent<ObjectEngage>().fullEngaged) && (closeToBlockItem))
		{
			navMeshAgent.speed = 0.0f;
		}
	}

	private void KeepMoving()
	{
		navMeshAgent.speed = this.GetComponent<UnitAttributes>().unitBaseAttributes.unitSpeed;
		CancelInvoke("SetAttackAnimation");
		attacking = false;
		ActionIdleFullEngagedClose();
		state = MinionState.SelectTarget;
	}

	private void ActionReachRoomTarget()
	{
		navMeshAgent.speed = 0f;
		destinationTarget.GetComponent<RoomEntityIdentifier>().roomTreasureScore -= minionTreasureCurry;
		if(!isMinionLooting)
		{
			SetLootAnimation();
			isMinionLooting = true;
		}
	}

	private void ActionReachFinalTarget()
	{
		//GameOver?

	}
	#endregion
}