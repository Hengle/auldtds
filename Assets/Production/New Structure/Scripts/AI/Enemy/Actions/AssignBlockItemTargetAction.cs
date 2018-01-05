using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Enemy/AssignBlockItemTarget")]
public class AssignBlockItemTargetAction : EnemyAction
{
	public override void Act(Enemy.StateController controller)
	{
		AssignBlockItemTarget(controller);
 	}

	private void AssignBlockItemTarget(Enemy.StateController controller)
	{
		if(controller.blockItemPointTarget != null)
		{
			controller.chaseTarget = controller.blockItemPointTarget;
			controller.blockItemTarget = controller.blockItemPointTarget.parent.parent;
		}
	}
}