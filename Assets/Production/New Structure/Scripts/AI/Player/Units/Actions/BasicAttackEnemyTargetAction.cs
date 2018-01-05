using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using BlockItem;
using PlayerUnit;

[CreateAssetMenu (menuName = "AI/Actions/PlayerUnit/BasicAttack")]
public class BasicAttackEnemyTargetAction : PlayerUnitAction
{
	public override void Act(PlayerUnit.StateController controller)
	{
		BasicAttackTarget(controller);
	}

	private void BasicAttackTarget(PlayerUnit.StateController controller)
	{
		if (!controller.IsChaseTargetTheSame())
		{
			controller.StopBasicAttack();
		}

		if (controller.IsChaseTargetReached())
		{
			controller.FaceEnemy();
			controller.ExecuteBasicAttack();
		}

	}
}