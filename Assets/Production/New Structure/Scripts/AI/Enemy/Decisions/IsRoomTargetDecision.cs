using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Enemy/isRoomTarget")]
public class IsRoomTargetDecision : EnemyDecision
{
	public override bool Decide(Enemy.StateController controller)
	{
		bool isRoomTarget = CheckRoomTarget(controller);
		return isRoomTarget ;
	}

	private bool CheckRoomTarget(Enemy.StateController controller)
	{

		if(controller.roomTarget && !controller.unitTarget)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}