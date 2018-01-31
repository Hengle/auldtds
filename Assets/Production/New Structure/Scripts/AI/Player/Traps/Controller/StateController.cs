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
		public bool isTrapTriggered;

        [SerializeField]
		private List<GameObject> minionsList;
		#endregion
		#region System Functions

		void Awake()
		{
			anim = GetComponent<Animator>();
            minionsList = new List<GameObject>();
		}

		void Start()
		{
			isTrapTriggered = false;
		}

		void Update()
		{
            currentState.UpdateState(this);
        }

		void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Minion" && !isTrapTriggered)
			{
                //Coroutine
                //TrapTrigger();
                isTrapTriggered = true;
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
		public void TrapTrigger()
        {
            anim.SetTrigger("TrapTrigger");
            isTrapTriggered = true;
        }

      
        public void DoBasicDamage()
        {
            if (minionsList!= null)
            {
                int damageMin = trapStats.minDamage;
                int damageMax = trapStats.maxDamage;

                //int toHitRoll = (Random.Range(1, 21) + toHit);
                foreach (GameObject enemyTarget in minionsList)
                {
                    if (enemyTarget.GetComponent<Enemy.StateController>().enemyStats.currentHealth > 0)
                    {
                       int damageRoll = (Random.Range(damageMin, damageMax + 1));
                       enemyTarget.GetComponent<Enemy.StateController>().TakeDamage(damageRoll, true);
                    }
                }
            }
        }

        #endregion
    }
}
