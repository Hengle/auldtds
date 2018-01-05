using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BlockItem
{
	public class StateController : MonoBehaviour 
	{
		#region Variables
		[Header("Main")]
		public BlockItemStats blockItemStats;
		[Header("Text")]
		public float textYOffset = 0;

		[SerializeField]
		private List<GameObject> engageEnemiesList;

		[HideInInspector]public GameObject savedPlacement;
		[HideInInspector]public List<Transform> reachPoints;
		[HideInInspector]public bool fullEngaged;
		#endregion
		#region System Functions
		void OnEnable()
		{
			blockItemStats = Object.Instantiate(blockItemStats);
		}

		// Use this for initialization
		void Start () 
		{
			reachPoints = new List<Transform>();
			engageEnemiesList = new List<GameObject>();
			AssignReachPoints();
		}
		
		// Update is called once per frame
		void Update () 
		{
			BlockItemDeath();
		}
		#endregion
		#region General Functions
		private void BlockItemDeath()
		{
			if (blockItemStats.currentHealth <=0 && blockItemStats.isAlive == true)
			{
				GetComponent<BlockItem.StateController>().blockItemStats.isAlive = false;
				Reactivate();
				Destroy(gameObject);
				Debug.Log("DEAD");
			}
		}
			
		private void Reactivate()
		{
			if(savedPlacement)
			{
				savedPlacement.SetActive(true);
				savedPlacement.GetComponent<MeshRenderer>().enabled = false;
			}
		}

		private void AssignReachPoints()
		{
			Transform ReachPoints = transform.Find("ReachPoints");
			foreach (Transform child in ReachPoints.transform)
			{
				if(child.CompareTag("BlockItemPoints"))
				{
					reachPoints.Add(child);
				}
			}
		}
		#endregion
		#region Take Damage Functions
		public void TakeDamage(int damage, bool critical)
		{
			CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, "-" + damage.ToString(), Color.red, critical);
			blockItemStats.currentHealth -= damage;
		}

		public void MissDamage(string miss, bool critical)
		{
			CombatTextManager.Instance.CreateText(this.transform.position, textYOffset, miss, Color.red, critical);
		}
		#endregion
		#region BlockItem Engage Functions
		public void UpdateEngageList(GameObject newEnemy)
		{
			engageEnemiesList.RemoveAll(GameObject => GameObject == null);

			if(engageEnemiesList.Count < blockItemStats.totalEngagedEnemies)
			{
				if(!engageEnemiesList.Any(i=>i == newEnemy))
				{
					engageEnemiesList.Add(newEnemy);	
				}
			}

			if(engageEnemiesList.Count < blockItemStats.totalEngagedEnemies)
			{
				fullEngaged = false;
			}
			else
			{
				fullEngaged = true;
			}
		}

		public bool CheckEngageList(GameObject newEnemy)
		{
			if(engageEnemiesList.Any(i=>i == newEnemy) || engageEnemiesList.Any(i=>i == null))
			{
				return true;
			}
			else
			{
				return false;	
			}
		}
		#endregion
	}
}
