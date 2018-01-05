using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using PlayerUnit;

[CreateAssetMenu (menuName = "AI/Actions/Enemy/AssignRoomTarget")]
public class AssignRoomTargetAction : EnemyAction
{
	public override void Act(Enemy.StateController controller)
	{
		AssignRoomTarget(controller);
	}

	private void AssignRoomTarget(Enemy.StateController controller)
	{
		if(controller.roomTarget != null)
		{
			controller.chaseTarget = controller.roomTarget;
		}
	}
}