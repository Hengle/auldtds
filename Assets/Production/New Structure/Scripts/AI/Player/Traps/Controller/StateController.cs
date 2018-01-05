using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trap
{
	public class StateController : MonoBehaviour 
	{
		#region Variables
		[Header("Main")]
		public TrapState currentState;
		public TrapState remainState;
		public TrapStats trapStats;
		public Transform eyes;

		[HideInInspector]public Animator anim; 
		[HideInInspector]public float stateTimeElapsed;
		[HideInInspector]public bool isTrapTriggered;

		private List<GameObject> minionsList;
		#endregion
		#region System Functions

		void Awake()
		{
			anim = GetComponent<Animator>();
		}

		void Start()
		{
			isTrapTriggered = false;
		}

		void Update()
		{

		}

		void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Minion" && !isTrapTriggered)
			{
				//Coroutine
			}

			if(other.tag == "Minion")
			{
				if(!minionsList.Contains(other.gameObject))
				{
					minionsList.Add(other.gameObject);
				}
			}
		}

		void OnTriggerExit(Collider other)
		{
			if(other.tag == "Minion")
			{
				minionsList.Remove(other.gameObject);
			}
		}

		void OnDrawGizmos()
		{
			if(currentState != null)
			{
				Gizmos.color = currentState.sceneGizmoColor;
				Gizmos.DrawWireSphere(eyes.position, trapStats.lookSphereCastRadius);
			}
		}

		public void TransitionToState(TrapState nextState)
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
		#region AttackAction Functions
		/*
		private IEnumerator ActivateTrap(float time)
		{
			if (time < timeLengthAnimation)
			{
				time = timeLengthAnimation;
			}

			isTrapTriggered = true;
			yield return new WaitForSeconds(time);
			anim.SetTrigger("TrapTrigger");
			TrapDamage();
			this.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
			if (trapCD >0)
			{
				Invoke("ResetTrap", trapCD);
			}
		}

		private void TrapDamage()
		{
			foreach (GameObject minion in minionsList)
			{
				minion.GetComponent<MinionDamages>().TakeDamage(trapDamage, true);
			}
		}

		public void DoBasicDamage()
		{
			foreach (GameObject minion in minionsList)
			{
				int damageMin = playerUnitStats.minDamage;
				int damageMax = playerUnitStats.maxDamage;
				int toHit = playerUnitStats.toHitScore;
				int critScore = playerUnitStats.critScore;
				int critMultiplier = playerUnitStats.critMultiplier;
				int actualCritMultiplier;
				bool critText;

				int toHitRoll = (Random.Range(1, 21) + toHit);

				if (enemyTarget.GetComponent<Enemy.StateController>().enemyStats.currentHealth > 0)
				{
					int totalAC = enemyTarget.GetComponent<Enemy.StateController>().enemyStats.baseArmor + enemyTarget.GetComponent<Enemy.StateController>().enemyStats.unitArmor;

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
*/
		#endregion
	}
}
