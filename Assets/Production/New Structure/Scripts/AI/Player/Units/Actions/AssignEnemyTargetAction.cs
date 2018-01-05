using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/PlayerUnit/AssignEnemyTarget")]
public class AssignEnemyTargetAction : PlayerUnitAction
{
	public override void Act(PlayerUnit.StateController controller)
	{
		AssignUnitTarget(controller);
	}

	private void AssignUnitTarget(PlayerUnit.StateController controller)
	{
		if(controller.enemyTarget != null)
		{
			controller.chaseTarget = controller.enemyTarget;
		}
	}
}