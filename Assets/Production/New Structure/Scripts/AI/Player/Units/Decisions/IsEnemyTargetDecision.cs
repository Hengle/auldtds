using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/PlayerUnit/isEnemyTarget")]
public class IsEnemyTargetDecision : PlayerUnitDecision
{
	public override bool Decide(PlayerUnit.StateController controller)
	{
		bool isEnemyTarget = CheckEnemyTarget(controller);
		return isEnemyTarget ;
	}

	private bool CheckEnemyTarget(PlayerUnit.StateController controller)
	{
		if(controller.enemyTarget)
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