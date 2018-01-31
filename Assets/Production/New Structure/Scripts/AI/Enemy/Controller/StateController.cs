using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using BlockItem;
//using PlayerUnit;

namespace Enemy
{
	public class StateController : MonoBehaviour 
	{
		#region Variables
		[Header("Main")]
		public EnemyState currentState;
		public EnemyState remainState;
		public EnemyStats enemyStats;
		public Transform eyes;
		public GameObject radarForEnemies;

		[Header("Text")]
		public float textYOffset = 0;

		private bool aiActive;
		private bool dieOnce;
		private bool isEnemyLooting;
		private float nextAttack;

		[HideInInspector]public NavMeshAgent navMeshAgent;
		//[HideInInspector]
		public Transform chaseTarget;
		[HideInInspector]public Transform savedChaseTarget;
		[HideInInspector]public float stateTimeElapsed;
		[HideInInspector]public Transform finalTarget;
		[HideInInspector]public Animator anim; 
		[HideInInspector]public Transform roomTarget;
		[HideInInspector]public Transform savedRoomTarget;
		[HideInInspector]public Transform lockedRoomTarget;
		[HideInInspector]public Transform unitTarget;
		[HideInInspector]public Transform savedUnitTarget;
		[HideInInspector]public Transform lockedUnitTarget;
		[HideInInspector]public bool isChaseTargetReachable;
		[HideInInspector]public Transform blockItemTarget;
		[HideInInspector]public Transform blockItemPointTarget;

		#endregion
		#region System Functions
		void OnEnable()
		{
			enemyStats = Object.Instantiate(enemyStats);
		}

		void Awake()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			anim = GetComponent<Animator>();
			radarForEnemies.GetComponent<SphereCollider>().radius = enemyStats.eyeSight;
			dieOnce = true;
			isEnemyLooting = false;
		}


		void Update()
		{
			currentState.UpdateState(this);
			EnemyDeath();
		}

		void OnDrawGizmos()
		{
			if(currentState != null)
			{
				Gizmos.color = currentState.sceneGizmoColor;
				Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
				/*
				if(chaseTarget)
				{
					Gizmos.DrawLine(this.gameObject.transform.position, chaseTarget.position);
				}*/
			}
		}
			
		public void TransitionToState(EnemyState nextState)
		{
			if (nextState != remainState)
			{
				currentState = nextState;
				OnExitState();
			}
		}

		private void OnExitState()
		{
			stateTimeElapsed = 0;
		}
		#endregion
		#region General Functions
		public void SetupAI(bool aiActivationFromManager, Transform finalTargetFromManager)
		{
			aiActive = aiActivationFromManager;
			finalTarget = finalTargetFromManager;
			if(aiActive)
			{
				navMeshAgent.enabled = true;
			}
			else
			{
				navMeshAgent.enabled = false;
			}
		}

		public void SetEyeSight(float eyeSight)
		{
			radarForEnemies.GetComponent<SphereCollider>().radius = eyeSight;
		}

		public void LootRoom()
		{
			if(!isEnemyLooting)
			{
				chaseTarget.GetComponent<RoomEntityIdentifier>().roomTreasureScore -= enemyStats.lootCapacity;
				anim.SetTrigger("PickUpTrigger");
				isEnemyLooting = true;
			}
		}

		private void EnemyDeath()
		{
			if (enemyStats.currentHealth <=0 && enemyStats.isAlive == true)
			{
				if (dieOnce == true)
				{
					enemyStats.isAlive = false;
					AwardMinionXP();
					AwardGold();
					AwardKill();
					Loot();
					dieOnce = false;
					navMeshAgent.speed = 0;
					anim.SetTrigger("DeathTrigger");
				}
			}
		}

		private void KillObject()
		{
			Destroy(this.gameObject);
		}
		#endregion
		#region Checks Functions
		public bool CheckIfCountDownElapsed(float duration)
		{
			stateTimeElapsed += Time.deltaTime;
			return(stateTimeElapsed >= duration);
		}

