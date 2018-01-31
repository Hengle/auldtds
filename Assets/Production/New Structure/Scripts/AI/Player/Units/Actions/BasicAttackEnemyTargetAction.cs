using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "AI/Actions/PlayerUnit/BasicAttack")]
public class BasicAttackEnemyTargetAction : PlayerUnitAction
{
	public override void Act(PlayerUnit.StateController controller)
	{
		BasicAttackTarget(controller);
	}

	private void BasicAttackTarget(PlayerUnit.StateController controller)
	{
		if (controller.IsChaseTargetReached())
		{
			controller.FaceEnemy();
			controller.ExecuteAttack();
		}

	}
}