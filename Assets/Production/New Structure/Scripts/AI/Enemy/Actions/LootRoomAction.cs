using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Enemy/LootRoom")]
public class LootRoomAction : EnemyAction
{
	public override void Act(Enemy.StateController controller)
	{
		if(controller.enemyStats.isAlive)
		{
			LootRoomTarget(controller);
		}
	}

	private void LootRoomTarget(Enemy.StateController controller)
	{
		if(controller.chaseTarget)
		{
			if(controller.chaseTarget == controller.lockedRoomTarget)
			{
				if (controller.IsChaseTargetReached())
				{
					controller.LootRoom();
				}
			}
		}
	}
}