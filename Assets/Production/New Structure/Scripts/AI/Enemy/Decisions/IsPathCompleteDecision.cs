using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Enemy/isPathOpen")]
public class IsPathCompleteDecision : EnemyDecision
{
	public override bool Decide(Enemy.StateController controller)
	{
		bool isPathOpen = CheckTargetPath(controller);
		return isPathOpen;
	}

	private bool CheckTargetPath(Enemy.StateController controller)
	{
		
		if(controller.CheckPath(controller.chaseTarget))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}