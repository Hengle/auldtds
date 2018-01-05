using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Enemy/isUnitTarget")]
public class IsUnitTargetDecision : EnemyDecision
{
	public override bool Decide(Enemy.StateController controller)
	{
		bool isUnitTarget = CheckUnitTarget(controller);
		return isUnitTarget ;
	}

	private bool CheckUnitTarget(Enemy.StateController controller)
	{

		if(controller.unitTarget)
		{
			return true;
		}
		else
		{
			controller.attacking = false;
			return false;
		}
	}
}