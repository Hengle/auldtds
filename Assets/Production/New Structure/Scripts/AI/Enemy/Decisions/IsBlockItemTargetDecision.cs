using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Enemy/isBloclItemTarget")]
public class IsBlockItemTargetDecision : EnemyDecision
{
	public override bool Decide(Enemy.StateController controller)
	{
		bool isBlockItemTarget = CheckBlockItemTarget(controller);
		return isBlockItemTarget ;
	}

	private bool CheckBlockItemTarget(Enemy.StateController controller)
	{
		if(controller.isChaseTargetReachable)
		{
			return false;
		}
		else
		{
			controller.savedUnitTarget = controller.unitTarget;
			controller.savedRoomTarget = controller.roomTarget;
			return true;
		}
	}
}