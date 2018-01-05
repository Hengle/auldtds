using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Enemy/isFinalTarget")]
public class IsFinalTargetDecision : EnemyDecision
{
	public override bool Decide(Enemy.StateController controller)
	{
		bool isFinalTarget = CheckFinalTarget(controller);
		return isFinalTarget ;
	}

	private bool CheckFinalTarget(Enemy.StateController controller)
	{

		if(controller.finalTarget && !controller.roomTarget && !controller.unitTarget)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}