using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using PlayerUnit;

[CreateAssetMenu (menuName = "AI/Actions/Enemy/AssignUnitTarget")]
public class AssignUnitTargetAction : EnemyAction
{
	public override void Act(Enemy.StateController controller)
	{
		AssignUnitTarget(controller);
	}

	private void AssignUnitTarget(Enemy.StateController controller)
	{
		if(controller.unitTarget != null)
		{
			controller.chaseTarget = controller.unitTarget;
		}
	}
}