using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace PlayerUnit
{
	public class StateController : MonoBehaviour 
	{
		#region Variables
		[Header("Main")]
		public PlayerUnitState currentState;
		public PlayerUnitState remainState;
		public PlayerUnitStats playerUnitStats;
		public Transform eyes;
		public GameObject radarForEnemies;

		[Header("Text")]
		public float textYOffset = 0;

		private bool dieOnce;
		[SerializeField]
		private List<GameObject> engageEnemiesList;
		private float nextAttack;

		[HideInInspector]public NavMeshAgent navMeshAgent;
		//[HideInInspector]
		public Transform chaseTarget;
		[HideInInspector]public Transform enemyTarget;
		[HideInInspector]public Transform savedChaseTarget;
		[HideInInspector]public bool isChaseTargetReachable;
		[HideInInspector]public Animator anim;
		[HideInInspector]public bool fullEngaged;
		#endregion
		#region System Functions

		[HideInInspector]public float stateTimeElapsed;

		void OnEnable()
		{
			playerUnitStats = Object.Instantiate(playerUnitStats);
		}

		void Awake()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			anim = GetComponent<Animator>();
			radarForEnemies.GetComponent<SphereCollider>().radius = playerUnitStats.eyeSight;
		}

		void Start()
		{
			dieOnce = true;
			engageEnemiesList = new List<GameObject>();
			fullEngaged = false;
		}

		void Update () 
		{
			currentState.UpdateState(this);
	
			UnitDeath();
		}

		void OnDrawGizmos()
		{
			if(currentState != null)
			{
				Gizmos.color = currentState.sceneGizmoColor;
				Gizmos.DrawWireSphere(eyes.position, playerUnitStats.lookSphereCastRadius);
			}
		}

		public void TransitionToState(PlayerUnitState nextState)
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
		private void UnitDeath()
		{
			if (playerUnitStats.currentHealth <=0 && playerUnitStats.isAlive == true)
			{
				if (dieOnce == true)
				{
					playerUnitStats.isAlive = false;
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

				if (range <= playerUnitStats.attackRange)
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
		#region Take Damage Functions
		public void TakeDamage(int damage, bool critical)
		{
			CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + damage.ToString(), Color.red, critical);
			playerUnitStats.currentHealth -= damage;
		}

		public void MissDamage(string miss, bool critical)
		{
			CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, miss, Color.red, critical);
		}
		#endregion
		#region AttackAction Functions
		private void BasicAttack()
		{
			if(Time.time > nextAttack)
			{
				nextAttack = Time.time + playerUnitStats.attackSpeed;
				anim.SetTrigger("BasicAttackTrigger");
			}
		}

		public void ExecuteAttack()
		{
			if(chaseTarget != null && chaseTarget.GetComponent<Enemy.StateController>().enemyStats.isAlive)
			{
				BasicAttack();
			}
		}

		public void DoBasicDamage()
		{
			if(chaseTarget)
			{
				int damageMin = playerUnitStats.minDamage;
				int damageMax = playerUnitStats.maxDamage;
				int toHit = playerUnitStats.toHitScore;
				int critScore = playerUnitStats.critScore;
				int critMultiplier = playerUnitStats.critMultiplier;
				int actualCritMultiplier;
				bool critText;

				int roll = Random.Range(1, 21);
				int toHitRoll = (roll + toHit);

				if (enemyTarget.GetComponent<Enemy.StateController>().enemyStats.isAlive)
				{
					int totalAC = enemyTarget.GetComponent<Enemy.StateController>().enemyStats.baseArmor + enemyTarget.GetComponent<Enemy.StateController>().enemyStats.unitArmor;

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

						enemyTarget.GetComponent<Enemy.StateController>().TakeDamage(damageRoll, critText);
					}
					else
					{
						string damageRoll = "Miss";

						enemyTarget.GetComponent<Enemy.StateController>().MissDamage(damageRoll, true);
					}
				}
			}
		}

		public void FaceEnemy()
		{
			Vector3 relativePos = chaseTarget.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePos);
			transform.rotation = rotation;
		}
		#endregion
		#region Unit Engage Functions
		public void UpdateEngageList(GameObject newEnemy)
		{
			engageEnemiesList.RemoveAll(GameObject => GameObject == null);

			if(engageEnemiesList.Count < playerUnitStats.totalEngagedEnemies)
			{
				if(!engageEnemiesList.Any(i=>i == newEnemy))
				{
					engageEnemiesList.Add(newEnemy);	
				}
			}

			if(engageEnemiesList.Count < playerUnitStats.totalEngagedEnemies)
			{
				fullEngaged = false;
			}
			else
			{
				fullEngaged = true;
			}
		}
		#endregion
	}
}
