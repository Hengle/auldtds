using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Enemy/BasicAttack")]
public class BasicAttackAction : EnemyAction
{
	public override void Act(Enemy.StateController controller)
	{
		if(controller.enemyStats.isAlive)
		{
			BasicAttackTarget(controller);
		}
	}

	private void BasicAttackTarget(Enemy.StateController controller)
	{
		if (controller.IsChaseTargetReached())
		{
			controller.FaceEnemy();
			controller.ExecuteAttack();

			UpdateBlockItemEngageList(controller);
		}
	}

	private void UpdateBlockItemEngageList(Enemy.StateController controller)
	{
		if (controller.chaseTarget == controller.blockItemPointTarget)
		{
			controller.blockItemTarget.GetComponent<BlockItem.StateController>().UpdateEngageList(controller.gameObject);
		}
			

	}
}