using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockItemClass
{
	public GameObject blockItem;
	public float blockItemDistance;
}

[CreateAssetMenu (menuName = "AI/Actions/Enemy/FindBlockItemTarget")]
public class FindBlockItemAction : EnemyAction 
{
	private List<BlockItemClass> blockItemsList;
	private BlockItemClass blockItemClass;
	private GameObject[] blockItems;

	public override void Act(Enemy.StateController controller)
	{
		FindBlockItem(controller);
	}

	private void CreateListOfBlockItems(Enemy.StateController controller)
	{
		blockItemsList.Clear();
		blockItems = GameObject.FindGameObjectsWithTag("BlockItems");

		for(int i = 0; i < blockItems.Length; i++)
		{
			blockItemClass = new BlockItemClass();
			blockItemClass.blockItem = blockItems[i];
			blockItemClass.blockItemDistance = Vector3.Distance(controller.chaseTarget.position, blockItems[i].transform.position);
			blockItemsList.Add(blockItemClass);
		}
		blockItemsList.Sort(SortBlockItems);
		blockItemsList.Reverse();
	}
		
	private static int SortBlockItems(BlockItemClass tr1, BlockItemClass tr2)
	{
		return tr2.blockItemDistance.CompareTo(tr1.blockItemDistance);
	}		

	private void FindBlockItem(Enemy.StateController controller)
	{
		CreateListOfBlockItems(controller);

		foreach (BlockItemClass blockItemTarget in blockItemsList)
		{
			foreach (Transform blockItemPointTarget in blockItemTarget.blockItem.GetComponent<BlockItem.StateController>().reachPoints)
			{
				if(controller.CheckPath(blockItemPointTarget))
				{	
					controller.blockItemPointTarget = blockItemPointTarget;
					controller.blockItemTarget = blockItemTarget.blockItem.transform;
					return;
				}
			}
		}
	}
}