		public bool CheckPath(Transform target)
		{
			if (target)
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				navMeshAgent.CalculatePath(target.position, navMeshPath);
				if(navMeshPath.status == NavMeshPathStatus.PathComplete)
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

		public bool IsChaseTargetReached()
		{
			if(chaseTarget)
			{
				float range  = new Vector3(this.transform.position.x - chaseTarget.position.x, 0, this.transform.position.z - chaseTarget.position.z).magnitude;
		
				if (range <= enemyStats.attackRange)
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

		public bool IsFullEngagedBlockItemClose()
		{
			if(chaseTarget)
			{
				float range  = new Vector3(this.transform.position.x - chaseTarget.position.x, 0, this.transform.position.z - chaseTarget.position.z).magnitude;
				if(range <= enemyStats.closeToFullEngagedBlockItem)
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
		#endregion
		#region AttackAction Functions
		private void BasicAttack()
		{
			if(Time.time > nextAttack)
			{
				nextAttack = Time.time + enemyStats.attackSpeed;
				anim.SetTrigger("BasicAttackTrigger");
			}
		}

		public void ExecuteAttack()
		{
			if(chaseTarget != null && chaseTarget.GetComponent<PlayerUnit.StateController>().playerUnitStats.isAlive)
			{
				BasicAttack();
			}
		}

		public void DoBasicDamage()
		{
			if(chaseTarget)
			{
				int damageMin = enemyStats.minDamage;
				int damageMax = enemyStats.maxDamage;
				int toHit = enemyStats.toHitScore;
				int critScore = enemyStats.critScore;
				int critMultiplier = enemyStats.critMultiplier;
				int actualCritMultiplier;
				bool critText;

				int roll = Random.Range(1, 21);
				int toHitRoll = (roll + toHit);

				if (chaseTarget == blockItemPointTarget)
				{
					if (blockItemTarget.GetComponent<BlockItem.StateController>().blockItemStats.currentHealth > 0)
					{
						int totalAC = blockItemTarget.GetComponent<BlockItem.StateController>().blockItemStats.baseArmor + blockItemTarget.GetComponent<BlockItem.StateController>().blockItemStats.unitArmor;

						if ((toHitRoll >= totalAC))
						{
							if (roll >= critScore)
							{
								actualCritMultiplier = critMultiplier;
								critText = true;
							}
							else
							{
								actualCritMultiplier = 1;
								critText = false;
							}
					
							int damageRoll = (Random.Range(damageMin, damageMax + 1) * actualCritMultiplier);

							blockItemTarget.GetComponent<BlockItem.StateController>().TakeDamage(damageRoll, critText);
						}
						else
						{
							string damageRoll = "Miss";
		
							blockItemTarget.GetComponent<BlockItem.StateController>().MissDamage(damageRoll, true);
						}
					}
				}
				else if (chaseTarget == lockedUnitTarget)
				{
					if (lockedUnitTarget.GetComponent<PlayerUnit.StateController>().playerUnitStats.currentHealth > 0)
					{
						int totalAC = lockedUnitTarget.GetComponent<PlayerUnit.StateController>().playerUnitStats.baseArmor + lockedUnitTarget.GetComponent<PlayerUnit.StateController>().playerUnitStats.unitArmor;

						if ((toHitRoll >= totalAC))
						{
							if (toHit >= critScore)
							{
								actualCritMultiplier = critMultiplier;
								critText = true;
							}
							else
							{
								actualCritMultiplier = 1;
								critText = false;
							}

							int damageRoll = (Random.Range(damageMin, damageMax + 1) * actualCritMultiplier);

							lockedUnitTarget.GetComponent<PlayerUnit.StateController>().TakeDamage(damageRoll, critText);
						}
						else
						{
							string damageRoll = "Miss";

							lockedUnitTarget.GetComponent<PlayerUnit.StateController>().MissDamage(damageRoll, true);
						}
					}
				}
			}
		}

		public void FaceEnemy()
		{
			Vector3 relativePos = new Vector3(chaseTarget.position.x - this.transform.position.x, 0, chaseTarget.position.z - this.transform.position.z);
			Quaternion rotation = Quaternion.LookRotation(relativePos);
			transform.rotation = rotation;
		}
		#endregion
		#region Take Damage Functions
		public void TakeDamage(int damage, bool critical)
		{
			CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + damage.ToString(), Color.red, critical);
			enemyStats.currentHealth -= damage;
		}

		public void MissDamage(string miss, bool critical)
		{
			CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, miss, Color.red, critical);
		}
		#endregion
		#region Award Functions
		private void AwardMinionXP()
		{
			GameObject xpManagerObject = GameObject.Find("XPBarManager");
			XPMan xpMan = xpManagerObject.GetComponent<XPMan>();
			xpMan.AwardXP(this.enemyStats.AwardExp);
		}

		private void AwardGold()
		{
			GameMainManager.Instance._treasureGold += this.enemyStats.AwardGold;
		}

		private void AwardKill()
		{
			GameMainManager.Instance._minionsKilled += 1;
            if (!GameMainManager.Instance.minionKillTags.Contains(this.enemyStats.name))
            {
                GameMainManager.Instance.minionKillTags.Add(this.enemyStats.name);
                int listIndex = GameMainManager.Instance.minionKillTags.IndexOf(this.enemyStats.name);
                GameMainManager.Instance.minionKillCounter.Add(1);

            }
            else
            {
                GameMainManager.Instance.minionKillTags.IndexOf(this.enemyStats.name);
                int listIndex = GameMainManager.Instance.minionKillTags.IndexOf(this.enemyStats.name);
                GameMainManager.Instance.minionKillCounter[listIndex] = GameMainManager.Instance.minionKillCounter[listIndex] +1;
            }
            
		}

		private void Loot()
		{
			GameObject lootManager = GameObject.Find("LootManager");
			LootTableClass lootTable = lootManager.GetComponent<LootTableClass>();
			lootTable.CalculateLoot(this.transform.position);
		}
		#endregion
	}
}