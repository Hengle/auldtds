using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Enemy/isRoomOrUnitTargetChanged")]
public class isRoomOrUnitChangedDecision : EnemyDecision
{
	public override bool Decide(Enemy.StateController controller)
	{
		bool isRoomOrUnitTargetChanged = CheckRoomOrUnitTarget(controller);
		return isRoomOrUnitTargetChanged ;
	}

	private bool CheckRoomOrUnitTarget(Enemy.StateController controller)
	{

		if((controller.unitTarget != controller.savedUnitTarget) || (controller.roomTarget != controller.savedRoomTarget) || (controller.CheckPath(controller.savedUnitTarget)) || (!controller.blockItemTarget))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}